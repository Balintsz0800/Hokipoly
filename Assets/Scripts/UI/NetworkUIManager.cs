using TMPro;
using UnityEngine;

public class NetworkUIManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField ipField;
    
    public string GetIP() => ipField.text;
}