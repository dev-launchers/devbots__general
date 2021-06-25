const BotPart = artifacts.require("BotPart");
const BotInstructionTokenFactory = artifacts.require("BotInstructionTokenFactory");
const DevLaunchersToken = artifacts.require("DevLaunchersToken");
const BotHull = artifacts.require("BotHull");
const LootBox = artifacts.require("LootBox");

function genProbabilities(arr){
  ret = [];
  aIndex = 0;
  i = 0;
  sum = 0;
  while(aIndex < arr.length){
    while(arr[aIndex] != i){
      ret[i] = sum;
      i++;
    }
    sum+=arr[aIndex+1];
    ret[i] = sum;
    aIndex+=2;
  }
  return ret;
}


module.exports = async function (deployer) {
  lootbox = await LootBox.deployed();

  lootbox.createLootBox(0, "1000000000000000000", 2, genProbabilities([0, 0x1000, 1, 0x2000, 2, 0x4000, 33, 0x6000, 34, 0x2FFF]));
};
