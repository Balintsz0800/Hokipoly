using System;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
    [Header("Tile Settings")] 
    [SerializeField] private TileType tileType;
    [SerializeField] private GameObject highLight;
    
    public TileType Type => tileType;

    private void Start()
    {
        SetHighLight(false);
    }

    public void SetHighLight(bool active)
    {
        if (highLight != null)
        {
            highLight.SetActive(active);
        }
    }
}

public enum TileType
{
    Buyable,
    Purchased,
    Prison,
    Finder,
    Start,
    Card,
    Bank
}
