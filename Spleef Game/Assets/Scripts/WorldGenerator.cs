using System.Collections.Generic;
using UnityEngine;

public enum PlatformShapes
{
    Square,
    Circle
}

public class WorldGenerator : MonoBehaviour
{
    public GameObject tileObject;

    //Radius of the platform in number of tiles
    [Min(1)]
    private int radius = 10;

    //Width of the platform in number of tiles
    [Min(1)]
    public int width = 10;

    //Length of the platform in number of tiles
    [Min(1)]
    public int length = 10;

    public float spacing = 1f;

    public Vector3 origin = new Vector3(0, 0, 0);

    private PlatformShapes platformShape = PlatformShapes.Square;

    public List<GameObject> allTiles;

    // Start is called before the first frame update
    private void Start()
    {
        allTiles = new List<GameObject>();

        GeneratePlatform();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public int CalculateMaxTileCount()
    {
        if (platformShape == PlatformShapes.Square)
        {
            return width * length;
        }
        //else if (platformShape == PlatformShapes.Circle)
        //{
        //    return 0;
        //}
        else
        {
            return 0;
        }
    }

    private void InstantiateTiles()
    {
        allTiles.Clear();
        for (int i = 0; i < CalculateMaxTileCount(); i++)
        {
            allTiles.Add(Instantiate(tileObject));
        }
    }

    private void GeneratePlatform()
    {
        int tileCount = CalculateMaxTileCount();
        if (allTiles.Count < tileCount)
        {
            InstantiateTiles();
        }
        for (int i = 0; i < tileCount; i++)
        {
            allTiles[i].SetActive(true);
            Vector3 tilePos = allTiles[i].transform.position;
            tilePos.x = origin.x + spacing * (i % width + 0.5f) - (width / 2);
            tilePos.z = origin.z + spacing * (i / length + 0.5f) - (length / 2);
            tilePos.y = origin.y;
            allTiles[i].transform.position = tilePos;
        }
    }
}