using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;

public static class ServerHandle
{
    public static void Username(NetPeer sender, NetDataReader reader)
    {
        string username = reader.GetString();

        NetworkManager.Instance.OnPlayerJoined(sender.Id, username);
    }
    
    public static void Roll(NetPeer sender, NetDataReader reader)
    {
        Debug.Log($"Player{sender.Id} rolled");
    }
}