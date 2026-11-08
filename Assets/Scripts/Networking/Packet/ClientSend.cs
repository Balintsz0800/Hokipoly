public static class ClientSend
{
    public static void SendUsername(string username)
    {
        NetworkManager.Instance.Client.SendPacket(ClientPacketId.Username, writer =>
        {
            writer.Put(username);
        });
    }

    public static void Ready()
    {
        NetworkManager.Instance.Client.SendPacket(ClientPacketId.Ready);
    }
    
    public static void Roll()
    {
        NetworkManager.Instance.Client.SendPacket(ClientPacketId.Roll, writer =>
        {
            
        });
    }
}