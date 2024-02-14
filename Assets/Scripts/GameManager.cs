using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DummyUnitColor
{
    None = 0,
    Red = 1,
    Green = 2,
}
public class GameManager : MonoBehaviour
{
    [Header("Art Stuff")]
    [SerializeField] private Material tileMaterial;
    [SerializeField] private float tileSize = 1.0f;
    [SerializeField] private float yOffset = 0.2f;
    [SerializeField] private Vector3 boardCenter = Vector3.zero;

    [Header("Prefabs")]
    [SerializeField] private GameObject[] prefabs;

    [SerializeField] private Material redMaterial;
    [SerializeField] private Material greenMaterial;

    private const int TILE_COUNT_X = 20;
    private const int TILE_COUNT_Y = 20;
    private Buildings[,] buildings;
    private Buildings currentlyDragging;
    private GameObject[,] tiles;
    private Camera currentCamera;
    private Vector2Int currentHover;
    private Vector3 bounds;

    public static GameManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        GenerateAllTiles(tileSize, TILE_COUNT_X, TILE_COUNT_Y);

        buildings = new Buildings[TILE_COUNT_X, TILE_COUNT_Y];

    }

    private void Update()
    {
        if(!currentCamera)
        {
            currentCamera = Camera.main;
            return;
        }

        RaycastHit info;
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Tile", "Hover")))
        {
            Vector2Int hitPosition = LookupTileIndex(info.transform.gameObject);

            if(currentHover == -Vector2Int.one)
            {
                currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            }

            if(currentHover != hitPosition)
            {
                tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
                currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            }

            CanDropBuildingHere();

            if (Input.GetMouseButtonDown(0))
            {
                if(buildings == null)
                {
                    return;
                }

                if (buildings[hitPosition.x, hitPosition.y] != null)
                {
                    currentlyDragging = buildings[hitPosition.x, hitPosition.y];

                }
                //else
                //{
                //    Debug.Log("There's no bulding here");
                //}
            }

            if (currentlyDragging != null && Input.GetMouseButtonUp(0))
            {
                Vector2Int previousPosition = new Vector2Int(currentlyDragging.currentX, currentlyDragging.currentY);

                bool validMove = MoveTo(currentlyDragging, hitPosition.x, hitPosition.y);

                Debug.Log(validMove);

                if(validMove)
                {
                    buildings[previousPosition.x, previousPosition.y] = null;
                    buildings[hitPosition.x, hitPosition.y] = currentlyDragging;
                    PositionSingleUnit(hitPosition.x, hitPosition.y);
                    currentlyDragging = null;

                }

                else 
                {
                    currentlyDragging.transform.position = GetTileCenter(previousPosition.x, previousPosition.y);
                    currentlyDragging = null;

                }
            }

        }
        else
        {
            if(currentHover != -Vector2Int.one)
            {
                tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
                currentHover = -Vector2Int.one;
            }
        }

        
    }

    private bool MoveTo(Buildings hut, int x, int y)
    {
        Vector2Int previousPosition = new Vector2Int(hut.currentX, hut.currentY);

        if (buildings[x, y] != null)
        {
            Debug.Log("Can't move here");
            return false;
        }
            
        return true;
    }

    public bool CanDropBuildingHere()
    {
        if(buildings[currentHover.x, currentHover.y] == null)
        {
            return true;
        }

        return false;
        
    }

    private void GenerateAllTiles(float tileSize, int tileCountX, int tileCountY)
    {
        yOffset += transform.position.y;
        bounds = new Vector3((tileCountX / 2) * tileSize, 0, (tileCountY / 2) * tileSize) + boardCenter;

        tiles = new GameObject[tileCountX, tileCountY];
        for (int x = 0; x < tileCountX; x++)
            for (int y = 0; y < tileCountY; y++)
                tiles[x, y] = GenerateSingleTile(tileSize, x, y);

    }

    private GameObject GenerateSingleTile(float tileSize, int x, int y)
    {
        GameObject tileObject = new GameObject(string.Format("X:{0}, Y:{1}", x, y));
        tileObject.transform.parent = transform;

        Mesh mesh = new Mesh();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>().material = tileMaterial;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tileSize, yOffset, y * tileSize) - bounds;
        vertices[1] = new Vector3(x * tileSize, yOffset, (y+1) * tileSize) - bounds;
        vertices[2] = new Vector3((x+1) * tileSize, yOffset, y* tileSize) - bounds;
        vertices[3] = new Vector3((x + 1) * tileSize, yOffset, (y + 1) * tileSize) - bounds;

        int[] tris = new int[] {0, 1, 2, 1, 3, 2};

        mesh.vertices = vertices;
        mesh.triangles = tris;

        mesh.RecalculateNormals();

        tileObject.layer = LayerMask.NameToLayer("Tile");
        tileObject.AddComponent<BoxCollider>();


        return tileObject;
    }

    private Vector2Int LookupTileIndex(GameObject hitInfo)
    {
        for (int x = 0; x < TILE_COUNT_X; x++)
            for (int y = 0; y < TILE_COUNT_X; y++)
                if (tiles[x, y] == hitInfo)
                    return new Vector2Int(x, y);

        return -Vector2Int.one;
    }


    //Spawn Buildings
    private Buildings SpawnSingleUnit(HouseVariations type)
    {
        Buildings hut = Instantiate(prefabs[(int)type - 1], transform).GetComponent<Buildings>();

        hut.type = type;
        
        return hut;
    }

    private void SpawnTheUnit(HouseVariations hv)
    {
        buildings[currentHover.x, currentHover.y] = SpawnSingleUnit(hv);

    }

    private void PositionSingleUnit( int x, int y, bool force = false)
    {
        buildings[x, y].currentX = x;
        buildings[x, y].currentY = y;
        buildings[x, y].transform.position = GetTileCenter(x, y);
    }

    private void PositionTheUnit()
    {
        for (int x = 0; x < TILE_COUNT_X; x++)
            for (int y = 0; y < TILE_COUNT_Y; y++)
                if (buildings[x, y] != null)
                    PositionSingleUnit(x, y, true);
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        return new Vector3(x * tileSize, yOffset, y * tileSize) - bounds + new Vector3(tileSize / 2, 0, tileSize / 2);
    }


    public GameObject GetHousePrefabViaVariation(HouseVariations variation)
    {
        switch (variation)
        {
            case HouseVariations.VarOne: return prefabs[0];
            case HouseVariations.VarTwo: return prefabs[1];
            case HouseVariations.VarThree: return prefabs[2];
            default: return prefabs[0];
        }
    }

    public void SetUpUnit(HouseVariations hv)
    {
        SpawnTheUnit(hv);
        PositionSingleUnit(currentHover.x, currentHover.y, true);
    }

    public Material UpdateDummyUnitMaterial(DummyUnitColor color)
    {
        switch (color)
        {
            case DummyUnitColor.None:
                return greenMaterial;
            case DummyUnitColor.Red:
                return redMaterial;
            case DummyUnitColor.Green:
                return greenMaterial;
            default:
                return greenMaterial;
        }

    }
}
