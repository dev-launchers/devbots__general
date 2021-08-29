// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

import "@openzeppelin/contracts/access/AccessControl.sol";
import "./BotPart.sol";
import "./BotHull.sol";
import "./GameDatabase.sol";
import "@openzeppelin/contracts/utils/cryptography/ECDSA.sol";
import "@openzeppelin/contracts/utils/cryptography/draft-EIP712.sol";
import "@openzeppelin/contracts/security/Pausable.sol";
import "@openzeppelin/contracts/security/ReentrancyGuard.sol";
import "abdk-libraries-solidity/ABDKMath64x64.sol";
import "./DevLaunchersToken.sol";

contract MatchPayout is AccessControl, EIP712, Pausable {

    uint256 constant public gameEnergyCost = 150; // 1 Unit of Energy = 1 Blockchain Block (~2seconds)
    uint256 constant public maxEnergy = 1500;

    address public gameDatabase;
    address public devLaunchersToken;
    address public botHullContract;

    address public matchSigner;

    mapping(uint256 => uint256) private lastGamePlayed;
    mapping(uint256 => uint256) private botEnergy;

    constructor() EIP712("DevBotsMatchPayout","0.1") {
        _setRoleAdmin(DEFAULT_ADMIN_ROLE, DEFAULT_ADMIN_ROLE);
        _setupRole(DEFAULT_ADMIN_ROLE, msg.sender);
    }

    function setGameDatabaseContract(address newGameDatabaseContract) external whenPaused() onlyRole(DEFAULT_ADMIN_ROLE) {
        gameDatabase = newGameDatabaseContract;
    }
    
    function setDevLaunchersTokenContract(address devLaunchersTokenContract) external whenPaused() onlyRole(DEFAULT_ADMIN_ROLE){
        devLaunchersToken = devLaunchersTokenContract;
    }

    function setBotHullContract(address botHullContractAddress) external whenPaused() onlyRole(DEFAULT_ADMIN_ROLE){
        botHullContract = botHullContractAddress;
    }

    function setMatchSignerAddress(address _matchSigner) external whenPaused() onlyRole(DEFAULT_ADMIN_ROLE){
        matchSigner = _matchSigner;
    }

    function getBotEnergy(uint256 botID, uint256 blockNumber) internal view returns (uint256){
        if(botEnergy[botID] == 0){
            return maxEnergy;
        }
        uint256 energy = botEnergy[botID] + (blockNumber - lastGamePlayed[botID]);
        if(energy > maxEnergy){
            return maxEnergy;
        }else{
            return energy;
        }
    }

    event Match(
        uint256 indexed attackerBotID,
        uint256 indexed defenderBotID,
        uint256 attackerBotHash,
        uint256 defenderBotHash,
        uint160 attackerAddress,
        uint32 attackerHealth,
        uint64 executedBlockNumber,
        uint160 defenderAddress,
        uint32 defenderHealth
    ) anonymous;

    struct MatchResult {
        uint256 attackerBotID;
        bytes32 attackerBotHash;
        uint256 defenderBotID;
        bytes32 defenderBotHash;

        uint160 attackerAddress;
        uint32 attackerHealth;
        uint64 blockNumber;
        
        uint160 defenderAddress;
        uint32 defenderHealth;
    }

    bytes32 payoutTypeHash = keccak256("payout(MatchResult(uint256 attackerBotID,bytes32 attackerBotHash,uint256 defenderBotID,bytes32 defenderBotHash,uint160 attackerAddress,unit32 attackerHealth,uint64 blockNumber, uint160 defenderAddress,uint32 defenderHealth) result,uint256 signature)");

    uint256 constant maxBlocksSinceSigning = 1000; // (~2 sec/block)

    function payout(MatchResult calldata result, bytes memory signature) public whenNotPaused() {
        require(result.blockNumber >= block.number - maxBlocksSinceSigning);
        require(result.blockNumber > lastGamePlayed[result.attackerBotID]);

        lastGamePlayed[result.attackerBotID] = result.blockNumber;

        uint256 energy = getBotEnergy(result.attackerBotID, result.blockNumber);
        require(energy >= gameEnergyCost);

        botEnergy[result.attackerBotID] = energy - gameEnergyCost;

        GameDatabase database = GameDatabase(gameDatabase);

        require(database.getBotHullSummaryHash(result.attackerBotID, [false, false, false, false, false]) == result.attackerBotHash);
        require(database.getBotHullSummaryHash(result.defenderBotID, [false, false, false, false, false]) == result.defenderBotHash);

        BotHull botHull = BotHull(botHullContract);

        require(botHull.ownerOf(result.attackerBotID) == address(result.attackerAddress));
        require(botHull.ownerOf(result.defenderBotID) == address(result.defenderAddress));

        bytes32 digest = _hashTypedDataV4(keccak256(abi.encode(
            payoutTypeHash,
            result.attackerBotID,
            result.attackerBotHash,
            result.defenderBotID,
            result.defenderBotHash,
            result.attackerAddress,
            result.attackerHealth,
            result.blockNumber,
            result.defenderAddress,
            result.defenderHealth
        )));

        require(matchSigner != address(0x0));
        require(ECDSA.recover(digest, signature) == matchSigner);

        DevLaunchersToken token = DevLaunchersToken(devLaunchersToken);

        uint256 winnerPayout = 0;
        uint256 loserPayout = 0;

        if(result.defenderHealth == 0){
            (winnerPayout, loserPayout) = getPayoutAmount(scores[result.attackerBotID], scores[result.defenderBotID], result.attackerHealth);
        }else{
            (winnerPayout, loserPayout) = getPayoutAmount(scores[result.defenderBotID], scores[result.attackerBotID], result.defenderHealth);
        }

        require(token.balanceOf(address(this)) >= (winnerPayout + loserPayout));

        updateScores(result.attackerBotID, result.defenderBotID, result.attackerHealth, result.defenderHealth);

        if(result.defenderHealth == 0){
            token.transfer(address(result.attackerAddress), winnerPayout);
            token.transfer(address(result.defenderAddress), loserPayout);
        }else{
            token.transfer(address(result.defenderAddress), winnerPayout);
            token.transfer(address(result.attackerAddress), loserPayout);
        }

    }

    uint256 constant minimumPayout = 1 * 10**17; // = 0.1 Tokens
    uint256 constant basePayout = 1 * 10**15;

    function getPayoutAmount(uint256 winnerScore, uint256 loserScore, uint32 winnerHealth) pure internal returns (uint256, uint256) {
        int128 multiplier = ABDKMath64x64.mul(ABDKMath64x64.fromUInt(winnerScore), ABDKMath64x64.divu(loserScore, winnerScore));

        uint256 amount = ABDKMath64x64.toUInt(multiplier) * basePayout;

        if(amount < minimumPayout){
            return (minimumPayout, minimumPayout);
        }else{
            return (amount, amount / 10);
        }
    }

    mapping(uint256 => uint256) public scores;

    uint256 constant minScoreGain = 10;
    uint256 constant scoreDivider = 10;

    function updateScores(uint256 attackerBotID, uint256 defenderBotID, uint32 attackerHealth, uint32 defenderHealth) internal {
        uint256 scoreGain = 0;
        uint256 aBotScore = scores[attackerBotID];
        uint256 dBotScore = scores[defenderBotID];
        
        if(attackerHealth > 0){
            //Attacker won
            if(aBotScore < dBotScore){
                scoreGain = (dBotScore - aBotScore) / scoreDivider;
            }
            if(scoreGain < minScoreGain){
                scores[attackerBotID]+=minScoreGain;
            }else{
                scores[attackerBotID]+=scoreGain;
                scores[defenderBotID]-=scoreGain/4;
            }
        }else{
            //Defender won
            if(dBotScore < aBotScore){
                scoreGain = (aBotScore - dBotScore) / scoreDivider;
            }
            if(scoreGain < minScoreGain){
                scores[attackerBotID]-=minScoreGain;
            }else{
                scores[attackerBotID]-=scoreGain;
                scores[defenderBotID]+=scoreGain/4;
            }
        }
    }

}