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

    public static void Ready(NetPeer sender, NetDataReader reader)
    {
        if(!PlayerManager.Instance.TryGetPlayer(sender.Id, out Player player)) return;
        
        player.ready = !player.ready;
        
        ServerSend.PlayerReady(sender.Id, player.ready);
        UIManager.Instance.LobbyUI.PlayerReady(sender.Id, player.ready);
    }
    
    public static void Roll(NetPeer sender, NetDataReader reader)
    {
        Debug.Log($"Player{sender.Id} rolled");
    }
}