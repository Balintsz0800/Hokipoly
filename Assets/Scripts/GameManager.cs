using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameObject currentDice;
    [SerializeField] private Dice dicePrefab;
    [SerializeField] private Transform diceSpawn;

    public Player playerTurn;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    public void RollDice()
    {
        if (currentDice != null)
        {
            Destroy(currentDice);
        }

        Dice dice = Instantiate(dicePrefab, diceSpawn.position, Quaternion.identity);
        currentDice = dice.gameObject;

        CancelInvoke(nameof(DestroyDice));
        Invoke(nameof(DestroyDice), 5);

        dice.RollDice();
    }

    public void DestroyDice()
    {
        Destroy(currentDice);
    }

    public void PlayersTurn(int playerId)
    {
        if (PlayerManager.Instance.TryGetPlayer(playerId, out Player player))
        {
            UIManager.Instance.GameUI.Bruhhh($"{player.username}'s turn");

            UIManager.Instance.GameUI.SetRollButton(NetworkManager.Instance.myId == playerId);
        }
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

        ServerSend.PlayerTurn(0);
        PlayersTurn(0);

        if (PlayerManager.Instance.TryGetPlayer(0, out Player a))
        {
            playerTurn = a;
        }
    }
}
