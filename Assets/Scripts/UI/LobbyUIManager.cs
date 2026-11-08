using UnityEngine;

public class LobbyUIManager : MonoBehaviour
{
    public Transform lobbyPlayersHolder;
    
    public GameObject lobbyPlayerPrefab;

    private void BuildLobbyUI()
    {
        for (int i = 0; i < lobbyPlayersHolder.childCount; i++)
        {
            Destroy(lobbyPlayersHolder.GetChild(i).gameObject);
        }
        
        foreach (Player player in PlayerManager.Instance.players)
        {
            LobbyPlayerUIElement lobbyPlayerUIElement = Instantiate(lobbyPlayerPrefab, lobbyPlayersHolder).GetComponent<LobbyPlayerUIElement>();
            
            lobbyPlayerUIElement.SetPlayerNameText(player.username);
            lobbyPlayerUIElement.SetReadyText(player.ready);
        }
    }
}