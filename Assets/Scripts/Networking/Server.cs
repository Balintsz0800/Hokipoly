using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;

public class Server
{
    private NetManager server;
    private EventBasedNetListener listener;

    public Server(int port)
    {
        listener = new EventBasedNetListener();
        server = new NetManager(listener);
        
        Debug.Log("Starting server on port " + port);
        server.Start(port);
        
        AssignEvents();
        RegisterPackets();
    }

    public void Stop()
    {
        server.Stop();
    }

    public void Update()
    {
        server.PollEvents();
    }

    private void AssignEvents()
    {
        listener.NetworkReceiveEvent += OnNetworkReceive;
    }

    private void RegisterPackets()
    {
        PacketRouter.RegisterServerPacket((ushort)ClientPacketId.Test, TestPacket);
    }

    private static void TestPacket(NetPeer peer, NetDataReader reader)
    {
        string message = reader.GetString();
        Debug.Log(message);
    }
    
    private static void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
    {
        PacketRouter.HandleServerPacket(peer, reader);
    }
}