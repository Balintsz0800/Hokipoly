using System;
using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;

public class Client
{
    private NetManager client;
    private EventBasedNetListener listener;

    private NetPeer serverPeer;

    public Client(string ip, int port)
    {
        listener = new EventBasedNetListener();
        client = new NetManager(listener);
     
        Debug.Log($"Connecting -> {ip}:{port}...");
     
        client.Start();
        client.Connect(ip, port, "a");
        
        AssignEvents();
        RegisterPackets();
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
    }

    private static void RegisterPackets()
    {
        
    }
    
    private static void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
    {
        PacketRouter.HandleClientPacket(peer, reader);
    }

    public void SendPacket(ClientPacketId packetId, Action<NetDataWriter> writeData = null, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered)
    {
        NetDataWriter writer = new NetDataWriter();
        
        writer.Put((ushort)packetId);
        writeData?.Invoke(writer);

        serverPeer?.Send(writer, deliveryMethod);
    }
}