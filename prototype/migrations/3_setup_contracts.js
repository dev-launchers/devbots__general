const BotPart = artifacts.require("BotPart");
const BotInstructionTokenFactory = artifacts.require("BotInstructionTokenFactory");
const DevLaunchersToken = artifacts.require("DevLaunchersToken");
const BotHull = artifacts.require("BotHull");
const LootBox = artifacts.require("LootBox");

module.exports = async function (deployer) {
  lootbox = await LootBox.deployed();
  part = await BotPart.deployed();
  hull = await BotHull.deployed();
  devToken = await DevLaunchersToken.deployed();
  factory = await BotInstructionTokenFactory.deployed();

  await lootbox.setDevsTokenContract(devToken.address);
  await part.grantRole("0x9f2df0fed2c77648de5860a4cc508cd0818c85b8b8a1ab4ceeef8d981c8956a6", lootbox.address);
  await hull.grantRole("0x9f2df0fed2c77648de5860a4cc508cd0818c85b8b8a1ab4ceeef8d981c8956a6", lootbox.address);
  await devToken.grantRole("0x9f2df0fed2c77648de5860a4cc508cd0818c85b8b8a1ab4ceeef8d981c8956a6", "0xC1a467F46d65575724caD15a512249708F09745f");
 
};
