using System;
using UnityEngine;

public class Finder : MonoBehaviour
{
    private BoardManager boardManager;
    private Movement movement;
    private Camera camera;
    
    private BoardTile selectedTile;
    private bool isActive;
    
    public void Activate(BoardManager manager, Movement playerMovement)
    {
        boardManager = manager;
        movement = playerMovement;
        camera = Camera.main;
        isActive = true;
    }

    private void Update()
    {
        if (!isActive || movement.isMoving)
        {
            return;
        }
        
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            BoardTile tile = hit.collider.GetComponent<BoardTile>();

            if (tile != null && tile != boardManager.BoardTiles[movement.currentCell])
            {
                if (selectedTile != tile)
                {
                    ClearSelection();
                    selectedTile = tile;
                    selectedTile.SetHighLight(true);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    int targetIndex = System.Array.IndexOf(boardManager.BoardTiles, selectedTile);

                    if (targetIndex >= 0)
                    {
                        isActive = false;
                        ClearSelection();
                        movement.MoveToCell(targetIndex);
                    }
                }
                return;
            }
        }
        ClearSelection();
    }

    private void ClearSelection()
    {
        if (selectedTile != null)
        {
            selectedTile.SetHighLight(false);
            selectedTile = null;
        }
    }
}
