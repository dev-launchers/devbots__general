const BotPart = artifacts.require("BotPart");
const DevLaunchersToken = artifacts.require("DevLaunchersToken");
const BotHull = artifacts.require("BotHull");
const LootBox = artifacts.require("LootBox");

module.exports = async function (deployer, network, accounts) {
  lootbox = await LootBox.deployed();
  token = await DevLaunchersToken.deployed();

  await token.mint(accounts[0], "10000000000000000000", "0x0", "0x0");
  lootbox.openLootBox(0, 123, accounts[0]);
  lootbox.openLootBox(0, 124, accounts[0]);
  lootbox.openLootBox(0, 125, accounts[0]);
  lootbox.openLootBox(0, 126, accounts[0]);
  lootbox.openLootBox(0, 127, accounts[0]);
  lootbox.openLootBox(0, 128, accounts[0]);
  lootbox.openLootBox(0, 129, accounts[0]);
  lootbox.openLootBox(0, 130, accounts[0]);
};
