using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameObject currentDice;
    [SerializeField] private Dice dicePrefab;
    [SerializeField] private Transform diceSpawn;

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
        Dice dice = Instantiate(dicePrefab, diceSpawn.position, Quaternion.identity);
        currentDice = dice.gameObject;
        
        Invoke(nameof(DestroyDice), 5);
        
        dice.RollDice();
    }

    public void DestroyDice()
    {
        Destroy(currentDice);
    }
}