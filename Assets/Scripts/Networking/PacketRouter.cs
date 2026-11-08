using System;
using System.Collections.Generic;
using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;

public static class PacketRouter
{
    private static readonly Dictionary<ushort, ServerPacketHandler> ServerMessageCallbacks = new Dictionary<ushort, ServerPacketHandler>();
    private static readonly Dictionary<ushort, ClientPacketHandler> ClientMessageCallbacks = new Dictionary<ushort, ClientPacketHandler>();
    
    public delegate void ServerPacketHandler(NetPeer sender, NetDataReader reader);
    public delegate void ClientPacketHandler(NetDataReader reader);

    private const int HeaderSize = sizeof(ushort);
    
    public static void RegisterServerPacket(ushort packetId, ServerPacketHandler callback)
    {
        if(callback == null) return;
        
        ServerMessageCallbacks[packetId] = callback;
    }

    public static void RegisterClientPacket(ushort packetId, ClientPacketHandler callback)
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

            if (!TryGetServerHandler(packetId, out ServerPacketHandler callback)) return;

            callback(peer, reader);
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }

    public static bool TryGetServerHandler(ushort packetId, out ServerPacketHandler callback)
    {
        return ServerMessageCallbacks.TryGetValue(packetId, out callback);
    }

    public static void HandleClientPacket(NetPacketReader reader)
    {
        try
        {
            if (!IsPacketValid(reader)) return;

            ushort packetId = reader.GetUShort();

            if (!TryGetClientHandler(packetId, out ClientPacketHandler callback)) return;

            callback(reader);
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }

    public static bool TryGetClientHandler(ushort packetId, out ClientPacketHandler callback)
    {
        return ClientMessageCallbacks.TryGetValue(packetId, out callback);
    }

    public static bool IsPacketValid(NetPacketReader reader)
    {
        return reader.AvailableBytes >= HeaderSize;
    }
}