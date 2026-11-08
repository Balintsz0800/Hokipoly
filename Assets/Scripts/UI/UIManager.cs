using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public NetworkUIManager NetworkUI;
    
    public static UIManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}