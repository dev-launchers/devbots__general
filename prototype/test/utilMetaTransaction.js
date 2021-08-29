const getMessage = require("eip-712").getMessage;
var Wallet = require('ethereumjs-wallet').default;
var EthUtil = require('ethereumjs-util');

const interfaceTokenMetaSend = exports.interfaceTokenMetaSend = {
    name: "metaSend",
    type: "function",
    inputs: [
        {
            type: "address",
            name: "recipient"
        },
        {
            type: "uint256",
            name: "amount"
        },
        {
            type: "bytes",
            name: "data"
        },
        {
            type: "address",
            name: "from"
        }
    ]
};

const generateNewWallet = exports.generateNewWallet = function() {
    var temp = '0b';
    for (let i = 0; i < 256; i++) {
      temp += Math.round(Math.random());
    }

    const randomPrivKey = BigInt(temp);
    const privateKeyBuffer = EthUtil.toBuffer("0x" + randomPrivKey.toString(16));
    const wallet = Wallet.fromPrivateKey(privateKeyBuffer);
    return wallet;
}

const sendMetaTransaction = exports.sendMetaTransaction = async function(forwarder, metaAccount, contractAddress, nonce, calldata, sender) {
    const chainId = Number(await web3.eth.net.getId());
    
    const msgParams = {
        domain: {
            name: 'Forwarder',
            version: "1"
        },
        // Defining the message signing data content.
        message: {
            from: metaAccount.getAddressString(),
            to: contractAddress,
            chainId: chainId.toString(),
            value: "0",
            replayProtection: "0x0000000000000000000000000000000000000000",
            nonce: web3.eth.abi.encodeParameters(["uint128", "uint128"], [nonce, 0]),
            data: calldata,
            innerMessageHash: "0x0"
        },
        // Refers to the keys of the *types* object below.
        primaryType: 'MetaTransaction',
        types: {
            // TODO: Clarify if EIP712Domain refers to the domain the contract is hosted on
            EIP712Domain: [
                { name: 'name', type: 'string' },
                { name: 'version', type: 'string' },
            ],
            MetaTransaction: [
                { name: 'from', type: 'address' },
                { name: 'to', type: 'address' },
                { name: 'value', type: 'uint256'},
                { name: 'chainId', type: 'uint256' },
                { name: 'replayProtection', type: 'address' },
                { name: 'nonce', type: 'bytes' },
                { name: 'data', type: 'bytes' },
                { name: 'innerMessageHash', type: 'bytes32' },
            ],
        },
    };

    const { r, s, v } = EthUtil.ecsign(getMessage(msgParams, true), metaAccount.getPrivateKey());
    const signature = EthUtil.toRpcSig(v,r,s);

    await forwarder.forward([metaAccount.getAddressString(), contractAddress, chainId, "0x0000000000000000000000000000000000000000", web3.eth.abi.encodeParameters(["uint128", "uint128"], [nonce, 0]), calldata, web3.eth.abi.encodeParameter("uint256", 0)], "0x", signature, {from: sender});
}