// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

import "@openzeppelin/contracts/access/AccessControl.sol";
import "@openzeppelin/contracts/utils/math/SafeCast.sol";
import "abdk-libraries-solidity/ABDKMath64x64.sol";
import "./BotPart.sol";
import "./BotHull.sol";

contract GameDatabase is AccessControl {

    bytes32 public REGISTRY_ROLE = keccak256("REGISTRY_ROLE");

    struct StatDistribution {
        uint16 min;
        uint32 lowerRangeProbability;
        uint16 middle;
        uint16 max; 
    }

    // BotHoll Index => Hash of all Parts etc.
    mapping(uint256 => bytes32) private botHullSummaryHashes;

    // BotPart Index => Hash of Stats etc.
    mapping(uint256 => bytes32) private botPartSummaryHashes;

    // BotPart Type => Normal Distribution of Stats
    mapping(uint32 => StatDistribution[4]) private statDistributions;

    address private botPartContract;
    address private lootBoxContract;
    address private botHullContract;

    constructor() {
        _setRoleAdmin(DEFAULT_ADMIN_ROLE, DEFAULT_ADMIN_ROLE);
        _setRoleAdmin(REGISTRY_ROLE, DEFAULT_ADMIN_ROLE);
        _setupRole(DEFAULT_ADMIN_ROLE, msg.sender);
    }

    function setBotPartContractAddress(address _botPartContract) public onlyRole(DEFAULT_ADMIN_ROLE) {
        botPartContract = _botPartContract;
    }

    function setLootBoxContractAddress(address _lootBoxContract) public onlyRole(DEFAULT_ADMIN_ROLE) {
        lootBoxContract = _lootBoxContract;
    }

    function setBotHullContractAddress(address _botHullContract) public onlyRole(DEFAULT_ADMIN_ROLE) {
        botHullContract = _botHullContract;
    }

    function registerStatDistribution(uint32 botPartType, uint8 statID, StatDistribution calldata distribution) public onlyRole(DEFAULT_ADMIN_ROLE) {
        statDistributions[botPartType][statID] = distribution;
    }

    function getBotHullSummaryHash(uint256 botHullID, bool[5] calldata forceUpdates) public returns (bytes32){
        BotHull bh = BotHull(botHullContract);
        require(bh.ownerOf(botHullID) != address(0x0));

        if(botHullSummaryHashes[botHullID] != 0x0 && !forceUpdates[0]){
            return botHullSummaryHashes[botHullID];
        }

        uint256[4] memory botParts = bh.getPartSlots(botHullID);
        bytes32[4] memory hashes;
        for(uint8 i = 0; i < botParts.length; i++){
            hashes[i] = getBotPartSummaryHash(botParts[i], forceUpdates[i+1]);
        }
        botHullSummaryHashes[botHullID] = keccak256(abi.encodePacked(botHullID, hashes));
        return botHullSummaryHashes[botHullID];
    }

    function getBotPartSummaryHash(uint256 botPartID, bool forceUpdate) public returns (bytes32){
        BotPart bp = BotPart(botPartContract);
        require(bp.ownerOf(botPartID) != address(0x0));

        if(botPartSummaryHashes[botPartID] != 0x0 && !forceUpdate){
            return botPartSummaryHashes[botPartID];
        }

        uint32 partType = bp.getBotPartType(botPartID);
        uint16 statOne = getBotPartStat(botPartID, 0);
        uint16 statTwo = getBotPartStat(botPartID, 1);
        uint16 statThree = getBotPartStat(botPartID, 2);
        uint16 statFour = getBotPartStat(botPartID, 3);

        botPartSummaryHashes[botPartID] = keccak256(abi.encodePacked(botPartID, partType, statOne, statTwo, statThree, statFour));
        return botPartSummaryHashes[botPartID];
    }

    function resetBotPartSummaryHash(uint256 botPartID) public returns (bool) {
        require(botPartSummaryHashes[botPartID] != 0x0);
        require(!BotPart(botPartContract).exists(botPartID));
        botPartSummaryHashes[botPartID] = 0;
    }

    function getBotPartStat(uint256 botPartID, uint8 statID) view public returns (uint16) {
        require(statID < 4);

        BotPart bp = BotPart(botPartContract);
        require(bp.ownerOf(botPartID) != address(0x0));
        uint32 partType = bp.getBotPartType(botPartID);
        uint32 stat = bp.getRawTokenStat(botPartID, statID);
        StatDistribution storage distribution = statDistributions[partType][statID];

        uint16 result;
        if(stat <= distribution.lowerRangeProbability){
            result = distribution.min;
            uint16 range = (distribution.middle-distribution.min);
            int128 x = ABDKMath64x64.divu(distribution.lowerRangeProbability-stat, distribution.lowerRangeProbability);
            int128 y = ABDKMath64x64.pow(x, 2);
            uint16 inverseRange = SafeCast.toUint16(ABDKMath64x64.toUInt(ABDKMath64x64.mul(y, ABDKMath64x64.fromUInt(range))));
            result+= (range - inverseRange);
        }else{
            result = distribution.middle;
            uint16 range = (distribution.max-distribution.middle);
            int128 x = ABDKMath64x64.divu(stat-distribution.lowerRangeProbability, ~distribution.lowerRangeProbability);
            int128 y = ABDKMath64x64.pow(x, 2);
            uint16 calculatedRange = SafeCast.toUint16(ABDKMath64x64.toUInt(ABDKMath64x64.mul(y, ABDKMath64x64.fromUInt(range))));
            result+= calculatedRange;
        }
        return result;
    }

}