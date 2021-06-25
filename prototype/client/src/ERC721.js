import React, { Component } from "react";

class ERC721 extends Component {
    
    state = {tokenName: null, tokenBalance: 0, nfts: []};

    componentDidMount = async () => {
        const tokenName = await this.props.token.methods.name().call();

        this.setState({tokenName}, this.fetchBalance);
    }

    fetchBalance = async () => {  
      // Get the value from the contract to prove it worked.
      const tokenBalance = await this.props.token.methods.balanceOf(this.props.accounts[0]).call();
  
      // Update state with the result.

      let nfts = [];
      for(let i = 0; i < tokenBalance; i++){
        let nft = [];
        nft.push(await this.props.token.methods.tokenOfOwnerByIndex(this.props.accounts[0], i).call());
        nft.push(await this.props.token.methods.tokenPartType(nft[0]).call());
        let stats = [];
        try{
          let k = 0;
          while(true){
            stats.push(await this.props.token.methods.tokenStats(nft[0], k).call());
            k++;
          }
        }catch(err){
        }
        console.log(stats);
        nft.push(stats);
        nfts.push(nft);
      }

      this.setState({ tokenBalance, nfts });
    };
  
    updateState = async () => {
      this.setState({}, this.fetchBalance);
    }

    addToMetaMask = async () => {
        const params = {
            type: 'ERC721',
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
  
    render() {
      return (
        <div className="ERC721">
          <p>{this.state.tokenName}:<br></br>
          Amount of NFTs: {this.state.tokenBalance}<br></br>
          </p>
          <div style={{'marginLeft': '40%', 'maxHeight':'400px', 'maxWidth': '20%', 'overflowY':'scroll', border: '2px solid'}}>
          {this.state.nfts.map(nft => (
                <div>
                NFT #{nft[0]}<br></br>
                TokenPartType: {nft[1]}<br></br>
                Stats:
                <ul>
                {nft[2].map((stat) => 
                  <li key={stat.toString()}>
                    {Math.round(stat / 10**74)}
                  </li>
                )}
                </ul>
                </div>
          ))}
          </div>
        </div>
      );
    }
  }
  
  export default ERC721;
  