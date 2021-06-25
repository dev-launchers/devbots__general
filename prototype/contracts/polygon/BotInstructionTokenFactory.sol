// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

import "./BotInstructionToken.sol";
import "./BotHull.sol";

contract BotInstructionTokenFactory is AccessControl {

  constructor() {
    _setRoleAdmin(DEFAULT_ADMIN_ROLE, DEFAULT_ADMIN_ROLE);
    _setupRole(DEFAULT_ADMIN_ROLE, msg.sender);
  }

  function newInstruction(address botHullContract, uint8 insID, string memory _name, string memory _symbol, address minter, address[] memory _defaultOperators) public onlyRole(DEFAULT_ADMIN_ROLE) returns (address){
      BotInstructionToken ins = (new BotInstructionToken(_name, _symbol, minter, _defaultOperators));
      BotHull(botHullContract).registerInstruction(insID, address(ins));
      return address(ins);
  }

}