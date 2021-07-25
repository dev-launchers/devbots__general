import getWeb3 from "./getWeb3";

const interfaceBatchCall = {
    name: "batch",
    type: "function",
    inputs: [
        {
            type: "tuple[]",
            name: "calls",
            components: [
                {
                    name: "to",
                    type: "address"
                },
                { 
                    name: "data",
                    type: "bytes"
                },
                {
                    name: "value",
                    type: "uint256"
                }
            ]
        }
    ]
}

const interfaceMetaOpenLootBox = {
    name: "openLootBox",
    type: "function",
    inputs: [
        {
            type: "uint8",
            name: "boxID"
        },
        {
            type: "uint16",
            name: "seed"
        },
        {
            type: "address",
            name: "from"
        }
    ],
    outputs: [
        {
            type: "uint256[] memory"
        }
    ]
};

const createBatchedMetaTransaction = async function(web3, forwarder, contractAddresses, functionInterfaces, parameters, sentEthValues){
    var calls = [];

    for(var i = 0; i < contractAddresses.length; i++){
        calls.push({to:contractAddresses[i], data:web3.eth.abi.encodeFunctionCall(functionInterfaces[i], parameters[i]), value: sentEthValues[i]});
    }

    await createMetaTransaction(web3, forwarder, forwarder._address, interfaceBatchCall, [calls]);
}


const createMetaTransaction = async function (web3, forwarder, contractAddress, functionInterface, parameters, ethValue) {
    const data = web3.eth.abi.encodeFunctionCall(functionInterface, parameters);
    const chainId = Number(await web3.eth.net.getId());
    const fromAddress = (await web3.eth.getAccounts())[0];
    const nonce = await forwarder.methods.getNonce(fromAddress, 0).call();

    const msgParams = {
        domain: {
            name: 'Forwarder',
            version: "1"
        },
        // Defining the message signing data content.
        message: {
            from: fromAddress,
            to: contractAddress,
            chainId: chainId.toString(),
            value: "0",
            replayProtection: "0x0000000000000000000000000000000000000000",
            nonce: web3.eth.abi.encodeParameters(["uint128", "uint128"], [nonce, 0]),
            data: data,
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
    //"MetaTransaction(address from,address to,uint256 value,uint256 chainId,address replayProtection,bytes nonce,bytes data,bytes32 innerMessageHash)"

    console.log(msgParams);
        
    var params = [fromAddress, JSON.stringify(msgParams)];
    var method = 'eth_signTypedData_v4';

    web3.currentProvider.sendAsync(
        {
            method,
            params,
            from: fromAddress,
        },
        function (err, result) {
            let msg = msgParams.message;
            fetch("https://devbots.jectrum.de/relay?signer=" + fromAddress + "&to=" + msg.to + "&chainId=" + msg.chainId + "&nonce=" + msg.nonce + "&data=" + msg.data + "&innerMessageHash=" + "0x0000000000000000000000000000000000000000000000000000000000000000" + "&signature=" + result.result)
                .then(res => res.json())
                .then((res) => {
                    if(res.code == 0){
                        alert("Transaction was submitted!");
                    }else{
                        alert("Error: " + res.msg);
                    }
                });
        }
    );
};




export { createMetaTransaction, createBatchedMetaTransaction, interfaceMetaOpenLootBox };

