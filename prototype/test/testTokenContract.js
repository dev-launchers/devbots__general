const truffleAssert = require('truffle-assertions');

const utilMeta = require("./utilMetaTransaction.js");

const DevLaunchersToken = artifacts.require("DevLaunchersToken");
const EIP712Forwarder = artifacts.require("EIP712Forwarder");

const assertBalance = async function (token, account, expectedBalance) {
    balance = await token.balanceOf.call(account);
    assert.strictEqual(balance.toNumber(), expectedBalance, "Balance of " + account + " is supposed to be " + expectedBalance + " (Actual: " + balance.toNumber() + ")");
}

contract("DevLaunchersToken", accounts => {

    it("Test Minting Tokens", async function () {
        const toMint = 50000;
        
        const token = await DevLaunchersToken.deployed();        
        await token.mint(accounts[1], toMint, "0x", "0x", {from: accounts[0]});
        await assertBalance(token, accounts[1], toMint);

        const secondMinting = 120042;
        await token.mint(accounts[2], secondMinting, "0x", "0x", {from: accounts[0]});
        await assertBalance(token, accounts[2], secondMinting);


        tokenSupply = await token.totalSupply.call();
        assert.strictEqual(tokenSupply.toNumber(), toMint + secondMinting);

        await assertBalance(token, accounts[0], 0);
    });

    it("Test Minter Role Restriction", async function () {
        const token = await DevLaunchersToken.deployed();        

        await truffleAssert.reverts(
            token.mint(accounts[3], 3210, "0x", "0x", {from: accounts[3]})
            );
        
    });

    it("Test Sending Tokens", async function () {
        const token = await DevLaunchersToken.deployed();

        const balanceBefore = await token.balanceOf.call(accounts[1]);
        await token.transfer(accounts[3], 200, {from: accounts[1]});
        balance = balanceBefore - 200;

        await assertBalance(token, accounts[1], balance);

        await assertBalance(token, accounts[3], 200);

        await token.send(accounts[4], 500, "0x", {from: accounts[1]});
        balance = balanceBefore - 200 - 500;

        await assertBalance(token, accounts[1], balance);

        await assertBalance(token, accounts[4], 500);
    });

    it("Test Sending Tokens with Meta Transaction", async function () {
        const token = await DevLaunchersToken.deployed();

        metaAccount = utilMeta.generateNewWallet();

        await token.mint(metaAccount.getAddressString(), 500, "0x", "0x", {from: accounts[0]});

        data = web3.eth.abi.encodeFunctionCall(utilMeta.interfaceTokenMetaSend, [accounts[5], 300, "0x", metaAccount.getAddressString()]);

        await utilMeta.sendMetaTransaction(await EIP712Forwarder.deployed(), metaAccount, (await DevLaunchersToken.deployed()).address, 0, data, accounts[0]);
    
        await assertBalance(token, metaAccount.getAddressString(), 200);

        await assertBalance(token, accounts[5], 300);
    });

    it("Test Approve and transferFrom Token", async function () {
        const token = await DevLaunchersToken.deployed();

        await token.approve(accounts[6], 1500, {from: accounts[1]});

        expect((await token.allowance.call(accounts[1], accounts[6])).toNumber()).to.equal(1500);

        await token.transferFrom(accounts[1], accounts[6], 1500, {from: accounts[6]});

        await assertBalance(token, accounts[6], 1500);
    });

    it("Test authorizeOperator and operatorSend", async function() {
        const token = await DevLaunchersToken.deployed();

        await token.authorizeOperator(accounts[7], {from: accounts[1]});

        expect(await token.isOperatorFor.call(accounts[7], accounts[1])).to.equal(true);

        await token.operatorSend(accounts[1], accounts[7], 500, "0x", "0x", {from: accounts[7]});

        await assertBalance(token, accounts[7], 500);
    });


});