import React, { Component } from "react";
import { createMetaTransaction, createBatchedMetaTransaction, interfaceMetaOpenLootBox } from "./MetaTransaction"

class LootBox extends Component {

    state = { lootboxes: [] };

    componentDidMount = async () => {
        console.log("mounted Lootbox");
        this.setState({}, this.fetchLootBoxes);
    }

    fetchLootBoxes = async () => {
        var { lootboxes } = this.state;
        try {
            let i = lootboxes.length;
            let lootbox = [];
            var price = await this.props.lootboxContract.methods.lootBoxPrices(i).call();
            if(price <= 0)
                throw "";
            lootbox.push(i);
            lootbox.push(price);
            lootbox.push(await this.props.lootboxContract.methods.amountItemsPerBox(i).call());
            lootbox.push([]);
            lootboxes.push(lootbox);
            this.setState({ lootboxes }, this.fetchLootBoxes);
        } catch (err) {
            this.setState({}, () => {
                for(var i = 0; i < lootboxes.length; i++){
                    this.fetchLootBoxProbabilities(i);
                }
            });
        }
    };

    fetchLootBoxProbabilities = async (id) => {
        var { lootboxes } = this.state;

        let probabilities = [];
        let k = 0;
        let sum = 0;
        while (sum < 0xFFFF) {
            let prob = await this.props.lootboxContract.methods.probabilityDistribution(id, k).call();
            prob -= sum;
            if (prob > 0) {
                probabilities.push({ id: k, probability: prob, name: await this.toTokenName(k) });
                sum += prob;
            }
            k++;
        }
        lootboxes[id][3] = probabilities;
        this.setState({ lootboxes }, this.fetchLootBoxes);
    }

    toTokenName = async (id) => {
        return "BotPart #" + id;
    }

    openLootBox = async (lootBoxID) => {
        const { lootboxes } = this.state;

        const balance = await this.props.devLaunchersToken.methods.balanceOf(this.props.accounts[0]).call();

        console.log(lootboxes);
        console.log(balance);

        if (Number(balance) < Number(lootboxes[lootBoxID][1])) {
            alert("You don't have enough DEVS to open this LootBox!");
            return;
        }
        const parentUpdateState = this.props.parentUpdateState;
        this.props.lootboxContract.methods.openLootBox(lootBoxID, Math.round(Math.random() * 0xFFFF), this.props.accounts[0]).send({ from: this.props.accounts[0] }).on('receipt', function () {
            parentUpdateState();
        });
    }

    

    metaOpenLootBox = async (lootBoxID) => {
        const { lootboxes } = this.state;

        const balance = await this.props.devLaunchersToken.methods.balanceOf(this.props.accounts[0]).call();

        console.log(lootboxes);
        console.log(balance);

        if (Number(balance) < Number(lootboxes[lootBoxID][1])) {
            alert("You don't have enough DEVS to open this LootBox!");
            return;
        }
        const parentUpdateState = this.props.parentUpdateState;
        
        await createMetaTransaction(this.props.web3, this.props.forwarder, 
            this.props.lootboxContract._address, 
            interfaceMetaOpenLootBox, 
            [lootBoxID, Math.round(Math.random()) % 0xFFFF, this.props.accounts[0]],
            0);

          
        // this.props.lootboxContract.methods.openLootBox(lootBoxID, Math.round(Math.random() * 0xFFFF), this.props.accounts[0]).send({ from: this.props.accounts[0] }).on('receipt', function () {
        //     parentUpdateState();
        // });
    }

    updateState = async () => {
    }

    createLootBox = async () => {
        let k = 0;
        let sum = 0;
        let probDistribution = [];
        let boxID = this.state.lootboxes.length;
        let price = prompt("Price for the LootBox (10^18):") * 10 ** 18;
        if (price === 0)
            return;
        let itemAmount = prompt("Amount of Items in the LootBox:");
        if (itemAmount == null)
            return;
        while (sum < 0xFFFF) {
            let inputIndex = prompt("Next Item ID(id < 32 => InstructionToken)(Last one was '" + (k - 1) + "'): ");
            if (inputIndex < k) {
                alert("Next ID must be bigger than the last one you put in.");
                continue;
            }
            let probability = Number(prompt("Probability for Item ID " + inputIndex + "(Remaining: 0x" + (0xFFFF - sum).toString(16) + "):"));
            if (probability == null || probability > (0xFFFF - sum)) {
                alert("Invalid Probability specified!");
                continue;
            }
            while (k < inputIndex) {
                probDistribution.push(sum);
                k++;
            }
            sum += probability;
            probDistribution.push(sum);
            k++
        }
        await this.props.lootboxContract.methods.createLootBox(boxID, price.toString(10), itemAmount, probDistribution).send({ from: this.props.accounts[0] });
        this.setState({}, this.fetchLootBoxes);
    }

    render() {
        return (
            <div className="LootBox">
                <p>LootBoxes:</p>
                <div style={{ 'marginLeft': '40%', 'maxHeight': '400px', 'maxWidth': '20%', 'overflowY': 'scroll', border: '2px solid' }}>
                    {this.state.lootboxes.map(lootbox => (
                        <div key={"lootbox_" + lootbox[0]}>
                            <h4>LootBox #{lootbox[0]}</h4>
                            Price: {lootbox[1] / 10 ** 18}<br></br>
                            Items per Box: {lootbox[2]}<br></br>
                            Probabilities:
                            <ul>
                                {lootbox[3].map((probObj) =>
                                    <li key={probObj.id.toString()}>
                                        {probObj.name + ": " + Math.round((probObj.probability / 0xFFFF) * 100) + "%"}
                                    </li>
                                )}
                            </ul>
                            <button onClick={() => this.metaOpenLootBox(lootbox[0])}>Open LootBox</button>
                        </div>
                    ))}
                    <br></br>
                    <button onClick={this.createLootBox}>Create New LootBox</button>
                </div>
            </div>
        );
    }
}

export default LootBox;
