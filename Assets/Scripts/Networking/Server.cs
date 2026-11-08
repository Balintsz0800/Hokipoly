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

        AssignEvents();
        RegisterPackets();
        
        Debug.Log("Starting server on port " + port);
        server.Start(port);
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
        listener.ConnectionRequestEvent += OnConnectionRequest;
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

    private void OnConnectionRequest(ConnectionRequest request)
    {
        Debug.Log($"Connection request received from {request.RemoteEndPoint}");
        
        if (server.ConnectedPeersCount < 6)
            request.AcceptIfKey("a");
        else
            request.Reject();
    }

    private static void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
    {
        PacketRouter.HandleServerPacket(peer, reader);
    }
}
