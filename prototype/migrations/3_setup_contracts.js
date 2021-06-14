const BotPart = artifacts.require("BotPart");
const BotInstructionToken = artifacts.require("BotInstructionToken");
const DevLaunchersToken = artifacts.require("DevLaunchersToken");
const BotHull = artifacts.require("BotHull");
const LootBox = artifacts.require("LootBox");


module.exports = async function (deployer) {
  lootbox = await LootBox.deployed();
  part = await BotPart.deployed();
  hull = await BotHull.deployed();
  devToken = await DevLaunchersToken.deployed();
  insToken = await BotInstructionToken.deployed();

  await lootbox.setDevsTokenContract(devToken.address);
  await part.grantRole("0xf0887ba65ee2024ea881d91b74c2450ef19e1557f03bed3ea9f16b037cbe2dc9", lootbox.address);
  await hull.grantRole("0xf0887ba65ee2024ea881d91b74c2450ef19e1557f03bed3ea9f16b037cbe2dc9", lootbox.address);
  await devToken.grantRole("0xf0887ba65ee2024ea881d91b74c2450ef19e1557f03bed3ea9f16b037cbe2dc9", "0xC1a467F46d65575724caD15a512249708F09745f");
 
  await hull.registerInstruction("0", insToken.address);
};
