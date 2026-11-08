using System;
using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Transform[] cells;

    [SerializeField] private BoardManager boardManager;
    
    public bool isMoving = false;
    public int currentCell = 0;

    public void MoveCells(int amount)
    {
        if (!isMoving)
        {
            StartCoroutine(MoveCellsRoutine(amount));
        }
    }

    public IEnumerator MoveCellsRoutine(int amount)
    {
        isMoving = true;

        for (int i = 0; i < amount; i++)
        {
            currentCell++;

            if (currentCell >= cells.Length)
            {
                currentCell = 0;
            }
            
            Transform targetcell = cells[currentCell];
            Quaternion targetRotation = targetcell.rotation;
            
            Vector3 targetPos = new Vector3(targetcell.position.x, transform.position.y, targetcell.position.z);

            while (Vector3.Distance(transform.position, targetPos) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                
                yield return null;
            }
            transform.position = targetPos;
            transform.rotation = targetRotation;
        }
        isMoving = false;
        Debug.Log("Current Cell: " + currentCell);
        boardManager.PlayerLanded(currentCell);
    }
}
