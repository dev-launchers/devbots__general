import React, { Component } from "react";

class Token extends Component {
    
    state = {tokenName: null, tokenBalance: 0};

    componentDidMount = async () => {
        const tokenName = await this.props.token.methods.name().call();

        this.setState({tokenName},this.fetchBalance);
    }

    fetchBalance = async () => {  
      // Get the value from the contract to prove it worked.
      const response = await this.props.token.methods.balanceOf(this.props.accounts[0]).call();
      console.log("Updated Balance: " + response);
      // Update state with the result.
      this.setState({ tokenBalance: response/10**18 });
    };
  
    mint = async () => {
        const minter_role = await this.props.token.methods.MINTER_ROLE().call();
        const isMinter = await this.props.token.methods.hasRole(minter_role, this.props.accounts[0]).call();
        if(!isMinter){
            alert("You don't have the Minter Role for this Token!");
            return;
        }
        const amount = prompt("Amount of Tokens to mint(10^18):");
        if(amount==null){
            return;
        }
        try{
            const parentUpdateState = this.props.parentUpdateState;
        this.props.token.methods.mint(this.props.accounts[0], (amount * 10**18).toString(), "0x0", "0x0").send({from: this.props.accounts[0]}, function (err, transactionHash) {
            console.log(err);
            console.log(transactionHash);
            if(err != null){            
                alert("Transaction was rejected!");
            }
        })
        .once('receipt', function (){
            alert("Tokens were minted!");
            parentUpdateState();
        })
        }catch(er){
            console.log(er);
        }
    }

    grantMinter = async () => {
        const admin_role = await this.props.token.methods.DEFAULT_ADMIN_ROLE().call();
        const isAdmin = await this.props.token.methods.hasRole(admin_role, this.props.accounts[0]).call();
        if(!isAdmin){
            alert("You don't have the Admin Role for this Token!");
            return;
        }
        const minter_role = await this.props.token.methods.MINTER_ROLE().call();
        const address = prompt("Address to grant Role to:");
        const isMinter = await this.props.token.methods.hasRole(minter_role, address).call();
        if(isMinter){
            alert("Address already has Minter Role");
            return;
        }
        try{
        this.props.token.methods.grantRole(minter_role, address).send({from: this.props.accounts[0]}, function (err, transactionHash) {
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

    addToMetaMask = async () => {
        const params = {
            type: 'ERC20',
            options: {
              address: this.props.token._address,
              symbol: await this.props.token.methods.symbol().call(),
              decimals: await this.props.token.methods.decimals().call()
            }
        };
        console.log(params);
        await window.ethereum.request({
            method: 'wallet_watchAsset',
            params: params,
        });
    }
  
    updateState = async () => {
        this.setState({}, this.fetchBalance);
    }

    render() {
      return (
        <div className="Token">
          <p>{this.state.tokenName}:<br></br>
          Balance: {this.state.tokenBalance}<br></br>
          <button onClick={this.mint}>Mint</button><br></br>
          <button onClick={this.grantMinter}>Grant Minter Role</button><br></br>
          <button onClick={this.addToMetaMask}>Add to MetaMask</button></p>
        </div>
      );
    }
  }
  
  export default Token;
  