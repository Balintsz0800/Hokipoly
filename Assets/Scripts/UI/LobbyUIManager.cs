using System.Collections.Generic;
using UnityEngine;

public class LobbyUIManager : MonoBehaviour
{
    public List<LobbyPlayerUIElement> lobbyPlayerUIElements = new List<LobbyPlayerUIElement>();

    public Transform lobbyPlayersHolder;
    public GameObject lobbyPlayerPrefab;

    public GameObject readyButton;
    public GameObject unreadyButton;
    public GameObject startButton;

    public bool ready;

    private void BuildLobbyUI()
    {
        lobbyPlayerUIElements.Clear();
        for (int i = 0; i < lobbyPlayersHolder.childCount; i++)
        {
            Destroy(lobbyPlayersHolder.GetChild(i).gameObject);
        }

        foreach (Player player in PlayerManager.Instance.players)
        {
            LobbyPlayerUIElement lobbyPlayerUIElement = Instantiate(lobbyPlayerPrefab, lobbyPlayersHolder).GetComponent<LobbyPlayerUIElement>();

            lobbyPlayerUIElement.id = player.id;
            
            lobbyPlayerUIElement.SetPlayerNameText(player.username);
            lobbyPlayerUIElement.SetReadyText(player.ready);
            
            lobbyPlayerUIElements.Add(lobbyPlayerUIElement);
        }
    }

    public void RefreshLobbyUI()
    {
        BuildLobbyUI();
    }

    public void PlayerReady(int playerId, bool ready)
    {
        LobbyPlayerUIElement element = lobbyPlayerUIElements.Find(x => x.id == playerId);
        if (element != null) element.SetReadyText(ready);
        
        if (playerId == NetworkManager.Instance.myId)
            this.ready = ready;

        if (NetworkManager.Instance.isHost)
        {
            startButton.gameObject.SetActive(EveryoneReady());
        }
    }

    public void ToggleReady()
    {
        if (NetworkManager.Instance.isHost)
        {
            if (PlayerManager.Instance.TryGetPlayer(0, out Player player))
            {
                player.ready = !player.ready;
                PlayerReady(0, player.ready);
                ServerSend.PlayerReady(0, player.ready);
            }
        }
        else
        {
            ClientSend.Ready();
            ready = !ready;
        }
        
        if (ready)
        {
            readyButton.SetActive(false);
            unreadyButton.SetActive(true);
        }
        else
        {
            readyButton.SetActive(true);
            unreadyButton.SetActive(false);
        }
        
        
    }

    public bool EveryoneReady()
    {
        int playerCount = PlayerManager.Instance.players.Count;
        int readyPlayerCount = 0;
        
        foreach (Player player in PlayerManager.Instance.players)
        {
            if(player.ready) readyPlayerCount++;
        }

        if (readyPlayerCount == playerCount) return true;

        return false;
    }

    public void StartGame()
    {
        ServerSend.StartGame();
        UIManager.Instance.SwitchUIState(UIState.Game);

        foreach (Player player in PlayerManager.Instance.players)
        {
            ServerSend.UpdateMoney(player.id, 1000000);
            UIManager.Instance.GameUI.UpdateMoney(player.id, 1000000);
        }
    }
}
