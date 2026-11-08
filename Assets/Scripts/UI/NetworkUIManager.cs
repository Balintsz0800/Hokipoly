using TMPro;
using UnityEngine;

public class NetworkUIManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField ipField;

    public void Connect()
    {
        NetworkManager.Instance.Connect(ipField.text);
    }
    
    public void Host()
    {
        NetworkManager.Instance.Host();
    }
}