// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

import "@openzeppelin/contracts/access/AccessControl.sol";
import "@openzeppelin/contracts/token/ERC721/extensions/ERC721URIStorage.sol";
import "@openzeppelin/contracts/utils/Counters.sol";


contract BotPart is ERC721URIStorage, AccessControl {
  using Counters for Counters.Counter;

  bytes32 public constant MINTER_ROLE = keccak256("MINTER");
  bytes32 public constant ADMIN_ROLE = keccak256("ADMIN");

  Counters.Counter private _tokenIds;

  mapping(uint256 => uint32) tokenPartType; // Lowest PartType starts at = 0x00000020 = 32
  mapping(uint256 => uint256[]) tokenStats;


  constructor(address minter) ERC721("BotPart", "BP") {
    _setRoleAdmin(MINTER_ROLE, ADMIN_ROLE);
    _setRoleAdmin(ADMIN_ROLE, ADMIN_ROLE);
    _setupRole(ADMIN_ROLE, msg.sender);
    _setupRole(MINTER_ROLE, minter);
  }

  function mintItem(address player, uint32 _tokenPartType, uint256[] calldata _tokenStats, string memory tokenURI) public onlyRole(MINTER_ROLE) returns (uint256) {
    _tokenIds.increment();

    uint256 newItemId = _tokenIds.current();
    _mint(player, newItemId);
    _setTokenURI(newItemId, tokenURI);
    _setTokenPartType(newItemId, _tokenPartType);
    _setTokenStats(newItemId, _tokenStats);

    return newItemId;
  }

  function _setTokenPartType(uint256 tokenID, uint32 partType) private {
    tokenPartType[tokenID] = partType;
  }

  function _setTokenStats(uint256 tokenID, uint256[] calldata _tokenStats) private {
    tokenStats[tokenID] = _tokenStats;
  }

  function supportsInterface(bytes4 interfaceId) public view virtual override(AccessControl, ERC721) returns (bool) {
    return ERC721.supportsInterface(interfaceId) || AccessControl.supportsInterface(interfaceId);
  }

}
