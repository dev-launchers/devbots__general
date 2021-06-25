const BotPart = artifacts.require("BotPart");
const BotInstructionTokenFactory = artifacts.require("BotInstructionTokenFactory");
const DevLaunchersToken = artifacts.require("DevLaunchersToken");
const BotHull = artifacts.require("BotHull");
const LootBox = artifacts.require("LootBox");

module.exports = async function (deployer) {
  hull = await BotHull.deployed();
  factory = await BotInstructionTokenFactory.deployed();
  lootbox = await LootBox.deployed();

  await factory.newInstruction(hull.address, 0, "First", "ON", lootbox.address, [lootbox.address]);
  await factory.newInstruction(hull.address, 1, "Second", "SE", lootbox.address, [lootbox.address]);
  await factory.newInstruction(hull.address, 2, "Third", "TH", lootbox.address, [lootbox.address]);

};
