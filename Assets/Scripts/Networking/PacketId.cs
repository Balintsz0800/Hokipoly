public enum ClientPacketId : ushort
{
    Ready,
    Username,
    Roll
}

public enum ServerPacketId : ushort
{
    Id,
    
    PlayerJoined,
    PlayerLeft,
    
    PlayerReady,
    StartGame,
    
    MovePlayer,
    UpdateMoney
}