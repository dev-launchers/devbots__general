import React, { Component } from "react";

class BotHull extends Component {
    
    state = { botHulls: []};

    componentDidMount = async () => {

        this.setState({},this.fetchHulls);
    }

    fetchHulls = async () => {  
      // Get the value from the contract to prove it worked.
      const response = await this.props.token.methods.balanceOf(this.props.accounts[0]).call();
      console.log("Updated Balance: " + response);
      // Update state with the result.
      this.setState({ tokenBalance: response/10**18 });
    };
  
    mint = async () => {
        const minter_role = await this.props.botHullContract.methods.MINTER_ROLE().call();
        const isMinter = await this.props.botHullContract.methods.hasRole(minter_role, this.props.accounts[0]).call();
        if(!isMinter){
            alert("You don't have the Minter Role for BotHulls!");
            return;
        }
        try{
            const parentUpdateState = this.props.parentUpdateState;
        this.props.botHullContract.methods.mint(this.props.accounts[0], (amount * 10**18).toString(), "0x0", "0x0").send({from: this.props.accounts[0]}, function (err, transactionHash) {
            console.log(err);
            console.log(transactionHash);
            if(err != null){            
                alert("Transaction was rejected!");
            }
        })
        .once('receipt', function (){
            alert("BotHull was minted!");
            parentUpdateState();
        })
        }catch(er){
            console.log(er);
        }
    }

    grantMinter = async () => {
        const admin_role = await this.props.botHullContract.methods.DEFAULT_ADMIN_ROLE().call();
        const isAdmin = await this.props.botHullContract.methods.hasRole(admin_role, this.props.accounts[0]).call();
        if(!isAdmin){
            alert("You don't have the Admin Role for this Token!");
            return;
        }
        const minter_role = await this.props.botHullContract.methods.MINTER_ROLE().call();
        const address = prompt("Address to grant Role to:");
        const isMinter = await this.props.botHullContract.methods.hasRole(minter_role, address).call();
        if(isMinter){
            alert("Address already has Minter Role");
            return;
        }
        try{
        this.props.botHullContract.methods.grantRole(minter_role, address).send({from: this.props.accounts[0]}, function (err, transactionHash) {
            console.log(err);
            console.log(transactionHash);
            if(err != null){            
                alert("Transaction was rejected!");
            }
        })
        .once('receipt', function (){
            alert("Minter Role was granted!");
            this.setState({}, this.fetchBalance);
        })
        }catch(er){
            console.log(er);
        }
    }
  
    updateState = async () => {
        this.setState({}, this.fetchBalance);
    }

    render() {
      return (
        <div className="BotHull">
          Amount of BotHulls owned: {this.state.botHulls.length}<br></br>
          <button onClick={this.mint}>Mint</button><br></br>
          <button onClick={this.grantMinter}>Grant Minter Role</button><br></br>
          <button onClick={this.addToMetaMask}>Add to MetaMask</button>
        </div>
      );
    }
  }
  
  export default BotHull;
  