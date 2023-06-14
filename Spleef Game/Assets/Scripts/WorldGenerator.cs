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
    public int radius = 10;

    //Width of the platform in number of tiles
    [Min(1)]
    public int width = 10;

    //Length of the platform in number of tiles
    [Min(1)]
    public int length = 10;

    public float spacing = 1f;

    public Vector3 origin = new Vector3(0, -0.25f, 0);

    [SerializeField]
    private PlatformShapes platformShape = PlatformShapes.Square;

    public List<GameObject> allTiles;

    public GameObject parentGameObject;

    // Start is called before the first frame update
    private void Start()
    {
        allTiles = new List<GameObject>();
        parentGameObject = new GameObject();
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
        else if (platformShape == PlatformShapes.Circle)
        {
            return 4 * radius * radius;
        }
        else
        {
            return 0;
        }
    }

    private void InstantiateTiles()
    {
        int initialTileCount = allTiles.Count;
        for (int i = 0; i < CalculateMaxTileCount() - initialTileCount; i++)
        {
            allTiles.Add(Instantiate(tileObject));
            allTiles[i].transform.parent = parentGameObject.transform;
        }
    }

    private void GeneratePlatform()
    {
        int tileCount = CalculateMaxTileCount();
        if (allTiles.Count < tileCount)
        {
            InstantiateTiles();
        }

        if (platformShape == PlatformShapes.Circle)
        {
            for (int i = 0; i < tileCount; i++)
            {
                int diameter = radius * 2;
                Vector3 tilePos = allTiles[i].transform.position;
                tilePos.x = origin.x + spacing * (i % diameter + spacing / 2) - radius;
                tilePos.z = origin.z + spacing * (i / diameter + spacing / 2) - radius;
                tilePos.y = origin.y;
                allTiles[i].transform.position = tilePos;
                if ((tilePos - origin).magnitude <= radius)
                {
                    allTiles[i].SetActive(true);
                    allTiles[i].name = "Tile " + ((i % diameter + 0.5f) - radius) + ", " + ((i / diameter + 0.5f) - radius);
                }
                else
                {
                    allTiles[i].SetActive(false);
                }
            }
        }
        else if (platformShape == PlatformShapes.Square)
        {
            for (int i = 0; i < tileCount; i++)
            {
                Vector3 tilePos = allTiles[i].transform.position;
                tilePos.x = origin.x + spacing * (i % width + 0.5f) - (width / 2);
                tilePos.z = origin.z + spacing * (i / length + 0.5f) - (length / 2);
                tilePos.y = origin.y;
                allTiles[i].transform.position = tilePos;
                allTiles[i].SetActive(true);
                allTiles[i].name = "Tile " + ((i % width + 0.5f) - (width / 2)) + ", " + ((i / length + 0.5f) - (length / 2));
            }
        }
    }
}