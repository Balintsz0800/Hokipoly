using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int id;
    public string username;
    public bool ready;

    public int currentCell;
    public int money;
}

public class PlayerManager : MonoBehaviour
{
    public List<Player> players = new List<Player>();

    public static PlayerManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPlayer(int id, string username)
    {
        players.Add(new Player
        {
            id = id,
            username = username
        });
    }

    public void RemovePlayer(int id)
    {
        if (TryGetPlayer(id, out Player player))
        {
            players.Remove(player);
        }
    }

    public bool TryGetPlayer(int id, out Player player)
    {
        player = players.Find(x => x.id == id);
        return player != null;
    }
}
