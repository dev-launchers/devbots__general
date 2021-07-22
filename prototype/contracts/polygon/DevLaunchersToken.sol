// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

import "@openzeppelin/contracts/access/AccessControl.sol";
import "@openzeppelin/contracts/token/ERC777/ERC777.sol";
import "./EIP712Forwarder.sol";

contract DevLaunchersToken is ERC777, AccessControl {

  bytes32 public constant MINTER_ROLE = keccak256("MINTER_ROLE");
  address public metaTransactionForwarder;

  constructor(address[] memory _defaultOperators, address _metaTransactionForwarder)
    ERC777("DEVS", "DEVS", _defaultOperators)
    {
      _setRoleAdmin(MINTER_ROLE, DEFAULT_ADMIN_ROLE);
      _setRoleAdmin(DEFAULT_ADMIN_ROLE, DEFAULT_ADMIN_ROLE);
      _setupRole(DEFAULT_ADMIN_ROLE, msg.sender);

      metaTransactionForwarder = _metaTransactionForwarder;
    }

    function mint(address receiver, uint256 amount, bytes calldata userData, bytes calldata operatorData) public onlyRole(MINTER_ROLE) {
      _mint(receiver, amount, userData, operatorData);
    }

    function metaSend(address recipient, uint256 amount, bytes calldata data, address from) public {
      require(msg.sender == metaTransactionForwarder);
      _send(from, recipient, amount, data, "", true);
    }


}

