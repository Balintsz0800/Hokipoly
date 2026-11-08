public static class ClientSend
{
    public static void Roll()
    {
        NetworkManager.Instance.Client.SendPacket(ClientPacketId.Roll, writer =>
        {
            
        });
    }
}