using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Transform[] cells;
    
    
    public bool isMoving = false;
    private int currentCell = 0;

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

            while (Vector3.Distance(transform.position, targetcell.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetcell.position, moveSpeed * Time.deltaTime);
                
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                
                yield return null;
            }
            transform.position = targetcell.position;
            transform.rotation = targetRotation;
        }
        isMoving = false;
    }
}
