using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorObject;

    [HideInInspector]
    public bool shouldBeSpawning = false;

    public float spawnTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        if (meteorObject != null)
        {
            shouldBeSpawning = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
