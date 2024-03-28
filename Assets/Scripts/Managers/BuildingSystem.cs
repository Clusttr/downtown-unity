using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Progress;

public class BuildingSystem : MonoBehaviour
{

    public static BuildingSystem current;
    public LayerMask TileLayer;
    public GridLayout gridLayout;
    private Grid grid;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase whiteTile;

    private PlacableObject placableObject;
    

    private void Awake()
    {
        current = this;
        grid = gridLayout.GetComponent<Grid>();

    }

    private void Update()
    {

        if(placableObject == null)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            placableObject.Rotate();
        }
    }

    public void PlaceCurrentBuilding()
    {
        if (placableObject == null)
            return;

        placableObject.Place();
        Vector3 start = SnapCoordinateToGrid(placableObject.GetTilePivotPosition());

        TakeArea(start, placableObject.Size);


        //CanBePlaced(placableObject);
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

    private Vector3[] GetTileBlockPoint(BoundsInt area)
    {
        Vector3[] array = new Vector3[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var item in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(item.x, item.y, 0);
            array[counter] = grid.GetCellCenterWorld(pos); //tilemap.GetTile(pos);
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

    public bool CanBePlaced(PlacableObject placableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(placableObject.GetTilePivotPosition());
        area.size = new Vector3Int(placableObject.BuildingData.CellSize.x, placableObject.BuildingData.CellSize.y, 1);
        //area.size = new Vector3Int(area.size.x + 1, area.size.y + 1, area.size.z);
        area.size = new Vector3Int(area.size.x + (int)(1f / 0.25f), area.size.y + (int)(1f / 0.25f), area.size.z);

        Vector3[] points = GetTileBlockPoint(area);

        

        foreach (var item in points)
        {
            Collider[] colliders = Physics.OverlapSphere(item, 0.1f, TileLayer);
            //Debug.Log(colliders.Length);
            if (colliders.Length > 0)
            {
                //Debug.Log(colliders[0].tag);
                if (colliders[0].tag != "Grass")
                    return false;
                GridCell cell = colliders[0].gameObject.GetComponent<GridCell>();
                if (cell == null || cell.occupied)
                    return false;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public void TakeArea(Vector3 start, Vector3Int size )
    {

        //tilemap.BoxFill(start, whiteTile, start.x, start.y, start.x + size.y, start.y + size.y);
        //Debug.Log(start + " " + size);

        

        BoundsInt area = new BoundsInt();
        
        area.position = gridLayout.WorldToCell(placableObject.GetTilePivotPosition());
        area.size = new Vector3Int(placableObject.BuildingData.CellSize.x, placableObject.BuildingData.CellSize.y, 1);
        area.size = new Vector3Int(area.size.x + (int)(1f / 0.25f), area.size.y + (int)(1f / 0.25f), area.size.z);

        //Debug.Log(area.size);

        Vector3[] points = GetTileBlockPoint(area);
        //Debug.Log(points.Length);

        for (int i = 0; i < points.Length; i++)
        {
            //GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //gameObject.transform.position = points[i];
            //gameObject.transform.localScale = Vector3.one * 0.2f;

            Collider[] colliders = Physics.OverlapSphere(points[i], 0.1f, TileLayer);
            if(colliders.Length > 0)
            {
                //GameObject tile = colliders[0].gameObject;
                colliders[0].GetComponent<GridCell>().occupied = true;
            }
        }

        
    }
}
