using System;
using System.Collections.Generic;
using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;

public static class PacketRouter
{
    private static readonly Dictionary<ushort, ServerMessageCallback> ServerMessageCallbacks = new Dictionary<ushort, ServerMessageCallback>();
    private static readonly Dictionary<ushort, ClientMessageCallback> ClientMessageCallbacks = new Dictionary<ushort, ClientMessageCallback>();
    
    public delegate void ServerMessageCallback(NetPeer sender, NetDataReader reader);
    public delegate void ClientMessageCallback(NetPeer sender, NetDataReader reader);

    private const int HeaderSize = sizeof(ushort);
    
    public static void RegisterServerPacket(ushort packetId, ServerMessageCallback callback)
    {
        if(callback == null) return;
        
        ServerMessageCallbacks[packetId] = callback;
    }

    public static void RegisterClientPacket(ushort packetId, ClientMessageCallback callback)
    {
        if(callback == null) return;
        
        ClientMessageCallbacks[packetId] = callback;
    }

    public static void HandleServerPacket(NetPeer peer, NetPacketReader reader)
    {
        try
        {
            if (!IsPacketValid(reader)) return;

            ushort packetId = reader.GetUShort();

            if (!TryGetServerHandler(packetId, out ServerMessageCallback callback)) return;

            callback(peer, reader);
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }

    public static bool TryGetServerHandler(ushort packetId, out ServerMessageCallback callback)
    {
        return ServerMessageCallbacks.TryGetValue(packetId, out callback);
    }

    public static void HandleClientPacket(NetPeer peer, NetPacketReader reader)
    {
        try
        {
            if (!IsPacketValid(reader)) return;

            ushort packetId = reader.GetUShort();

            if (!TryGetServerHandler(packetId, out ServerMessageCallback callback)) return;

            callback(peer, reader);
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }

    public static bool TryGetClientHandler(ushort packetId, out ClientMessageCallback callback)
    {
        return ClientMessageCallbacks.TryGetValue(packetId, out callback);
    }

    public static bool IsPacketValid(NetPacketReader reader)
    {
        return reader.AvailableBytes >= HeaderSize;
    }
}