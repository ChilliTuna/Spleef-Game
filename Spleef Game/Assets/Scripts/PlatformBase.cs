using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBase : MonoBehaviour
{
    public MeteorSpawner meteorSpawner;

    [HideInInspector]
    public List<GameObject> allTiles;

    private void Start()
    {
        meteorSpawner = gameObject.GetComponent<MeteorSpawner>();
        if (allTiles == null)
        {
            allTiles = new List<GameObject>();
            foreach(Transform child in transform)
            {
                allTiles.Add(child.gameObject);
            }
        }

    }
}
