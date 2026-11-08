using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkManager : MonoBehaviour
{
    public Client Client;
    public Server Server;

    public bool isHost;
    
    public static NetworkManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Client?.Update();
        Server?.Update();
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ClientSend.Roll();
        } 
    }

    public void Connect(string ip)
    {
        Client = new Client(ip, 6700)
        {
            OnConnectedEvent = OnClientConnected
        };
    }

    public void Host()
    {
        Server = new Server(6700);
        isHost = true;
        
        OnServerStarted();
    }

    public void Disconnect()
    {
        Client?.Disconnect();
        Server?.Stop();
        
        Clear();
    }


    private void OnClientConnected()
    {
        UIManager.Instance.SwitchUIState(UIState.Lobby);
        ClientSend.SendUsername("Client");
    }
    
    private void OnServerStarted()
    {
        UIManager.Instance.SwitchUIState(UIState.Lobby);
        OnPlayerJoined(0, "Host");
    }
    

    public void OnPlayerJoined(int playerId, string username)
    {
        Debug.Log($"{username} joined the game ({playerId})");
        
        PlayerManager.Instance.AddPlayer(playerId, username);
        
        if (isHost)
        {
            ServerSend.PlayerJoined(playerId, username);

            foreach (Player player in PlayerManager.Instance.players)
            {
                ServerSend.PlayerJoined(playerId, player.id, player.username);
            }
        }
    }

    public void OnPlayerLeft(int playerId)
    {
        PlayerManager.Instance.RemovePlayer(playerId);
        Debug.Log($"Player ({playerId}) disconnected");
    }
    

    private void Clear()
    {
        Client = null;
        Server = null;

        isHost = false;
        
        UIManager.Instance.SwitchUIState(UIState.MainMenu);
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }
}