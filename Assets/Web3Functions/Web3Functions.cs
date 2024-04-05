using System;
using System.Text;
using DowntownProgram;
using DowntownProgram.Program;
using Solana.Unity.Programs;
using Solana.Unity.Rpc;
using Solana.Unity.Wallet;
using Unity.VisualScripting;
using UnityEngine;

public class Web3Functions : Singleton<Web3Functions>
{
    

    private static readonly IRpcClient rpcClient = ClientFactory.GetClient(Cluster.DevNet);
    private static readonly IStreamingRpcClient streamingRpcClient = ClientFactory.GetStreamingClient(Cluster.DevNet);
    public String InsertBuilding()
    {
        Debug.Log("Start transaction");
        Account userAccount = new("upKEJZSbNrhPXyj9BA4aNxdiuzLWQUhK1zgz8JHEiE7sakpvAKMqzyG6idV6ztVticFdQfCsmpkfMHgbP58zUx4", "9831HW6Ljt8knNaN6r6JEzyiey939A2me3JsdMymmz5J");
        PublicKey nft = new("FM6Yt3k8DKzj7fvJ6tJ9mWKX8BBWaLJtshebyMdt8KF");

        //
        var programId = ProgramHelper.programId;
        PublicKey user_nft_ata = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(userAccount.PublicKey, nft);
        PublicKey town_address = ProgramHelper.GetTownAccount(out byte bump_town_address);
        PublicKey nft_vault = ProgramHelper.GetAssetVaultAccount(nft, out byte bump_nft_vault);

        // Debug.Log("user_nft_ata: " + user_nft_ata);
        // Debug.Log("town_address: " + town_address);
        // Debug.Log("nft_vault: " + nft_vault);

        // return "";
        var downtownClient = new DowntownProgramClient(rpcClient, streamingRpcClient, programId);
        var accounts = new InsertHouseAccounts
        {
            Signer = userAccount,
            Town = town_address,
            NftMint = nft,
            UserNftAta = user_nft_ata,
            NftVault = nft_vault
        };
        byte houseVariant = 1;
        var tx = downtownClient.SendInsertHouseAsync(accounts, houseVariant, 0, 0, 0, userAccount.PublicKey, signingCallback, programId).Result.Result;
        Debug.Log(tx);
        return tx;
    }

    private byte[] signingCallback(byte[] arg1, PublicKey key)
    {
        Debug.Log("E be like say e workd");
        return arg1;
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