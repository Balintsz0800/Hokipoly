public enum ClientPacketId : ushort
{
    Username,
    Roll
}

public enum ServerPacketId : ushort
{
    PlayerJoined,
    PlayerLeft,
    
    StartGame,
    
    MovePlayer,
    UpdateMoney
}