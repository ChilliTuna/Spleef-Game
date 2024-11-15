using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public enum MeteorTargetingModes
    {
        Scan,
        Random
    }

    public GameObject meteorBaseObject;

    public bool shouldBeSpawning = false;

    private bool isSpawning = false;

    public float spawnTime = 3f;

    public MeteorTargetingModes targetingMode = MeteorTargetingModes.Random;

    public Vector3 spawnPosition;

    private PlatformBase platform;

    private List<GameObject> instantiatedMeteors;

    private List<GameObject> freeMeteors;

    private List<GameObject> meteorTargetOrder;

    private bool isTargetOrderEstablished = false;

    // Start is called before the first frame update
    private void Start()
    {
        platform = GetComponent<PlatformBase>();

        if (meteorBaseObject != null)
        {
            shouldBeSpawning = true;
            instantiatedMeteors = new List<GameObject>();
            freeMeteors = new List<GameObject>();
            InstantiateMeteor();
        }
        if (spawnPosition == null)
        {
            spawnPosition = transform.position;
            spawnPosition.y += 10f;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (shouldBeSpawning)
        {
            if(!isTargetOrderEstablished)
            {
                GetMeteorTargetOrder();
            }
            if (isSpawning == false)
            {
                StartCoroutine(DoSpawning());
                isSpawning = true;
            }
        }
        else
        {
            if (isSpawning == true)
            {
                StopCoroutine(DoSpawning());
                isSpawning = false;
            }
        }
    }

    private IEnumerator DoSpawning()
    {
        while (true)
        {
            SpawnMeteor();
            yield return new WaitForSeconds(spawnTime);
        }
    }

    private void SpawnMeteor()
    {
        if (platform.activeTiles.Count > 0)
        {
            if (freeMeteors.Count == 0)
            {
                InstantiateMeteor();
            }
            GameObject spawnedMeteor = freeMeteors[0];
            OccupyMeteor(spawnedMeteor);
            spawnedMeteor.GetComponent<Meteor>().Spawn(GetTargetTile(), spawnPosition);
        }
    }

    private void GetMeteorTargetOrder()
    {
        isTargetOrderEstablished = true;
        if (targetingMode == MeteorTargetingModes.Scan)
        {
            //Scan type target order
            meteorTargetOrder = platform.activeTiles;
        }
        else if(targetingMode == MeteorTargetingModes.Random)
        {
            platform.activeTiles.Shuffle();
            meteorTargetOrder = platform.activeTiles;
            //Introduce random target order. (Shuffle active tiles somehow)
        }
        // If no targeting mode, don't sort
    }

    private GameObject GetTargetTile()
    {
        GameObject targetTile = meteorTargetOrder[0];
        meteorTargetOrder.RemoveAt(0);
        return targetTile;
    }

    private void OccupyMeteor(GameObject meteor)
    {
        freeMeteors.Remove(meteor);
    }

    private void FreeUpMeteor(GameObject meteor)
    {
        freeMeteors.Add(meteor);
    }

    private void InstantiateMeteor()
    {
        GameObject newMeteor = Instantiate(meteorBaseObject);
        newMeteor.GetComponent<Meteor>().OnCollision += FreeUpMeteor;
        instantiatedMeteors.Add(newMeteor);
        FreeUpMeteor(newMeteor);
    }
}