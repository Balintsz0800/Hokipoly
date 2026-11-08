using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;

public static class ServerHandle
{
    public static void Roll(NetPeer sender, NetDataReader reader)
    {
        Debug.Log($"Player{sender.Id} rolled");
    }
}