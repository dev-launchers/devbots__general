const NFT = artifacts.require('NFT')

module.exports = async(deployer, network, [defaultAccount]) => {
    deployer.deply(NFT)
}