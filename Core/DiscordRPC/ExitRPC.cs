using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using DiscordRPC;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;

public class ExitRPC
{
    private static DiscordRpcClient client;

    public static async void Start()
    {
        if (client == null)
        {
            client = new DiscordRpcClient("put your discord bot id here so discord rpc works but this config is for my client so change it");

            client.Initialize();
        }

        if (client.IsInitialized)
        {
            if (Definitions.DiscordPRC == true)
            {
                if (Definitions.UserName == null)
                {
                    while (Definitions.UserName == null)
                    {
                        await Task.Delay(1000); 
                    }
                    UpdatePresence("Logged in as " + Definitions.UserName, "");
                }
            }
        }

    }

    public static void Stop()
    {
        if (client != null && client.IsInitialized)
        {
            client.ClearPresence();
            client.Deinitialize();
        }
    }

    public static void UpdatePresence(string state, string details)
    {
        if (client.IsInitialized)
        {
            client.SetPresence(new RichPresence()
            {
                State = state,
                Details = details,
                Timestamps = new Timestamps()
                {
                    Start = DateTime.UtcNow 
                },
                Assets = new Assets()
                {
                    LargeImageKey = "exit",
                    LargeImageText = "Exit",
                    SmallImageKey = "battlepass",
                    SmallImageText = "Tier 100"
                },
                Buttons = new Button[]
                {
                new Button() { Label = "Join Exit Discord", Url = "https://discord.gg/exitfn" },
                new Button() { Label = "Join Support Server", Url = "https://discord.gg/Wx9pHBW3Tm" }
                }
            });
        }
    }

}