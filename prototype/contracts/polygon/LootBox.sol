// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

import "@openzeppelin/contracts/access/AccessControl.sol";
import "@openzeppelin/contracts/token/ERC721/IERC721.sol";
import "@openzeppelin/contracts/token/ERC777/ERC777.sol";
import "@openzeppelin/contracts/token/ERC777/IERC777Recipient.sol";
import "@openzeppelin/contracts/utils/introspection/IERC1820Registry.sol";
import "@openzeppelin/contracts/utils/introspection/ERC165Checker.sol";
import "bytes/BytesLib.sol";
import "./DevLaunchersToken.sol";
import "./BotHull.sol";
import "./BotPart.sol";
import "./BotInstructionToken.sol";

contract LootBox is AccessControl, IERC777Recipient {

  bytes32 public constant ADMIN_ROLE = keccak256("ADMIN");

  address public devsTokenContract;
  address public botHullContract;
  address public botPartContract;

  mapping(uint8 => uint256) lootBoxPrices;
  mapping(uint8 => uint8) amountItemsPerBox;
  mapping(uint8 => uint16[]) probabilityDistribution; //boxID => Array(Index=partTypeID,val=sum(previous+current probability))

  constructor(address _devsTokenContract, address _botHullContract, address _botPartContract)
    {
      IERC1820Registry(0x1820a4B7618BdE71Dce8cdc73aAB6C95905faD24).setInterfaceImplementer(address(this), type(IERC777Recipient).interfaceId, address(this));

      devsTokenContract = _devsTokenContract;
      botHullContract = _botHullContract;
      botPartContract = _botPartContract;

      _setRoleAdmin(ADMIN_ROLE, ADMIN_ROLE);
      _setupRole(ADMIN_ROLE, msg.sender);
    }

    function setDevsTokenContract(address _devsTokenContract) public onlyRole(ADMIN_ROLE) {
      require(ERC165Checker.supportsInterface(_devsTokenContract, type(IERC777).interfaceId));
      devsTokenContract = _devsTokenContract;
    }

    function setBotHullContract(address _botHullContract) public onlyRole(ADMIN_ROLE) {
      require(ERC165Checker.supportsInterface(_botHullContract, type(IERC777).interfaceId));
      botHullContract = _botHullContract;
    }

    function setBotPartContract(address _botPartContract) public onlyRole(ADMIN_ROLE) {
      require(ERC165Checker.supportsInterface(_botPartContract, type(IERC721).interfaceId));
      botPartContract = _botPartContract;
    }

    // Seed is used here as INSECURE randomized number, needs to be replaced either by operator provided random or Chainlink Random
    function openLootBox(uint8 boxID, uint16 seed) public returns (uint256[]) {
      require(lootBoxPrices[boxID] != 0);
      require(amountItemsPerBox[boxID] != 0);

      DevLaunchersToken devsToken = DevLaunchersToken(devsTokenContract);
      require(devsToken.balanceOf(msg.sender) >= lootBoxPrices[boxID]);

      devsToken.operatorBurn(msg.sender, lootBoxPrices[boxID], "", "");

      return _openLootBox(msg.sender, boxID, seed);
    }

    function _openLootBox(address receiver, uint8 boxID, uint256 seed) private returns (uint256[]) {
      uint256[] items = new uint256[];
      for(uint item = 0; item < amountItemsPerBox[boxID]; item++){
        seed = _pseudoRand(seed, 21);
        uint i = 0;
        while(probabilityDistribution[boxID][i] < seed){
          i++;
        }

        if(i<32){
          //Mint Instruction Token
          address instructionTokenContract = BotHull(botHullContract).instructionsContracts[i];
          require(instructionTokenContract != address(0));
          BotInstructionToken(instructionTokenContract).mint(receiver, 1 * 10**18, "", "");
        }else{
          BotPart(botPartContract).mintItem(receiver, i, _generateStats(_pseudoRand(seed, item)), "testURI");
        }
      }
    }

    function _generateStats(uint256 seed) private returns (uint256[]) {
      uint256[] stats = new uint256[](3);
      for(uint8 i = 0; i < 3; i++){
        stats[i] = _pseudoRand(seed, i);
      }
      return stats;
    }

    function _pseudoRand(uint256 seed, uint16 id) private returns (uint256){
      return uint256(keccak256(abi.encodePacked(seed, id)));
    }

    function tokensReceived(
        address operator,
        address from,
        address to,
        uint256 amount,
        bytes calldata userData,
        bytes calldata operatorData
    ) external override {
      require(msg.sender == devsTokenContract);
      require(operator == address(0) || userData.length == 1);
      uint8 boxID = BytesLib.toUint8(userData, 0);

      require(lootBoxPrices[boxID] != 0);
      require(lootBoxPrices[boxID] == amount);
      require(amountItemsPerBox[boxID] != 0);

      DevLaunchersToken(devsTokenContract).burn(amount, "");

      // SEED Mechanism needs to be changed ASAP
      _openLootBox(from, boxID, 542168123);
    }

}
