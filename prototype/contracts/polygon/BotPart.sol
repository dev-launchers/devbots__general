// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

import "@openzeppelin/contracts/access/AccessControl.sol";
import "@openzeppelin/contracts/token/ERC721/presets/ERC721PresetMinterPauserAutoId.sol";
import "@openzeppelin/contracts/utils/Counters.sol";


contract BotPart is ERC721PresetMinterPauserAutoId {
  using Counters for Counters.Counter;

  Counters.Counter private _tokenIds;

  mapping(uint256 => uint32) public tokenPartType; // Lowest PartType starts at = 0x00000020 = 32
  mapping(uint256 => uint256[]) public tokenStats;
  
  address public metaTransactionForwarder;

  constructor(address _metaTransactionForwarder) ERC721PresetMinterPauserAutoId("BotPart", "BP", "testURI") {
    _setRoleAdmin(DEFAULT_ADMIN_ROLE, DEFAULT_ADMIN_ROLE);
    _setRoleAdmin(MINTER_ROLE, DEFAULT_ADMIN_ROLE);
    _setRoleAdmin(PAUSER_ROLE, DEFAULT_ADMIN_ROLE);

    metaTransactionForwarder = _metaTransactionForwarder;
  }

  function mint(address) pure public override {
    revert("Function not used!");
  }

  function mintItem(address player, uint32 _tokenPartType, uint256[] calldata _tokenStats) public onlyRole(MINTER_ROLE) returns (uint256) {
    _tokenIds.increment();

    uint256 newItemId = _tokenIds.current();
    _mint(player, newItemId);
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

  function metaApprove(address to, uint256 tokenId, address from) public {
        address owner = ERC721.ownerOf(tokenId);
        require(to != owner, "ERC721: approval to current owner");

        require(_msgSender() == metaTransactionForwarder);
        require(from != owner || isApprovedForAll(owner, from),
            "ERC721: approve caller is not owner nor approved for all"
        );

        _approve(to, tokenId);
  }

  function metaSafeTransfer(address to, uint256 tokenId, bytes memory _data, address from) public {
        require(_msgSender() == metaTransactionForwarder);
        _safeTransfer(from, to, tokenId, _data);
  }

}

