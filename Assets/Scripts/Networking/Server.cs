using System;
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
        listener.PeerDisconnectedEvent += OnPeerDisconnected;
    }

    private static void RegisterPackets()
    {
        PacketRouter.RegisterServerPacket((ushort)ClientPacketId.Username, ServerHandle.Username);
        PacketRouter.RegisterServerPacket((ushort)ClientPacketId.Roll, ServerHandle.Roll);
    }

    private void OnConnectionRequest(ConnectionRequest request)
    {
        if (server.ConnectedPeersCount < 6)
            request.AcceptIfKey("a");
        else
            request.Reject();
    }

    private void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        NetworkManager.Instance.OnPlayerLeft(peer.Id);
    }

    private static void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
    {
        PacketRouter.HandleServerPacket(peer, reader);
    }

    public void SendPacketToAll(ServerPacketId packetId, Action<NetDataWriter> writeData = null, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered)
    {
        NetDataWriter writer = new NetDataWriter();

        writer.Put((ushort)packetId);
        writeData?.Invoke(writer);

        server.SendToAll(writer, deliveryMethod);
    }

    public void SendPacketToAllExcept(ServerPacketId packetId, int peerIdExcept, Action<NetDataWriter> writeData, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered)
    {
        NetDataWriter writer = new NetDataWriter();

        writer.Put((ushort)packetId);
        writeData?.Invoke(writer);

        NetPeer excludedPeer = null;

        foreach (NetPeer peer in server)
        {
            if (peer.Id == peerIdExcept)
            {
                excludedPeer = peer;
                break;
            }
        }

        server.SendToAll(writer, deliveryMethod, excludedPeer);
    }

    public void SendPacketTo(ServerPacketId packetId, int peerId, Action<NetDataWriter> writeData = null, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered)
    {
        NetDataWriter writer = new NetDataWriter();

        writer.Put((ushort)packetId);
        writeData?.Invoke(writer);

        foreach (NetPeer peer in server)
        {
            if (peer.Id == peerId)
            {
                peer.Send(writer, deliveryMethod);
                break;
            }
        }
    }
}
