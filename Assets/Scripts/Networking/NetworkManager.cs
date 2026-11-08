using System;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public Client Client;
    public Server Server;

    public bool isHost;
    
    public static NetworkManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        Client?.Update();
        Server?.Update();
    }

    public void Connect(string ip)
    {
        Client = new Client(ip, 6700);
    }

    public void Host()
    {
        Server = new Server(6700);
        isHost = true;
    }

    public void Disconnect()
    {
        Client?.Disconnect();
        Server?.Stop();
        
        Clear();
    }

    private void Clear()
    {
        Client = null;
        Server = null;

        isHost = false;
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }
}