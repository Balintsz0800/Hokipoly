public static class ServerSend
{
    public static void Id(int playerId)
    {
        NetworkManager.Instance.Server.SendPacketTo(ServerPacketId.Id, playerId, writer =>
        {
            writer.Put((ushort)playerId);
        });
    }
    
    public static void PlayerJoined(int playerId, string username)
    {
        NetworkManager.Instance.Server.SendPacketToAllExcept(ServerPacketId.PlayerLeft, playerId, writer =>
        {
            writer.Put((ushort)playerId);
            writer.Put(username);
        });
    }
    
    public static void PlayerJoined(int target, int playerId, string username)
    {
        NetworkManager.Instance.Server.SendPacketTo(ServerPacketId.PlayerJoined, target, writer =>
        {
            writer.Put((ushort)playerId);
            writer.Put(username);
        });
    }

    public static void PlayerLeft(int playerId)
    {
        NetworkManager.Instance.Server.SendPacketToAllExcept(ServerPacketId.PlayerJoined, playerId, writer =>
        {
            writer.Put((ushort)playerId);
        });
    }

    public static void PlayerReady(int playerId, bool ready)
    {
        NetworkManager.Instance.Server.SendPacketToAll(ServerPacketId.PlayerReady, writer =>
        {
            writer.Put((ushort)playerId);
            writer.Put(ready);
        });
    }

    public static void StartGame()
    {
        NetworkManager.Instance.Server.SendPacketToAll(ServerPacketId.StartGame, writer =>
        {

        });
    }

    public static void MovePlayer(int playerId, int cell)
    {
        NetworkManager.Instance.Server.SendPacketToAll(ServerPacketId.MovePlayer, writer =>
        {
            writer.Put((ushort)playerId);
            writer.Put((ushort)cell);
        });
    }

    public static void UpdateMoney(int playerId, int money)
    {
        NetworkManager.Instance.Server.SendPacketToAll(ServerPacketId.UpdateMoney, writer =>
        {
            writer.Put((ushort)playerId);
            writer.Put(money);
        });
    }
}
