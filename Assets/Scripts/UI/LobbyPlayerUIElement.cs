using TMPro;
using UnityEngine;

public class LobbyPlayerUIElement : MonoBehaviour
{
    public TMP_Text readyText;
    public TMP_Text playerNameText;

    public void SetReadyText(bool ready)
    {
        if(ready)
        {
            readyText.color = Color.green;
            readyText.text = "READY";
        }
        else
        {
            readyText.color = Color.red;
            readyText.text = "NOT READY";
        }
    }

    public void SetPlayerNameText(string playerName)
    {
        playerNameText.text = playerName;
    }
}