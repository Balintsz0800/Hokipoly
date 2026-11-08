using System.Collections;
using UnityEngine;

public class movementtest : MonoBehaviour
{
    [SerializeField] private Movement movement;
    [SerializeField] private Dice dice;
    
    private bool isPlaying = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isPlaying)
        {
            dice.RollDice();
            StartCoroutine(RollMove());
        }
    }

    private IEnumerator RollMove()
    {
        isPlaying = true;
        
        dice.RollDice();

        while (dice.IsRolling())
        {
            yield return null;
        }
        
        movement.MoveCells(dice.currentNumber);

        isPlaying = false;
    }
}
