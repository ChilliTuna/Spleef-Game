using System.Collections.Generic;
using UnityEngine;

public enum PlatformShapes
{
    Square,
    Circle
}

public class WorldGenerator : MonoBehaviour
{
    [Header("Platform Details")]
    [Tooltip("GameObject to be used as tile")]
    public GameObject tileObject;

    //Radius of the platform in number of tiles
    [Min(1)]
    [Tooltip("Only needed if generating a cicle. Represents radius of the platform in number of tiles")]
    public int radius = 10;

    //Width of the platform in number of tiles
    [Min(1)]
    [Tooltip("Only needed if generating a square. Represents width of the platform in number of tiles")]
    public int width = 10;

    //Length of the platform in number of tiles
    [Min(1)]
    [Tooltip("Only needed if generating a square. Represents length of the platform in number of tiles")]
    public int length = 10;

    [Min(0.001f)]
    [Tooltip("The spacing between each tile (should factor in the size of the tile)")]
    public float spacing = 1f;

    [Tooltip("The origin/center of the generated platform")]
    public Vector3 origin = new Vector3(0, -0.25f, 0);

    [SerializeField]
    [Tooltip("The shape of the platform. Remember to fill the respective dimensions for your chosen shape")]
    private PlatformShapes platformShape = PlatformShapes.Square;

    [HideInInspector]
    public List<GameObject> allTiles;

    private GameObject parentGameObject;

    [Space(30f)]
    [Header("Platform Spawning")]
    [Tooltip("Should a platform spawn on game start? This option will usually be false if you want to make a custom platform. \nTo make a custom platform, disable this variable and press the \"Generate Platform\" button below.")]
    public bool spawnPlatformOnLoad = true;

    [Tooltip("This field is just for debug purposes, to help keep track of the platforms generated so far")]
    public bool existingDynamicPlatform = false;

    [Tooltip("This field is just for debug purposes, to help keep track of the platforms generated so far")]
    private int currentPlatformNumber = 0;

    // Start is called before the first frame update
    private void Start()
    {
        if (spawnPlatformOnLoad)
        {
            Initialise();
        }
    }

    public void Initialise()
    {
        if (allTiles == null)
        {
            allTiles = new List<GameObject>();
        }
        if (parentGameObject == null)
        {
            parentGameObject = new GameObject();
            parentGameObject.name = GeneratePlatformParentName("Dynamic");
            currentPlatformNumber += 1;
        }
        GeneratePlatform();
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
        int initialTileCount = CalculateMaxTileCount() - allTiles.Count;
        for (int i = 0; i < initialTileCount; i++)
        {
            allTiles.Add(Instantiate(tileObject));
            allTiles[i].transform.parent = parentGameObject.transform;
        }
    }

    public void DeletePlatform()
    {
        if (existingDynamicPlatform)
        {
            for (int i = 0; i < allTiles.Count; i++)
            {
                DestroyImmediate(allTiles[i]);
            }
            allTiles.Clear();
            allTiles = null;
            DestroyImmediate(parentGameObject);
            parentGameObject = null;
            existingDynamicPlatform = false;
        }
    }

    public void StaticizePlatform()
    {
        if (existingDynamicPlatform)
        {
            allTiles.Clear();
            allTiles = null;
            parentGameObject.name = GeneratePlatformParentName("Static");
            currentPlatformNumber += 1;
            parentGameObject = null;
            existingDynamicPlatform = false;
        }
    }

    /// <summary>
    /// Used to generate a new dynamic platform. Is created and referenced by WorldGenerator
    /// </summary>
    public void GeneratePlatform()
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
                    allTiles[i].name = "Tile " + ((i % diameter - radius) + ", " + (i / diameter - radius));
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
                tilePos.x = origin.x + spacing * (i % width + spacing / 2) - (width / 2);
                tilePos.z = origin.z + spacing * (i / length + spacing / 2) - (length / 2);
                tilePos.y = origin.y;
                allTiles[i].transform.position = tilePos;
                allTiles[i].SetActive(true);
                allTiles[i].name = "Tile " + ((i % width) - (width / 2)) + ", " + ((i / length) - (length / 2));
            }
        }
        existingDynamicPlatform = true;
    }

    /// <summary>
    /// Used to generate a new static platform. Is created, but not referenced by WorldGenerator
    /// </summary>
    public void GenerateStaticPlatform()
    {
        List<GameObject> staticTiles = new List<GameObject>();
        GameObject staticPlatformParent = new GameObject();
        staticPlatformParent.name = GeneratePlatformParentName("Static");
        currentPlatformNumber += 1;
        int tileCount = CalculateMaxTileCount();
        for (int i = 0; i < tileCount; i++)
        {
            staticTiles.Add(Instantiate(tileObject));
            staticTiles[i].transform.parent = staticPlatformParent.transform;
        }

        if (platformShape == PlatformShapes.Circle)
        {
            for (int i = 0; i < tileCount; i++)
            {
                int diameter = radius * 2;
                Vector3 tilePos = staticTiles[i].transform.position;
                tilePos.x = origin.x + spacing * (i % diameter + spacing / 2) - radius;
                tilePos.z = origin.z + spacing * (i / diameter + spacing / 2) - radius;
                tilePos.y = origin.y;
                staticTiles[i].transform.position = tilePos;
                if ((tilePos - origin).magnitude <= radius)
                {
                    staticTiles[i].SetActive(true);
                    staticTiles[i].name = "Tile " + (i % diameter - radius) + ", " + (i / diameter - radius);
                }
                else
                {
                    staticTiles[i].SetActive(false);
                }
            }
        }
        else if (platformShape == PlatformShapes.Square)
        {
            for (int i = 0; i < tileCount; i++)
            {
                Vector3 tilePos = staticTiles[i].transform.position;
                tilePos.x = origin.x + spacing * (i % width + spacing / 2) - (width / 2);
                tilePos.z = origin.z + spacing * (i / length + spacing / 2) - (length / 2);
                tilePos.y = origin.y;
                staticTiles[i].transform.position = tilePos;
                staticTiles[i].SetActive(true);
                staticTiles[i].name = "Tile " + ((i % width) - (width / 2)) + ", " + ((i / length) - (length / 2));
            }
        }
    }

    private string GeneratePlatformParentName(string staticness)
    {
        return "Platform " + platformShape + currentPlatformNumber + " (" + staticness +")";
    }
}