// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

import "@openzeppelin/contracts/access/AccessControl.sol";
import "@openzeppelin/contracts/token/ERC777/ERC777.sol";

contract BotInstructionToken is ERC777, AccessControl {

  bytes32 public constant MINTER_ROLE = keccak256("MINTER_ROLE");

  constructor(string memory _name, string memory _symbol, address minter, address[] memory _defaultOperators)
    ERC777(_name, _symbol, _defaultOperators)
  {
    _setRoleAdmin(MINTER_ROLE, DEFAULT_ADMIN_ROLE);
    _setRoleAdmin(DEFAULT_ADMIN_ROLE, DEFAULT_ADMIN_ROLE);
    _setupRole(DEFAULT_ADMIN_ROLE, msg.sender);
    _setupRole(MINTER_ROLE, minter);
  }

  function mint(address receiver, uint256 amount, bytes calldata userData, bytes calldata operatorData) public onlyRole(MINTER_ROLE) {
    _mint(receiver, amount, userData, operatorData);
  }

}

