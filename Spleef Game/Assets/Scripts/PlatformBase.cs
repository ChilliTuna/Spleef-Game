using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBase : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> allTiles;

    [HideInInspector]
    public List<GameObject> activeTiles;

    private void Start()
    {
        if (allTiles.Count == 0)
        {
            allTiles = new List<GameObject>();
            foreach(Transform child in transform)
            {
                allTiles.Add(child.gameObject);
            }
        }
        foreach (GameObject tile in allTiles)
        {
            if (tile.activeInHierarchy == true)
            {
                activeTiles.Add(tile);
            }
        }
    }

    public void KillTile(GameObject tile)
    {
        activeTiles.Remove(tile);
        tile.SetActive(false);
    }

    public void KillPlatform()
    {
        foreach (GameObject tile in allTiles)
        {
            KillTile(tile);
        }
    }

    public void WakeTile(GameObject tile)
    {
        activeTiles.Add(tile);
        tile.SetActive(true);
    }

    public void WakePlatform()
    {
        foreach (GameObject tile in allTiles)
        {
            WakeTile(tile);
        }
    }
}
