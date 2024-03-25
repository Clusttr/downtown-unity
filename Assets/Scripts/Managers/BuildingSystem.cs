using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Progress;

public class BuildingSystem : MonoBehaviour
{

    public static BuildingSystem current;

    public GridLayout gridLayout;
    private Grid grid;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase whiteTile;


    public GameObject building1;
    public GameObject building2;

    private PlacableObject placableObject;


    private void Awake()
    {
        current = this;
        grid = gridLayout.GetComponent<Grid>();

    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.A))
        //{
        //    InitializeWithGameObject(building1);
        //}else if(Input.GetKeyDown(KeyCode.B))
        //{
        //    InitializeWithGameObject(building2);    
        //}

        if(placableObject == null)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            //if (CanBePlaced(placableObject))
            //{
                
            //}
            //else
            //{
            //    Destroy(placableObject.gameObject);
            //}
        }
        else if(Input.GetKeyDown(KeyCode.Return))
        {
            placableObject.Rotate();
        }
    }

    public void PlaceCurrentBuilding()
    {
        if (placableObject == null)
            return;

        placableObject.Place();
        Vector3Int start = gridLayout.WorldToCell(placableObject.GetStartPosition());
        //TakeArea(start, placableObject.Size);
        CanBePlaced(placableObject);

        FindAnyObjectByType<ConstructionUIHandler>().Close();

        
    }

    public void CancelPlacement()
    {
        if (placableObject == null)
            return;
        Destroy(placableObject.gameObject);
    }

    public static Vector3 GetMouseWorldPosition()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(GetMouseHitEvent(out RaycastHit raycastHit))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public static bool GetMouseHitEvent(out RaycastHit raycastHit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit))
        {
            return true;
        }
        return false;
    }

    public Vector3 SnapCoordinateToGrid(Vector3 position )
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position );
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }

    private static TileBase[] GetTileBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var item in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(item.x, item.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }
        return array;
    }



    public void InitializeWithGameObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinateToGrid(GetMouseWorldPosition());

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        placableObject = obj.GetComponent<PlacableObject>();
        obj.AddComponent<ObjectDrag>().StartDrag();
        obj.GetComponent<ObjectDrag>().isDraging = true;
        placableObject.ShowBuildingDummy();

        FindAnyObjectByType<ConstructionUIHandler>().Open();
    }

    private bool CanBePlaced(PlacableObject placableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(placableObject.GetStartPosition());
        area.size = new Vector3Int(area.size.x + 1, area.size.y + 1, area.size.z);

        

        TileBase[] baseArray = GetTileBlock(area, tilemap);
        Debug.Log(baseArray.Length);

        foreach (var item in baseArray)
        {
            Debug.Log(item.name);

            if(item == whiteTile)
            {
                return false;
            }
        }
        return true;
    }

    public void TakeArea(Vector3Int start, Vector3Int size )
    {
        
        tilemap.BoxFill(start, whiteTile, start.x, start.y, start.x + size.y, start.y + size.y);

    }
}
