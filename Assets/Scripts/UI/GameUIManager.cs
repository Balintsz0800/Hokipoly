using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private Transform playerHolder;

    private void BuildPlayerList()
    {
        List<Player> players = PlayerManager.Instance.players;

        for (int i = 0; i < playerHolder.childCount; i++)
        {
            playerHolder.GetChild(i).gameObject.SetActive(false);
        }

        foreach (Player player in players)
        {
            PlayerListUIElement element = playerHolder.GetChild(player.id).gameObject.GetComponent<PlayerListUIElement>();

            element.gameObject.SetActive(true);
            element.SetPlayerName(player.username);
        }
    }

    public void RefreshGameUI()
    {
        BuildPlayerList();
    }

    public void UpdateMoney(int playerId, int money)
    {
        PlayerListUIElement element = playerHolder.GetChild(playerId).gameObject.GetComponent<PlayerListUIElement>();
        
        element.SetMoney(money);
    }

    public void Rolandice()
    {
        GameManager.Instance.RollDice();
    }
}