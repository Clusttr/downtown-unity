using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DowntownProgram;
using DowntownProgram.Program;
using Solana.Unity.Programs;
using Solana.Unity.Rpc;
using Solana.Unity.Rpc.Builders;
using Solana.Unity.Rpc.Core.Http;
using Solana.Unity.Rpc.Messages;
using Solana.Unity.Rpc.Models;
using Solana.Unity.SDK;
using Solana.Unity.Wallet;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class FunctionsWeb3 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateWallet()
    {
        Web3.Instance.CreateAccount("private key", "password");
    }

    public void OnEnable()
    {
        Web3.OnLogin += OnLogin;
        Web3.OnLogout += OnLogout;
        Web3.OnBalanceChange += OnBalanceChange;
    }

    private void OnLogin(Account obj)
    {
        Console.WriteLine(obj.PublicKey);
    }

    private void OnLogout()
    {

    }

    private void OnBalanceChange(Double amount)
    {
        Console.WriteLine(amount);
    }

    private static readonly IRpcClient rpcClient = ClientFactory.GetClient(Cluster.DevNet);
    private static readonly IStreamingRpcClient streamingRpcClient = ClientFactory.GetStreamingClient(Cluster.DevNet);

    public async void insertNft()
    {
        // Wallet wallet = new Wallet("secret key");
        RequestResult<ResponseValue<LatestBlockHash>> blockhash = rpcClient.GetLatestBlockHashAsync().Result;
        ulong minRentExcept = rpcClient.GetMinimumBalanceForRentExemptionAsync(TokenProgram.TokenAccountDataSize).Result.Result;

        // Account owner = wallet.GetAccount(0);
        Account account = new Account("secret_key", "public_key");

        var txBuilder = new TransactionBuilder().
        SetRecentBlockHash(blockhash.Result.Value.Blockhash).
        SetFeePayer(account);

        PublicKey programId = new PublicKey("");
        PublicKey nft_mint = new PublicKey("");



        var txbyte = txBuilder.Build(new List<Account> { account });
        string txSignature = rpcClient.SendAndConfirmTransactionAsync(txbyte).Result.Result;
        Console.WriteLine("Tx signature here >>>" + txSignature);



    }

    public String insertBuilding(PublicKey user, PublicKey nft_mint)
    {
        var programId = ProgramHelper.programId;

        PublicKey user_nft_ata = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(user, nft_mint);
        PublicKey town_address = ProgramHelper.GetTownAccount(out byte bump_town_address);
        PublicKey nft_vault = ProgramHelper.GetAssetVaultAccount(nft_mint, out byte bump_nft_vault);

        var downtownClient = new DowntownProgramClient(rpcClient, streamingRpcClient, programId);
        var accounts = new InsertHouseAccounts
        {
            Signer = user,
            Town = town_address,
            NftMint = nft_mint,
            UserNftAta = user_nft_ata,
            NftVault = nft_vault
        };
        byte houseVariant = 1;
        var tx = downtownClient.SendInsertHouseAsync(accounts, houseVariant, 0, 0, 0, user, signingCallback, programId).Result.Result;
        return tx;
    }

    private byte[] signingCallback(byte[] arg1, PublicKey key)
    {
        throw new NotImplementedException();
    }

    public String withdrawBuilding(PublicKey user, PublicKey nft_mint)
    {
        var programId = ProgramHelper.programId;
        var downtownClient = new DowntownProgramClient(rpcClient, streamingRpcClient, programId);

        PublicKey town_address = ProgramHelper.GetTownAccount(out byte bump_town_address);
        PublicKey user_nft_ata = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(user, nft_mint);
        PublicKey nft_vault = ProgramHelper.GetAssetVaultAccount(nft_mint, out byte bump_nft_vault);

        var accounts = new WithdrawHouseAccounts
        {
            Signer = user,
            Town = town_address,
            UserNftAta = user_nft_ata,
            NftMint = nft_mint,
            NftVault = nft_vault
        };
        var tx = downtownClient.SendWithdrawHouseAsync(accounts, user, withdrawalCallback, programId).Result.Result;
        return tx;
    }

    private byte[] withdrawalCallback(byte[] arg1, PublicKey key)
    {
        throw new NotImplementedException();
    }

}


public class ProgramHelper
{
    public static PublicKey programId = new PublicKey("CgGCmVn7W9zjKjAqw3ypEQfEEiJGSM1u87AzyEC81m5b");

    public static PublicKey GetTownAccount(out byte vaultBump)
    {

        if (!PublicKey.TryFindProgramAddress(new[]
                    {
                        Encoding.UTF8.GetBytes("town"),
                    },
                    programId, out PublicKey vaultPda, out vaultBump))
        {
            Debug.LogError("Could not find vault address");
            return null;
        }

        return vaultPda;
    }

    public static PublicKey GetAssetVaultAccount(PublicKey asset, out byte vaultBump)
    {
        if (!PublicKey.TryFindProgramAddress(new[]
                    {
                        Encoding.UTF8.GetBytes("vault"), asset.KeyBytes
                    },
                    programId, out PublicKey vaultPda, out vaultBump))
        {
            Debug.LogError("Could not find vault address");
            return null;
        }

        return vaultPda;
    }
}