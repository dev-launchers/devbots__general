const GameDatabase = artifacts.require("GameDatabase");

module.exports = async function (deployer, network, accounts) {
  gameDatabase = await GameDatabase.deployed();
  gameDatabase.registerStatDistribution(0, 0, [10, 0x90000000, 40, 80]);
  gameDatabase.registerStatDistribution(0, 1, [20, 0x50000000, 70, 90]);
  gameDatabase.registerStatDistribution(0, 2, [30, 0x60000000, 50, 100]);
  gameDatabase.registerStatDistribution(0, 3, [40, 0x30000000, 60, 99]);

  gameDatabase.registerStatDistribution(1, 0, [40, 0x90000000, 63, 75]);
  gameDatabase.registerStatDistribution(1, 1, [30, 0x50000000, 70, 110]);
  gameDatabase.registerStatDistribution(1, 2, [30, 0xA0000000, 50, 120]);
  gameDatabase.registerStatDistribution(1, 3, [40, 0xB0000000, 60, 320]);
};


//         uint16 min;
//         uint32 lowerRangeProbability;
//         uint16 middle;
//         uint16 max;