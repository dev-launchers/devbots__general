import React, { Component } from "react";
import BotHullContract from "./contracts/BotHull.json";
import BotInstructionTokenFactoryContract from "./contracts/BotInstructionTokenFactory.json";
import BotInstructionTokenContract from "./contracts/BotInstructionToken.json";
import BotPartContract from "./contracts/BotPart.json";
import DevLaunchersTokenContract from "./contracts/DevLaunchersToken.json";
import LootBoxContract from "./contracts/LootBox.json";
import getWeb3 from "./getWeb3";
import Token from "./Token";
import ERC721 from "./ERC721";
import LootBox from "./LootBox"

import "./App.css";

class App extends Component {
  childs = {};
  state = { web3: null, accounts: null, botHull: null, botInstructionTokenFactory: null, botPart: null, devLaunchersToken: null, 
	    lootBox: null, instructionTokens: [], displayERC777Tokens: false };

  componentDidMount = async () => {
    try {
      // Get network provider and web3 instance.
      const web3 = await getWeb3();

      // Use web3 to get the user's accounts.
      const accounts = await web3.eth.getAccounts();

      // Get the contract instance.
      const networkId = await web3.eth.net.getId();
      let deployedNetwork = BotHullContract.networks[networkId];
      const botHull = new web3.eth.Contract(
	BotHullContract.abi,
	deployedNetwork && deployedNetwork.address,
      );
      deployedNetwork = BotInstructionTokenFactoryContract.networks[networkId];
      const botInstructionTokenFactory = new web3.eth.Contract(
        BotInstructionTokenFactoryContract.abi,
        deployedNetwork && deployedNetwork.address,
      );
      deployedNetwork = BotPartContract.networks[networkId];
      const botPart = new web3.eth.Contract(
        BotPartContract.abi,
        deployedNetwork && deployedNetwork.address,
      );
      deployedNetwork = DevLaunchersTokenContract.networks[networkId];
      const devLaunchersToken = new web3.eth.Contract(
        DevLaunchersTokenContract.abi,
        deployedNetwork && deployedNetwork.address,
      );
      const obj = {type: web3.utils.toHex("ERC20"), options: {address: deployedNetwork.address}};
      console.log(obj);
      deployedNetwork = LootBoxContract.networks[networkId];
      const lootBox = new web3.eth.Contract(
        LootBoxContract.abi,
        deployedNetwork && deployedNetwork.address,
      );

      // Set web3, accounts, and contract to the state, and then proceed with an
      // example of interacting with the contract's methods. 
      this.setState({ web3, accounts, botHull, botInstructionTokenFactory, botPart, devLaunchersToken, lootBox }, this.fetchInstructionTokens);
    } catch (error) {
      // Catch any errors for any of the above operations.
      alert(
        `Failed to load web3, accounts, or contract. Check console for details.`,
      );
      console.error(error);
    }
  };

  fetchInstructionTokens = async () => {
    const { web3, botHull } = this.state;

    const amountInstructions = await botHull.methods.amountInstructions().call();
    let instructionTokens = [];
    for(let i = 0; i < amountInstructions; i++){
      let address = await botHull.methods.getInstructionContract(i).call();
      const token = new web3.eth.Contract(
        BotInstructionTokenContract.abi,
        address
      );
      instructionTokens.push(token);
    }

    // Update state with the result.
    this.setState({ instructionTokens });
  };

  updateState = async () => {
    for(var key in this.childs){
      this.childs[key].updateState();
    };
  }

  render() {
    if (!this.state.web3) {
      return <div>Loading Web3, accounts, and contract...</div>;
    }
    return (
      <div className="App">
        <h2>Tokens:</h2>
        <Token accounts={this.state.accounts} token={this.state.devLaunchersToken} parentUpdateState={this.updateState} ref={(node) => {this.childs["devs"] = node}}></Token>
        <button onClick={() => this.setState({ displayERC777Tokens: !this.state.displayERC777Tokens })}>Toggle Instruction Tokens</button>
        <div style={{display:this.state.displayERC777Tokens ? 'block' : 'none'}}>
        {this.state.instructionTokens.map(tok => (<Token accounts={this.state.accounts} token={tok} parentUpdateState={this.updateState} ref={(node) => {this.childs[tok._address] = node}}/>))}
        </div>
        <ERC721 accounts={this.state.accounts} token={this.state.botPart} ref={(node) => {this.childs["erc721"] = node}}></ERC721>
        { this.state.instructionTokens.length > 0 && (<LootBox accounts={this.state.accounts} lootboxContract={this.state.lootBox} devLaunchersToken={this.state.devLaunchersToken} insTokens={this.state.instructionTokens} parentUpdateState={this.updateState} ref={(node) => {this.childs["lootbox"] = node}}></LootBox>)}
       </div>


    );
  }
}

export default App;
