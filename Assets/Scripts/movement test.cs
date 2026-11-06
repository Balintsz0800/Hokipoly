using System;
using UnityEngine;

public class movementtest : MonoBehaviour
{
    [SerializeField] private Movement movement;
    [SerializeField] private int step;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            movement.MoveCells(step);
        }
    }
}
