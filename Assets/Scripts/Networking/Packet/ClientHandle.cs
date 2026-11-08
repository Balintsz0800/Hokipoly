using LiteNetLib.Utils;

public static class ClientHandle
{
    public static void Id(NetDataReader reader)
    {
        int playerId = reader.GetUShort();
        
        NetworkManager.Instance.myId = playerId;
    }
    
    public static void PlayerJoined(NetDataReader reader)
    {
        int playerId = reader.GetUShort();
        string username = reader.GetString();
        
        NetworkManager.Instance.OnPlayerJoined(playerId, username);
    }

    public static void PlayerLeft(NetDataReader reader)
    {
        int playerId = reader.GetUShort();
        
        NetworkManager.Instance.OnPlayerLeft(playerId);
    }

    public static void PlayerReady(NetDataReader reader)
    {
        int playerId = reader.GetUShort();
        bool ready = reader.GetBool();
        
        UIManager.Instance.LobbyUI.PlayerReady(playerId, ready);
    }
    
    public static void StartGame(NetDataReader reader)
    {
        UIManager.Instance.SwitchUIState(UIState.Game);
    }

    public static void MovePlayer(NetDataReader reader)
    {
        int playerId = reader.GetUShort();
        int cell = reader.GetUShort();
    }

    public static void UpdateMoney(NetDataReader reader)
    {
        int playerId = reader.GetUShort();
        int money = reader.GetInt();

        if (PlayerManager.Instance.TryGetPlayer(playerId, out Player player))
        {
            player.money = money;
        }
        UIManager.Instance.GameUI.UpdateMoney(playerId, money);
    }
}
