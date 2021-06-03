// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

import "@openzeppelin/contracts/access/AccessControl.sol";
import "@openzeppelin/contracts/token/ERC721/extensions/ERC721Enumerable.sol";
import "@openzeppelin/contracts/token/ERC721/IERC721Receiver.sol";
import "@openzeppelin/contracts/token/ERC777/IERC777.sol";
import "@openzeppelin/contracts/token/ERC777/IERC777Recipient.sol";
import "@openzeppelin/contracts/utils/Counters.sol";
import "@openzeppelin/contracts/utils/introspection/ERC165Checker.sol";
import "@openzeppelin/contracts/utils/introspection/IERC1820Registry.sol";
import "bytes/BytesLib.sol";
import "./BotPart.sol";

contract BotHull is ERC721, IERC721Receiver, IERC777Recipient, AccessControl {
  using Counters for Counters.Counter;

  address private botPartContract;

  bytes32 public constant MINTER_ROLE = keccak256("MINTER");
  bytes32 public constant ADMIN_ROLE = keccak256("ADMIN");

  Counters.Counter private _tokenIds;

  uint8 public constant MAX_PART_SLOTS = 8;
  uint8 public amountInstructions = 0;
  mapping(uint8 => address) public instructionsContracts;


  mapping(uint256 => uint256[MAX_PART_SLOTS]) public partSlots;
  mapping(uint256 => uint8[]) public instructionSlots;
  mapping(uint256 => uint16[]) public instructionData;

  constructor(address minter, address _botPartContract) ERC721("BotHull", "BH") {
    IERC1820Registry(0x1820a4B7618BdE71Dce8cdc73aAB6C95905faD24).setInterfaceImplementer(address(this), type(IERC777Recipient).interfaceId, address(this));

    botPartContract = _botPartContract;
    _setRoleAdmin(MINTER_ROLE, ADMIN_ROLE);
    _setRoleAdmin(ADMIN_ROLE, ADMIN_ROLE);
    _setupRole(ADMIN_ROLE, msg.sender);
    _setupRole(MINTER_ROLE, minter);
  }

  function registerInstruction(uint8 id, address instructionContract) public onlyRole(ADMIN_ROLE) {
    require(ERC165Checker.supportsInterface(instructionContract, type(IERC777).interfaceId));
    require(id <= amountInstructions);
    require(id < 32);
    instructionsContracts[id] = instructionContract;
    amountInstructions++;
  }

  function mintHull(address player) public onlyRole(MINTER_ROLE) returns (uint256) {
    _tokenIds.increment();

    uint256 newItemId = _tokenIds.current();
    _mint(player, newItemId);

    return newItemId;
  }

  function fillSlot(uint256 botHullID, uint8 slotID, uint256 part) public {
    require(slotID < MAX_PART_SLOTS);
    require(ownerOf(botHullID) == msg.sender);
    require(BotPart(botPartContract).ownerOf(part) == msg.sender);
    require(partSlots[botHullID][slotID] == 0);

    _fillSlot(botHullID, slotID, part);
  }

  function _fillSlot(uint256 botHullID, uint8 slotID, uint256 part) private {
    BotPart(botPartContract).safeTransferFrom(msg.sender, address(this), part);
    partSlots[botHullID][slotID] = part;
  }

  function batchFillSlot(uint256[] calldata botHullIDs, uint8[] calldata slotIDs, uint256[] calldata parts) public {
    require(botHullIDs.length == slotIDs.length);
    require(slotIDs.length == parts.length);

    for(uint i = 0; i < botHullIDs.length; i++){
      fillSlot(botHullIDs[i], slotIDs[i], parts[i]);
    }
  }

  function emptySlot(uint256 botHullID, uint8 slotID) public {
    require(slotID < MAX_PART_SLOTS);
    require(ownerOf(botHullID) == msg.sender);

    _emptySlot(botHullID, slotID);
  }

  function _emptySlot(uint256 botHullID, uint8 slotID) private {
    BotPart(botPartContract).safeTransferFrom(address(this), msg.sender, partSlots[botHullID][slotID]);
    partSlots[botHullID][slotID] = 0;
  }

  function batchEmptySlot(uint256[] calldata botHullIDs, uint8[] calldata slotIDs) public {
    require(botHullIDs.length == slotIDs.length);

    for(uint i = 0; i < botHullIDs.length; i++){
      emptySlot(botHullIDs[i], slotIDs[i]);
    }
  }

  function clearBotInstructions(uint256 botHullID) public {
    require(ownerOf(botHullID) == msg.sender);
    require(instructionSlots[botHullID].length != 0);

    uint256[] memory depositedInstructionTokens = new uint256[](amountInstructions);

    //Check Valid Instructions and valid Instructions Balances
    for(uint i = 0; i < instructionSlots[botHullID].length; i++){
      depositedInstructionTokens[instructionSlots[botHullID][i]]++;
    }

    instructionSlots[botHullID] = new uint8[](0);
    instructionData[botHullID] = new uint16[](0);

    // Ignore Instructions which have its Contract set to this contract (=tokenless Instructions)
    for(uint8 j = 0; j < depositedInstructionTokens.length; j++){
      if(instructionsContracts[j] != address(this)) {
        IERC777(instructionsContracts[j]).send(msg.sender, depositedInstructionTokens[j] * 10**18, "");
      }
    }

  }

  function setupBotInstructions(uint256 botHullID, uint8[] calldata instructions, uint16[] calldata data) public {
    require(ownerOf(botHullID) == msg.sender);
    require(instructionSlots[botHullID].length == 0);

    uint256[] memory requiredAmounts = new uint256[](amountInstructions);

    //Check Valid Instructions and valid Instructions Balances
    for(uint i = 0; i < instructions.length; i++){
      requiredAmounts[instructions[i]]++;
    }
    // Ignore Instructions which have its Contract set to this contract (=tokenless Instructions)
    for(uint8 j = 0; j < requiredAmounts.length; j++){
      if(instructionsContracts[j] != address(this)) {
        require(IERC777(instructionsContracts[j]).balanceOf(msg.sender) >= requiredAmounts[j] * 10**18);
        IERC777(instructionsContracts[j]).operatorSend(msg.sender, address(this), requiredAmounts[j] * 10**18, "", "");
      }
    }

    instructionSlots[botHullID] = instructions;
    instructionData[botHullID] = data;
  }

  function onERC721Received(address operator, address from, uint256 tokenId, bytes calldata data) external override returns (bytes4){
    if(operator == address(this)){
      return IERC721Receiver.onERC721Received.selector;
    }
    require(msg.sender == botPartContract);
    require(operator == address(0));
    // 32 Bytes for BotHullID + 1 Byte for slotID
    require(data.length== 32 + 1);
    uint256 botHullID = BytesLib.toUint256(data, 0);
    uint8 slotID = BytesLib.toUint8(data, 32);

    require(ownerOf(botHullID) == from);
    require(slotID < MAX_PART_SLOTS);

    if(partSlots[botHullID][slotID] != 0){
      _emptySlot(botHullID, slotID);
    }
    partSlots[botHullID][slotID] = tokenId;

    return IERC721Receiver.onERC721Received.selector;
  }

  function supportsInterface(bytes4 interfaceId) public view virtual override(AccessControl, ERC721) returns (bool) {
    return interfaceId == type(IERC721Receiver).interfaceId ||
           interfaceId == type(IERC777Recipient).interfaceId ||
            ERC721.supportsInterface(interfaceId) ||
            AccessControl.supportsInterface(interfaceId);
  }

  function tokensReceived(
      address operator,
      address from,
      address to,
      uint256 amount,
      bytes calldata userData,
      bytes calldata operatorData
  ) external override {
    require(operator == address(this));
  }

}
