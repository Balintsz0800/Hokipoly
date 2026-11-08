using LiteNetLib.Utils;

public static class ClientHandle
{
    public static void PlayerJoined(NetDataReader reader)
    {
        int playerId = reader.GetUShort();
    }

    public static void PlayerLeft(NetDataReader reader)
    {
        int playerId = reader.GetUShort();
    }

    public static void StartGame(NetDataReader reader)
    {

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
    }
}
