using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private Movement movement;
    [SerializeField] private BoardTile[] boardTiles;
    public BoardTile[] BoardTiles => boardTiles;
    
    public void PlayerLanded(int finalCellIndex)
    {
        if (finalCellIndex < 0 || finalCellIndex >= boardTiles.Length)
        {
            return;
        }
        
        BoardTile landedTile = boardTiles[finalCellIndex];
        
        ClearAllHighlights();
        
        Debug.Log("Tile type: " + boardTiles[finalCellIndex].Type);
        
        switch (landedTile.Type)
        {
            case TileType.Buyable:
                break;
            case TileType.Purchased:
                break;
            case TileType.Finder:
                Finder finder = landedTile.GetComponent<Finder>();

                if (finder != null)
                {
                    finder.Activate(this, movement);
                }
                break;
            case TileType.Prison:
                break;
            case TileType.Start:
                break;
            case TileType.Card:
                break;
            case TileType.Bank:
                break;
            default:
                break;
        }
    }

    public void ClearAllHighlights()
    {
        foreach (var tile in boardTiles)
        {
            tile.SetHighLight(false);
        }
    }

    public void HighlightTiles()
    {
        foreach (var tile in boardTiles)
        {
            tile.SetHighLight(true);
        }
    }
}
