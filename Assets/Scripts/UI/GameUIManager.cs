using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject rollButton;
    [SerializeField] private Transform playerHolder;
    [SerializeField] private TMP_Text idk;

    private void BuildPlayerList()
    {
        List<Player> players = PlayerManager.Instance.players;

        for (int i = 0; i < playerHolder.childCount; i++)
        {
            playerHolder.GetChild(i).gameObject.SetActive(false);
        }

        foreach (Player player in players)
        {
            if (player.id < 0 || player.id >= playerHolder.childCount) continue;
            PlayerListUIElement element = playerHolder.GetChild(player.id).GetComponent<PlayerListUIElement>();

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
        if (playerId >= 0 && playerId < playerHolder.childCount)
            playerHolder.GetChild(playerId).GetComponent<PlayerListUIElement>()?.SetMoney(money);
    }

    public void Rolandice()
    {
        GameManager.Instance.RollDice();
    }

    public void Bruhhh(string a)
    {
        idk.text = a;
    }

    public void SetRollButton(bool a)
    {
        rollButton.gameObject.SetActive(a);
    }
}
