const truffleAssert = require('truffle-assertions');

const utilMeta = require("./utilMetaTransaction.js");

const BotPart = artifacts.require("BotPart");
const EIP712Forwarder = artifacts.require("EIP712Forwarder");

const mintRandomItem = async function(botPart, playerAddress, sender){

    statObject = [Math.round(Math.random()*Math.pow(2,32)), Math.round(Math.random()*Math.pow(2,32)), Math.round(Math.random()*Math.pow(2,32)), Math.round(Math.random()*Math.pow(2,32))];

    //Generate random uint128 Number
    stats = "0x" + statObject[0].toString(16).padStart(8, "0");
    stats+=statObject[1].toString(16).padStart(8, "0");
    stats+=statObject[2].toString(16).padStart(8, "0");
    stats+=statObject[3].toString(16).padStart(8, "0");

    await botPart.mintItem(playerAddress, Math.round(Math.random()*50)+1, stats, {from: sender});
    return statObject;
}

contract("BotPart", accounts => {

    it("Standard Minting Function disabled", async function () {
        const botPart = await BotPart.deployed();

        truffleAssert.reverts(botPart.mint(accounts[0]), "Function disabled");
    });

    it("Only Minter Role is allowed to mint", async function () {
        const botPart = await BotPart.deployed();

        truffleAssert.reverts(mintRandomItem(botPart, accounts[1], accounts[1]));
    });

    it("Test Minting BotParts", async function () {
        const botPart = await BotPart.deployed();

        await mintRandomItem(botPart, accounts[1], accounts[0]);
        await mintRandomItem(botPart, accounts[1], accounts[0]);
        await mintRandomItem(botPart, accounts[1], accounts[0]);
        await mintRandomItem(botPart, accounts[1], accounts[0]);
        await mintRandomItem(botPart, accounts[1], accounts[0]);
        await mintRandomItem(botPart, accounts[1], accounts[0]);
    
        expect((await botPart.balanceOf.call(accounts[1])).toNumber()).to.equal(6);
    });

    it("Test Transfering BotParts", async function () {
        const botPart = await BotPart.deployed();
        
        await botPart.safeTransferFrom(accounts[1], accounts[2], 1, {from: accounts[1]});

        expect((await botPart.balanceOf.call(accounts[2])).toNumber()).to.equal(1);
        expect((await botPart.ownerOf.call(1))).to.equal(accounts[2]);
    });

    it("Test Approve and Transfer", async function () {
        const botPart = await BotPart.deployed();

        await botPart.approve(accounts[3], 2, {from: accounts[1]});

        expect(await botPart.getApproved.call(2)).to.equal(accounts[3]);

        await botPart.safeTransferFrom(accounts[1], accounts[3], 2, {from: accounts[3]});

        expect((await botPart.balanceOf.call(accounts[3])).toNumber()).to.equal(1);
    });

    it("Test Burning Parts", async function () {
        const botPart = await BotPart.deployed();

        await botPart.burn(3, {from: accounts[1]});

        truffleAssert.reverts(botPart.ownerOf.call(3));
    });

    it("Only Owner is able to Burn", async function () {
        const botPart = await BotPart.deployed();

        truffleAssert.reverts(botPart.burn(4, {from: accounts[4]}));
    });

    it("Test Raw Token Stats", async function () {
        const botPart = await BotPart.deployed();

        const statOne = await mintRandomItem(botPart, accounts[4], accounts[0]);

        expect((await botPart.getRawTokenStat.call(7, 0)).toNumber()).to.equal(statOne[3]);
        expect((await botPart.getRawTokenStat.call(7, 1)).toNumber()).to.equal(statOne[2]);
        expect((await botPart.getRawTokenStat.call(7, 2)).toNumber()).to.equal(statOne[1]);
        expect((await botPart.getRawTokenStat.call(7, 3)).toNumber()).to.equal(statOne[0]);

        const statTwo = await mintRandomItem(botPart, accounts[4], accounts[0]);

        expect((await botPart.getRawTokenStat.call(8, 0)).toNumber()).to.equal(statTwo[3]);
        expect((await botPart.getRawTokenStat.call(8, 1)).toNumber()).to.equal(statTwo[2]);
        expect((await botPart.getRawTokenStat.call(8, 2)).toNumber()).to.equal(statTwo[1]);
        expect((await botPart.getRawTokenStat.call(8, 3)).toNumber()).to.equal(statTwo[0]);
    });


})