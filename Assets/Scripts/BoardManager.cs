using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private BoardTile[] boardTiles;
    
    public void PlayerLanded(int finalCellIndex)
    {
        BoardTile landedTile = boardTiles[finalCellIndex];
        
        ClearAllHighlights();
        
        switch (landedTile.Type)
        {
            case TileType.Buyable:
                break;
            case TileType.Purchased:
                break;
            case TileType.Finder:
                HighlightTiles();
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
