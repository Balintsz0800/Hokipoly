using System;
using UnityEngine;

public enum UIState
{
    MainMenu,
    Rules,
    Connecting,
    Lobby,
    Game
}

public class UIManager : MonoBehaviour
{
    public GameObject BG;
    
    public MainMenuManager MainMenu;
    public NetworkUIManager NetworkUI;
    public LobbyUIManager LobbyUI;
    public GameObject RulesUI;
    public GameUIManager GameUI;
    
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
        RulesUI.gameObject.SetActive(state == UIState.Rules);
        NetworkUI.gameObject.SetActive(state == UIState.Connecting);
        LobbyUI.gameObject.SetActive(state == UIState.Lobby);
        GameUI.gameObject.SetActive(state == UIState.Game);
        
        BG.SetActive(state != UIState.Game);

        if (state == UIState.Game)
        {
            GameUI.RefreshGameUI();
        }

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

    public void SwitchToRulesUI()
    {
        SwitchUIState(UIState.Rules);
    }
}