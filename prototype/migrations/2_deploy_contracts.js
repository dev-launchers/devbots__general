const BotPart = artifacts.require("BotPart");
const BotInstructionToken = artifacts.require("BotInstructionToken");
const DevLaunchersToken = artifacts.require("DevLaunchersToken");
const BotHull = artifacts.require("BotHull");
const LootBox = artifacts.require("LootBox");


module.exports = async function (deployer) {
  await deployer.deploy(BotPart);
  part = await BotPart.deployed();
  await deployer.deploy(BotHull, part.address);
  hull = await BotHull.deployed();
  await deployer.deploy(LootBox, "0x000000000000000000000000000000000000dEaD", hull.address, part.address);
  lootbox = await LootBox.deployed();
  await deployer.deploy(DevLaunchersToken, [lootbox.address.toString()]);
  devToken = await DevLaunchersToken.deployed();
  await deployer.deploy(BotInstructionToken, "TE", "ST", lootbox.address, [lootbox.address.toString()]);
  insToken = await BotInstructionToken.deployed();
};
