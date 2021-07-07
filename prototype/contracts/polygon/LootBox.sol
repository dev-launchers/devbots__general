// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

import "@openzeppelin/contracts/access/AccessControl.sol";
import "@openzeppelin/contracts/token/ERC721/IERC721.sol";
import "@openzeppelin/contracts/token/ERC777/ERC777.sol";
import "@openzeppelin/contracts/token/ERC777/IERC777Recipient.sol";
import "@openzeppelin/contracts/utils/introspection/IERC1820Registry.sol";
import "@openzeppelin/contracts/utils/introspection/ERC165Checker.sol";
import "solidity-bytes-utils/contracts/BytesLib.sol";
import "./DevLaunchersToken.sol";
import "./BotHull.sol";
import "./BotPart.sol";
import "./BotInstructionToken.sol";
import "@openzeppelin/contracts/utils/math/SafeCast.sol";

contract LootBox is AccessControl, IERC777Recipient {

  IERC1820Registry private constant _ERC1820_REGISTRY = IERC1820Registry(0x1820a4B7618BdE71Dce8cdc73aAB6C95905faD24);

  bytes32 public constant ERC777_INTERFACE = keccak256("ERC777Token");
  bytes32 public constant ERC721_INTERFACE = keccak256("ERC721Token");

  address public devsTokenContract;
  address public botHullContract;
  address public botPartContract;

  mapping(uint8 => uint256) public lootBoxPrices;
  mapping(uint8 => uint8) public amountItemsPerBox;
  mapping(uint8 => uint16[]) public probabilityDistribution; //boxID => Array(Index=partTypeID,val=sum(previous+current probability))

  constructor(address _devsTokenContract, address _botHullContract, address _botPartContract)
    {
      _ERC1820_REGISTRY.setInterfaceImplementer(address(this), 0xac7fbab5f54a3ca8194167523c6753bfeb96a445279294b6125b68cce2177054, address(this));

      devsTokenContract = _devsTokenContract;
      botHullContract = _botHullContract;
      botPartContract = _botPartContract;

      _setRoleAdmin(DEFAULT_ADMIN_ROLE, DEFAULT_ADMIN_ROLE);
      _setupRole(DEFAULT_ADMIN_ROLE, msg.sender);
    }

    function setDevsTokenContract(address _devsTokenContract) public onlyRole(DEFAULT_ADMIN_ROLE) {
      address implementer = _ERC1820_REGISTRY.getInterfaceImplementer(_devsTokenContract, ERC777_INTERFACE);
      require(implementer != address(0));
      devsTokenContract = _devsTokenContract;
    }

    function setBotHullContract(address _botHullContract) public onlyRole(DEFAULT_ADMIN_ROLE) {
      address implementer = _ERC1820_REGISTRY.getInterfaceImplementer(_botHullContract, ERC777_INTERFACE);
      require(implementer != address(0));
      botHullContract = _botHullContract;
    }

    function setBotPartContract(address _botPartContract) public onlyRole(DEFAULT_ADMIN_ROLE) {
      address implementer = _ERC1820_REGISTRY.getInterfaceImplementer(_botPartContract, ERC721_INTERFACE);
      require(implementer != address(0));
      botPartContract = _botPartContract;
    }

    function createLootBox(uint8 boxID, uint256 price, uint8 itemAmount, uint16[] calldata itemProbabilities) public onlyRole(DEFAULT_ADMIN_ROLE) {
      require(lootBoxPrices[boxID] == 0);
      require(amountItemsPerBox[boxID] == 0);
      uint16 prob = 0;
      for(uint i = 0; i < itemProbabilities.length;i++){
        require(itemProbabilities[i] >= prob);
        prob = itemProbabilities[i];
        require(prob <= 0xFFFF);
      }
      require(prob == 0xFFFF);

      lootBoxPrices[boxID] = price;
      amountItemsPerBox[boxID] = itemAmount;
      probabilityDistribution[boxID] = itemProbabilities;
    }

    // Seed is used here as INSECURE randomized number, needs to be replaced either by operator provided random or Chainlink Random
    function openLootBox(uint8 boxID, uint16 seed) public returns (uint256[] memory) {
      require(lootBoxPrices[boxID] != 0);
      require(amountItemsPerBox[boxID] != 0);

      DevLaunchersToken devsToken = DevLaunchersToken(devsTokenContract);
      require(devsToken.balanceOf(msg.sender) >= lootBoxPrices[boxID]);

      devsToken.operatorBurn(msg.sender, lootBoxPrices[boxID], "", "");

      return _openLootBox(msg.sender, boxID, seed);
    }

    function _openLootBox(address receiver, uint8 boxID, uint256 seed) private returns (uint256[] memory) {
      uint256[] memory items = new uint256[](0);
      for(uint item = 0; item < amountItemsPerBox[boxID]; item++){
        seed = _pseudoRand(seed, 21);
        uint i = 0;
        while(probabilityDistribution[boxID][i] < (seed & 0xFFFF)){
          i++;
        }

        if(i<32){
          //Mint Instruction Token
          address instructionTokenContract = BotHull(botHullContract).getInstructionContract(SafeCast.toUint8(i % 2**8));
          require(instructionTokenContract != address(0));
          BotInstructionToken(instructionTokenContract).mint(receiver, 1 * 10**18, "", "");
        }else{
          BotPart(botPartContract).mintItem(receiver, SafeCast.toUint32(i % 2**32), _generateStats(_pseudoRand(seed, SafeCast.toUint16(item))));
        }
      }
      return items;
    }

    function _generateStats(uint256 seed) private pure returns (uint256[] memory) {
      uint256[] memory stats = new uint256[](3);
      for(uint8 i = 0; i < 3; i++){
        stats[i] = _pseudoRand(seed, i);
      }
      return stats;
    }

    function _pseudoRand(uint256 seed, uint16 id) private pure returns (uint256){
      return uint256(keccak256(abi.encodePacked(seed, id)));
    }

    function tokensReceived(
        address operator,
        address from,
        address,
        uint256 amount,
        bytes calldata userData,
        bytes calldata
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

