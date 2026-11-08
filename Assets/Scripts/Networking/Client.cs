using System;
using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;

public class Client
{
    private NetManager client;
    private EventBasedNetListener listener;

    public Action OnConnectedEvent;

    private NetPeer serverPeer;

    public Client(string ip, int port)
    {
        listener = new EventBasedNetListener();
        client = new NetManager(listener);

        Debug.Log($"Connecting -> {ip}:{port}...");

        AssignEvents();
        RegisterPackets();

        client.Start();
        client.Connect(ip, port, "a");
    }

    public void Disconnect()
    {
        client.Stop();
    }

    public void Update()
    {
        client.PollEvents();
    }

    private void AssignEvents()
    {
        listener.PeerConnectedEvent += OnPeerConnected;
        listener.NetworkReceiveEvent += OnNetworkReceive;
    }

    private void OnPeerConnected(NetPeer peer)
    {
        Debug.Log($"Connected to {peer.Address}");

        serverPeer = peer;

        OnConnectedEvent?.Invoke();
    }

    private static void RegisterPackets()
    {
        PacketRouter.RegisterClientPacket((ushort)ServerPacketId.Id, ClientHandle.Id);
        PacketRouter.RegisterClientPacket((ushort)ServerPacketId.PlayerJoined, ClientHandle.PlayerJoined);
        PacketRouter.RegisterClientPacket((ushort)ServerPacketId.PlayerLeft, ClientHandle.PlayerLeft);
        PacketRouter.RegisterClientPacket((ushort)ServerPacketId.PlayerReady, ClientHandle.PlayerReady);
        PacketRouter.RegisterClientPacket((ushort)ServerPacketId.StartGame, ClientHandle.StartGame);
        PacketRouter.RegisterClientPacket((ushort)ServerPacketId.UpdateMoney, ClientHandle.UpdateMoney);
        PacketRouter.RegisterClientPacket((ushort)ServerPacketId.MovePlayer, ClientHandle.MovePlayer);
    }

    private static void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
    {
        PacketRouter.HandleClientPacket(reader);
    }

    public void SendPacket(ClientPacketId packetId, Action<NetDataWriter> writeData = null, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered)
    {
        NetDataWriter writer = new NetDataWriter();

        writer.Put((ushort)packetId);
        writeData?.Invoke(writer);

        serverPeer?.Send(writer, deliveryMethod);
    }
}
