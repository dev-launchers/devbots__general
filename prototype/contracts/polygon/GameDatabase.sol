// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

import "@openzeppelin/contracts/access/AccessControl.sol";
import "@openzeppelin/contracts/utils/math/SafeCast.sol";
import "abdk-libraries-solidity/ABDKMath64x64.sol";
import "./BotPart.sol";

contract GameDatabase is AccessControl {

    bytes32 public REGISTRY_ROLE = keccak256("REGISTRY_ROLE");

    struct StatDistribution {
        uint16 min;
        uint32 lowerRangeProbability;
        uint16 middle;
        uint16 max; 
    }


    // BotPart Index => Hash of Stats etc.
    mapping(uint256 => bytes32) private botPartSummaryHashes;

    // BotPart Type => Normal Distribution of Stats
    mapping(uint32 => StatDistribution[4]) private statDistributions;

    address private botPartContract;
    address private lootBoxContract;

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

    function registerStatDistribution(uint32 botPartType, uint8 statID, StatDistribution calldata distribution) public onlyRole(DEFAULT_ADMIN_ROLE) {
        statDistributions[botPartType][statID] = distribution;
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
            int128 y = ABDKMath64x64.div(ABDKMath64x64.pow(x, 2), ABDKMath64x64.fromUInt(2))*2;
            uint16 inverseRange = SafeCast.toUint16(ABDKMath64x64.toUInt(ABDKMath64x64.mul(y, ABDKMath64x64.fromUInt(range))));
            result+= (range - inverseRange);
        }else{
            result = distribution.middle;
            uint16 range = (distribution.max-distribution.middle);
            int128 x = ABDKMath64x64.divu(stat-distribution.lowerRangeProbability, ~distribution.lowerRangeProbability);
            int128 y = ABDKMath64x64.div(ABDKMath64x64.pow(x, 2), ABDKMath64x64.fromUInt(2))*2;
            uint16 calculatedRange = SafeCast.toUint16(ABDKMath64x64.toUInt(ABDKMath64x64.mul(y, ABDKMath64x64.fromUInt(range))));
            result+= calculatedRange;
        }
        return result;
    }

}