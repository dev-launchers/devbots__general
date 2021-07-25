const EIP712Forwarder = artifacts.require("EIP712Forwarder");
const BotPart = artifacts.require("BotPart");
const DevLaunchersToken = artifacts.require("DevLaunchersToken");
const BotHull = artifacts.require("BotHull");
const LootBox = artifacts.require("LootBox");
const GameDatabase = artifacts.require("GameDatabase");

require('@openzeppelin/test-helpers/configure')({
  provider: web3.currentProvider,
  singletons: {
    abstraction: 'truffle',
  },
});
const {singletons} = require("@openzeppelin/test-helpers");

module.exports = async function (deployer, network, accounts) {
  await singletons.ERC1820Registry(accounts[0]);
  await deployer.deploy(EIP712Forwarder);
  forwarder = await EIP712Forwarder.deployed();
  await deployer.deploy(BotPart, forwarder.address);
  part = await BotPart.deployed();
  await deployer.deploy(BotHull, part.address, forwarder.address);
  hull = await BotHull.deployed();
  await deployer.deploy(LootBox, "0x000000000000000000000000000000000000dEaD", hull.address, part.address, forwarder.address);
  lootbox = await LootBox.deployed();
  await deployer.deploy(DevLaunchersToken, [lootbox.address.toString()], forwarder.address);
  devToken = await DevLaunchersToken.deployed();
  await deployer.deploy(GameDatabase);
  gameDatabase = await GameDatabase.deployed();
};
