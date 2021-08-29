const BotPart = artifacts.require("BotPart");
const DevLaunchersToken = artifacts.require("DevLaunchersToken");
const BotHull = artifacts.require("BotHull");
const LootBox = artifacts.require("LootBox");
const GameDatabase = artifacts.require("GameDatabase");

module.exports = async function (deployer, network, accounts) {
  lootbox = await LootBox.deployed();
  part = await BotPart.deployed();
  hull = await BotHull.deployed();
  devToken = await DevLaunchersToken.deployed();
  gameDatabase = await GameDatabase.deployed();

  gameDatabase.setBotHullContractAddress(hull.address);
  gameDatabase.setLootBoxContractAddress(lootbox.address);
  gameDatabase.setBotPartContractAddress(part.address);

  await lootbox.setDevsTokenContract(devToken.address);
  await part.grantRole("0x9f2df0fed2c77648de5860a4cc508cd0818c85b8b8a1ab4ceeef8d981c8956a6", lootbox.address);
  await hull.grantRole("0x9f2df0fed2c77648de5860a4cc508cd0818c85b8b8a1ab4ceeef8d981c8956a6", lootbox.address);
  await hull.grantRole("0x9f2df0fed2c77648de5860a4cc508cd0818c85b8b8a1ab4ceeef8d981c8956a6", accounts[0]);
  await devToken.grantRole("0x9f2df0fed2c77648de5860a4cc508cd0818c85b8b8a1ab4ceeef8d981c8956a6", accounts[0]);
 
};
