
using System;
using UnityEngine;


public enum DummyUnitColor
{
    None = 0,
    Red = 1,
    Green = 2,
}
public class GameManager : Singleton<GameManager>
{
    //[Header("Art Stuff")]
    //[SerializeField] private Material tileMaterial;
    //[SerializeField] private float tileSize = 1.0f;
    //[SerializeField] private float yOffset = 0.2f;
    //[SerializeField] private Vector3 boardCenter = Vector3.zero;

    //[Header("Prefabs")]
    //[SerializeField] private GameObject[] prefabs;

    //[SerializeField] private Material redMaterial;
    //[SerializeField] private Material greenMaterial;

    //private const int TILE_COUNT_X = 20;
    //private const int TILE_COUNT_Y = 20;
    //private Buildings[,] buildings;
    //private Buildings currentlyDragging;
    //private GameObject[,] tiles;
    //private Camera currentCamera;
    //private Vector2Int currentHover;
    //private Vector3 bounds;

    private void Start()
    {
       // GenerateAllTiles(tileSize, TILE_COUNT_X, TILE_COUNT_Y);

        //buildings = new Buildings[TILE_COUNT_X, TILE_COUNT_Y];

       // SpawnEnvironment();
        //PositionTheUnit();


    }

    private void Update()
    {
        //return;
        //if(!currentCamera)
        //{
        //    currentCamera = Camera.main;
        //    return;
        //}

        //RaycastHit info;
        //Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        //if(Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Tile", "Hover")))
        //{
        //    Vector2Int hitPosition = LookupTileIndex(info.transform.gameObject);

        //    if(currentHover == -Vector2Int.one)
        //    {
        //        currentHover = hitPosition;
        //        tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
        //    }

        //    if(currentHover != hitPosition)
        //    {
        //        tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
        //        currentHover = hitPosition;
        //        tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
        //    }

        //    CanDropBuildingHere();

        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        if(buildings == null)
        //        {
        //            return;
        //        }

        //        if (buildings[hitPosition.x, hitPosition.y] != null)
        //        {
        //            currentlyDragging = buildings[hitPosition.x, hitPosition.y];

        //        }
        //        //else
        //        //{
        //        //    Debug.Log("There's no bulding here");
        //        //}
        //    }

        //    if (currentlyDragging != null && Input.GetMouseButtonUp(0))
        //    {
        //        Vector2Int previousPosition = new Vector2Int(currentlyDragging.currentX, currentlyDragging.currentY);

        //        bool validMove = MoveTo(currentlyDragging, hitPosition.x, hitPosition.y);

        //        Debug.Log(validMove);

        //        if(validMove)
        //        {
        //            buildings[previousPosition.x, previousPosition.y] = null;
        //            buildings[hitPosition.x, hitPosition.y] = currentlyDragging;
        //            PositionSingleUnit(hitPosition.x, hitPosition.y);
        //            currentlyDragging = null;

        //        }

        //        else 
        //        {
        //            currentlyDragging.transform.position = GetTileCenter(previousPosition.x, previousPosition.y);
        //            currentlyDragging = null;

        //        }
        //    }

        //}
        //else
        //{
        //    if(currentHover != -Vector2Int.one)
        //    {
        //        tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
        //        currentHover = -Vector2Int.one;
        //    }
        //}

        
    }

    //private bool MoveTo(Buildings hut, int x, int y)
    //{
    //    Vector2Int previousPosition = new Vector2Int(hut.currentX, hut.currentY);

    //    if (buildings[x, y] != null)
    //    {
    //        Debug.Log("Can't move here");
    //        return false;
    //    }
            
    //    return true;
    //}

    //public bool CanDropBuildingHere()
    //{
    //    if(buildings[currentHover.x, currentHover.y].GetComponent<GreenSoil>())
    //    {
    //        return true;
    //    }

    //    return false;
        
    //}

    //private void GenerateAllTiles(float tileSize, int tileCountX, int tileCountY)
    //{
    //    yOffset += transform.position.y;
    //    bounds = new Vector3((tileCountX / 2) * tileSize, 0, (tileCountY / 2) * tileSize) + boardCenter;

    //    tiles = new GameObject[tileCountX, tileCountY];
    //    for (int x = 0; x < tileCountX; x++)
    //        for (int y = 0; y < tileCountY; y++)
    //            tiles[x, y] = GenerateSingleTile(tileSize, x, y);

    //}

    //private GameObject GenerateSingleTile(float tileSize, int x, int y)
    //{
    //    GameObject tileObject = new GameObject(string.Format("X:{0}, Y:{1}", x, y));
    //    tileObject.transform.parent = transform;

    //    Mesh mesh = new Mesh();
    //    tileObject.AddComponent<MeshFilter>().mesh = mesh;
    //    tileObject.AddComponent<MeshRenderer>().material = tileMaterial;

    //    Vector3[] vertices = new Vector3[4];
    //    vertices[0] = new Vector3(x * tileSize, yOffset, y * tileSize) - bounds;
    //    vertices[1] = new Vector3(x * tileSize, yOffset, (y+1) * tileSize) - bounds;
    //    vertices[2] = new Vector3((x+1) * tileSize, yOffset, y* tileSize) - bounds;
    //    vertices[3] = new Vector3((x + 1) * tileSize, yOffset, (y + 1) * tileSize) - bounds;

    //    int[] tris = new int[] {0, 1, 2, 1, 3, 2};

    //    mesh.vertices = vertices;
    //    mesh.triangles = tris;

    //    mesh.RecalculateNormals();

    //    tileObject.layer = LayerMask.NameToLayer("Tile");
    //    tileObject.AddComponent<BoxCollider>();


    //    return tileObject;
    //}

    //private Vector2Int LookupTileIndex(GameObject hitInfo)
    //{
    //    for (int x = 0; x < TILE_COUNT_X; x++)
    //        for (int y = 0; y < TILE_COUNT_X; y++)
    //            if (tiles[x, y] == hitInfo)
    //                return new Vector2Int(x, y);

    //    return -Vector2Int.one;
    //}


    ////Spawn Buildings
    //private Buildings SpawnSingleUnit(HouseVariations type, Quaternion newRotation)
    //{

    //    GameObject building = new GameObject();

    //    GameObject instantiatedBuilding = Instantiate(building, transform);
    //    Buildings hut = AddComponentToGameObject<Buildings>(type, instantiatedBuilding);
    //    hut.transform.rotation = newRotation;


    //    hut.type = type;
    //    hut.SetUpData();

    //    return hut;


    //}

    //private void SpawnEnvironment()
    //{
    //    buildings = new Buildings[TILE_COUNT_X, TILE_COUNT_Y];

    //    buildings[2, 4] = SpawnSingleUnit(HouseVariations.Road1, Quaternion.Euler(new Vector3(0, 90, 0)));

    //    buildings[18, 4] = SpawnSingleUnit(HouseVariations.Road1, Quaternion.Euler(new Vector3(0, -90, 0)));

    //    buildings[18, 10] = SpawnSingleUnit(HouseVariations.Road1, Quaternion.Euler(new Vector3(0, -90, 0)));

    //    buildings[18, 16] = SpawnSingleUnit(HouseVariations.Road1, Quaternion.Euler(new Vector3(0, -90, 0)));

    //    buildings[2, 10] = SpawnSingleUnit(HouseVariations.Road1, Quaternion.Euler(new Vector3(0, 90, 0)));

    //    buildings[2, 16] = SpawnSingleUnit(HouseVariations.Road1, Quaternion.Euler(new Vector3(0, 90, 0)));

    //    for (int x = 2; x < 3; x++)
    //        for (int y = 0; y < 4; y++)
    //            buildings[x, y] = SpawnSingleUnit(HouseVariations.Road3, Quaternion.Euler(new Vector3(0, 0, 0)));

    //    for (int x = 2; x < 3; x++)
    //        for (int y = 5; y < 10; y++)
    //            buildings[x, y] = SpawnSingleUnit(HouseVariations.Road3, Quaternion.Euler(new Vector3(0, 0, 0)));

    //    for (int x = 2; x < 3; x++)
    //        for (int y = 11; y < 16; y++)
    //            buildings[x, y] = SpawnSingleUnit(HouseVariations.Road3, Quaternion.Euler(new Vector3(0, 0, 0)));

    //    for (int x = 2; x < 3; x++)
    //        for (int y = 17; y < 20; y++)
    //            buildings[x, y] = SpawnSingleUnit(HouseVariations.Road3, Quaternion.Euler(new Vector3(0, 0, 0)));

    //    for (int x = 3; x < 18; x++)
    //        for (int y = 4; y < 5; y++)
    //            buildings[x, y] = SpawnSingleUnit(HouseVariations.Road3, Quaternion.Euler(new Vector3(0, 90, 0)));

    //    for (int x = 18; x < 19; x++)
    //        for (int y = 5; y < 10; y++)
    //            buildings[x, y] = SpawnSingleUnit(HouseVariations.Road3, Quaternion.Euler(new Vector3(0, 0, 0)));

    //    for (int x = 18; x < 19; x++)
    //        for (int y = 11; y < 16; y++)
    //            buildings[x, y] = SpawnSingleUnit(HouseVariations.Road3, Quaternion.Euler(new Vector3(0, 0, 0)));

    //    for (int x = 18; x < 19; x++)
    //        for (int y = 17; y < 20; y++)
    //            buildings[x, y] = SpawnSingleUnit(HouseVariations.Road3, Quaternion.Euler(new Vector3(0, 0, 0)));

    //    for (int x = 18; x < 19; x++)
    //        for (int y = 0; y < 4; y++)
    //            buildings[x, y] = SpawnSingleUnit(HouseVariations.Road3, Quaternion.Euler(new Vector3(0, 0, 0)));

    //    for (int x = 3; x < 18; x++)
    //        for (int y = 10; y < 11; y++)
    //            buildings[x, y] = SpawnSingleUnit(HouseVariations.Road3, Quaternion.Euler(new Vector3(0, 90, 0)));

    //    for (int x = 3; x < 18; x++)
    //        for (int y = 16; y < 17; y++)
    //            buildings[x, y] = SpawnSingleUnit(HouseVariations.Road3, Quaternion.Euler(new Vector3(0, 90, 0)));

    //    for (int x = 0; x < 2; x++)
    //        for (int y = 0; y < 20; y++)
    //            buildings[x, y] = SpawnSingleUnit(HouseVariations.BrownSoil, Quaternion.Euler(new Vector3(0, 0, 0)));

    //    for (int x = 3; x < 18; x++)
    //        for (int y = 17; y < 20; y++)
    //            buildings[x, y] = SpawnSingleUnit(HouseVariations.BrownSoil, Quaternion.Euler(new Vector3(0, 0, 0)));

    //    for (int x = 19; x < 20; x++)
    //        for (int y = 0; y < 20; y++)
    //            buildings[x, y] = SpawnSingleUnit(HouseVariations.BrownSoil, Quaternion.Euler(new Vector3(0, 0, 0)));

    //    for (int x = 3; x < 18; x++)
    //        for (int y = 11; y < 16; y++)
    //            buildings[x, y] = SpawnSingleUnit(HouseVariations.GreenSoil, Quaternion.Euler(new Vector3(0, 0, 0)));

    //    for (int x = 3; x < 18; x++)
    //        for (int y = 5; y < 10; y++)
    //            buildings[x, y] = SpawnSingleUnit(HouseVariations.GreenSoil, Quaternion.Euler(new Vector3(0, 0, 0)));

    //    for (int x = 3; x < 18; x++)
    //        for (int y = 0; y < 4; y++)
    //            buildings[x, y] = SpawnSingleUnit(HouseVariations.GreenSoil, Quaternion.Euler(new Vector3(0, 0, 0)));






    //}



    //private T AddComponentToGameObject<T> (HouseVariations type, GameObject obj) where T : Component
    //{
    //    Type componentType = null;

    //    switch (type)
    //    {
    //        case HouseVariations.VarOne:
    //            componentType = typeof(VariationOne);
    //            break;
    //        case HouseVariations.VarTwo:
    //            componentType = typeof(VariationTwo);
    //            break;
    //        case HouseVariations.VarThree:
    //            componentType = typeof(VariationThree);
    //            break;
    //        case HouseVariations.GreenSoil:
    //            componentType = typeof(GreenSoil);
    //            break;
    //        case HouseVariations.RedSoil:
    //            componentType = typeof(RedSoil);
    //            break;
    //        case HouseVariations.BrownSoil:
    //            componentType = typeof(BrownSoil);
    //            break;
    //        case HouseVariations.Road1:
    //            componentType = typeof(Road1);
    //            break;
    //        case HouseVariations.Road2:
    //            componentType = typeof(Road2);
    //            break;
    //        case HouseVariations.Road3:
    //            componentType = typeof(Road3);
    //            break;
    //        case HouseVariations.Road4:
    //            componentType = typeof(Road4);
    //            break;
    //        default:
    //            break;
    //            // Add more cases for other variations as needed
    //    }

    //    if (componentType != null)
    //    {
    //        T component = obj.AddComponent(componentType) as T;

    //        if (component == null)
    //        {
    //            Debug.LogError("Failed to add component of type " + typeof(T) + " to the GameObject.");
    //        }

    //        return component ;
    //    }
    //    else
    //    {
    //        Debug.LogError("Unknown HouseVariations type: " + type);
    //        return null;
    //    }
    //}

    //private void SpawnTheUnit(HouseVariations hv)
    //{
    //    buildings[currentHover.x, currentHover.y] = SpawnSingleUnit(hv, transform.rotation);

    //}

    //private void PositionSingleUnit( int x, int y, bool force = false)
    //{
    //    buildings[x, y].currentX = x;
    //    buildings[x, y].currentY = y;
    //    buildings[x, y].transform.position = GetTileCenter(x, y);
    //}

    //private void PositionTheUnit()
    //{
    //    for (int x = 0; x < TILE_COUNT_X; x++)
    //        for (int y = 0; y < TILE_COUNT_Y; y++)
    //            if (buildings[x, y] != null)
    //                PositionSingleUnit(x, y, true);
    //}

    //private Vector3 GetTileCenter(int x, int y)
    //{
    //    return new Vector3(x * tileSize, yOffset, y * tileSize) - bounds + new Vector3(tileSize / 2, 0, tileSize / 2);
    //}


    //public GameObject GetHousePrefabViaVariation(HouseVariations variation)
    //{
    //    switch (variation)
    //    {
    //        case HouseVariations.VarOne: return prefabs[0];
    //        case HouseVariations.VarTwo: return prefabs[1];
    //        case HouseVariations.VarThree: return prefabs[2];
    //        case HouseVariations.GreenSoil: return prefabs[3];
    //        case HouseVariations.RedSoil: return prefabs[4];
    //        case HouseVariations.BrownSoil: return prefabs[5];
    //        case HouseVariations.Road1: return prefabs[6];
    //        case HouseVariations.Road2: return prefabs[7];
    //        case HouseVariations.Road3: return prefabs[8];
    //        case HouseVariations.Road4: return prefabs[9];
    //        default: return prefabs[0];
    //    }
    //}

    //public void SetUpUnit(HouseVariations hv)
    //{
    //    SpawnTheUnit(hv);
    //    PositionSingleUnit(currentHover.x, currentHover.y, true);
    //}

    //public Material UpdateDummyUnitMaterial(DummyUnitColor color)
    //{
    //    switch (color)
    //    {
    //        case DummyUnitColor.None:
    //            return greenMaterial;
    //        case DummyUnitColor.Red:
    //            return redMaterial;
    //        case DummyUnitColor.Green:
    //            return greenMaterial;
    //        default:
    //            return greenMaterial;
    //    }

    //}
}
