using TMPro;
using UnityEngine;

public class PlayerListUIElement : MonoBehaviour
{
    [SerializeField ]private TMP_Text playerName;
    [SerializeField ]private TMP_Text moneyText;
    
    public void SetPlayerName(string _playerName)
    {
        playerName.text = _playerName;
    }

    public void SetMoney(int _money)
    {
        moneyText.text = $"${_money}";
    }
}