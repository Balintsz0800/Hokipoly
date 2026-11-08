public enum ClientPacketId : ushort
{
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