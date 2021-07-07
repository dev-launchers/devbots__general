const BotPart = artifacts.require("BotPart");
const BotInstructionTokenFactory = artifacts.require("BotInstructionTokenFactory");
const DevLaunchersToken = artifacts.require("DevLaunchersToken");
const BotHull = artifacts.require("BotHull");
const LootBox = artifacts.require("LootBox");

module.exports = async function (deployer) {
  lootbox = await LootBox.deployed();
  token = await DevLaunchersToken.deployed();

  await token.mint("0xC1a467F46d65575724caD15a512249708F09745f", "10000000000000000000", "0x0", "0x0");
  lootbox.openLootBox(0, 123);
  lootbox.openLootBox(0, 124);
  lootbox.openLootBox(0, 125);
  lootbox.openLootBox(0, 126);
  lootbox.openLootBox(0, 127);
  lootbox.openLootBox(0, 128);
  lootbox.openLootBox(0, 129);
  lootbox.openLootBox(0, 130);
};
