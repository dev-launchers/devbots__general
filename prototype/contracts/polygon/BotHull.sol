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
import "solidity-bytes-utils/contracts/BytesLib.sol";
import "./BotPart.sol";

contract BotHull is ERC721, IERC721Receiver, AccessControl {
  using Counters for Counters.Counter;

  IERC1820Registry private constant _ERC1820_REGISTRY = IERC1820Registry(0x1820a4B7618BdE71Dce8cdc73aAB6C95905faD24);

  address private botPartContract;
  address public metaTransactionForwarder;

  bytes32 public constant ERC777_INTERFACE = keccak256("ERC777Token");
  bytes32 public constant MINTER_ROLE = keccak256("MINTER_ROLE");

  Counters.Counter private _tokenIds;

  uint8 public constant MAX_PART_SLOTS = 4;


  mapping(uint256 => uint256[MAX_PART_SLOTS]) public partSlots;

  constructor(address _botPartContract, address _metaTransactionForwarder) ERC721("BotHull", "BH") {
    _ERC1820_REGISTRY.setInterfaceImplementer(address(this), 0xac7fbab5f54a3ca8194167523c6753bfeb96a445279294b6125b68cce2177054, address(this));

    botPartContract = _botPartContract;
    _setRoleAdmin(MINTER_ROLE, DEFAULT_ADMIN_ROLE);
    _setRoleAdmin(DEFAULT_ADMIN_ROLE, DEFAULT_ADMIN_ROLE);
    _setupRole(DEFAULT_ADMIN_ROLE, msg.sender);

    metaTransactionForwarder = _metaTransactionForwarder;
  }

  function mintHull(address player) public onlyRole(MINTER_ROLE) returns (uint256) {
    _tokenIds.increment();

    uint256 newItemId = _tokenIds.current();
    _mint(player, newItemId);

    return newItemId;
  }

  function fillSlot(uint256 botHullID, uint8 slotID, uint256 part, address from) public {
    require(slotID < MAX_PART_SLOTS);
    require(msg.sender == from || msg.sender == metaTransactionForwarder);
    require(ownerOf(botHullID) == from);
    require(BotPart(botPartContract).ownerOf(part) == from);
    require(partSlots[botHullID][slotID] == 0);

    _fillSlot(botHullID, slotID, part, from);
  }

  function getPartSlots(uint256 botHullID) view public returns (uint256[4] memory){
    return partSlots[botHullID];
  }

  function _fillSlot(uint256 botHullID, uint8 slotID, uint256 part, address from) private {
    BotPart(botPartContract).safeTransferFrom(from, address(this), part);
    partSlots[botHullID][slotID] = part;
  }

  function batchFillSlot(uint256[] calldata botHullIDs, uint8[] calldata slotIDs, uint256[] calldata parts, address from) public {
    require(msg.sender == from || msg.sender == metaTransactionForwarder);
    require(botHullIDs.length == slotIDs.length);
    require(slotIDs.length == parts.length);

    for(uint i = 0; i < botHullIDs.length; i++){
      fillSlot(botHullIDs[i], slotIDs[i], parts[i], from);
    }
  }

  function emptySlot(uint256 botHullID, uint8 slotID, address from) public {
    require(slotID < MAX_PART_SLOTS);
    require(msg.sender == from || msg.sender == metaTransactionForwarder);
    require(ownerOf(botHullID) == from);

    _emptySlot(botHullID, slotID, from);
  }

  function _emptySlot(uint256 botHullID, uint8 slotID, address from) private {
    BotPart(botPartContract).safeTransferFrom(address(this), from, partSlots[botHullID][slotID]);
    partSlots[botHullID][slotID] = 0;
  }

  function batchEmptySlot(uint256[] calldata botHullIDs, uint8[] calldata slotIDs, address from) public {
    require(botHullIDs.length == slotIDs.length);
    require(msg.sender == from || msg.sender == metaTransactionForwarder);

    for(uint i = 0; i < botHullIDs.length; i++){
      emptySlot(botHullIDs[i], slotIDs[i], from);
    }
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
      _emptySlot(botHullID, slotID, from);
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

}

