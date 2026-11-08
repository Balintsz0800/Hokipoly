using System;
using UnityEngine;

public enum UIState
{
    MainMenu,
    Connecting,
    Lobby
}

public class UIManager : MonoBehaviour
{
    public MainMenuManager MainMenu;
    public NetworkUIManager NetworkUI;
    public LobbyUIManager LobbyUI;
    
    public UIState currentState = UIState.MainMenu;
    
    public static UIManager Instance;

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

    public void SwitchUIState(UIState state)
    {
        if (currentState == state)
            return;

        MainMenu.gameObject.SetActive(state == UIState.MainMenu);
        NetworkUI.gameObject.SetActive(state == UIState.Connecting);
        LobbyUI.gameObject.SetActive(state == UIState.Lobby);

        currentState = state;
    }




    public void SwitchToMainMenu()
    {
        SwitchUIState(UIState.MainMenu);
    }
    
    public void SwitchToNetworkUI()
    {
        SwitchUIState(UIState.Connecting);
    }
}