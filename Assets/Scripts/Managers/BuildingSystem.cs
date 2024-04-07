using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Progress;

public class BuildingSystem : MonoBehaviour
{

    public static BuildingSystem current;
    public LayerMask TileLayer;
    public GridLayout gridLayout;
    public GameObject buildingFloor;

    public Grid grid;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase whiteTile;

    private PlacableObject placableObject;
    private PlacableObject currentSelection;


    private bool clickOnABuilding;
    private float timeToSelectABuilding = 1f;
    private float timeToSelectABuildingTimer = 0;
    private Vector3 startClickPoint;

    private List<PlacableObject> createdBuidings;
    private DateTime clickTime;


    private void Awake()
    {
        current = this;
        grid = gridLayout.GetComponent<Grid>();
        createdBuidings = new List<PlacableObject>();

    }

    private void Start()
    {
        GameManager.Instance.cameraActions.BuildingSys.Select.performed += (x) => OnInputMouseLeftButton(true);
        GameManager.Instance.cameraActions.BuildingSys.Select.canceled += (x) => OnInputMouseLeftButton(false);
    }

    private void Update()
    {

        if(placableObject == null)
        {
            return;
        }

        if(clickOnABuilding)
        {
            timeToSelectABuildingTimer += Time.deltaTime;
            if (GetMouseHitEvent(out RaycastHit raycastHit))
            {
                if (raycastHit.collider.tag == "Building")
                {
                    float dis = Vector3.Distance(startClickPoint, raycastHit.point);
                    if(dis > 0.2f)
                    {
                        clickOnABuilding = false;
                        currentSelection = null;
                        return;
                    }
                }
            }
            

            if (timeToSelectABuildingTimer> timeToSelectABuilding)
            {
                clickOnABuilding = false;
                timeToSelectABuildingTimer = 0;
                SelectedPlacedBuidling();
            }
        }
        
    }


    private void OnInputMouseLeftButton(bool down)
    {
        //Debug.Log(down);

        if(down)
        {
            if (GetMouseHitEvent(out RaycastHit raycastHit))
            {
                clickTime = DateTime.UtcNow;
                startClickPoint = raycastHit.point;
                if (raycastHit.collider.tag == "Building")
                {

                    if (currentSelection != null)
                    {
                        if(currentSelection != raycastHit.collider.GetComponent<PlacableObject>())
                        {
                            currentSelection = null;
                            clickOnABuilding = false;
                            CancleSelectedPlacedBuilding();
                        }
                    }


                    if (raycastHit.collider.GetComponent<PlacableObject>().Placed)
                    {
                        timeToSelectABuildingTimer = 0;
                        clickOnABuilding = true;
                        
                        currentSelection = raycastHit.collider.GetComponent<PlacableObject>();
                    }

                }
                else
                {

                }
            }
        }
        else
        {
            clickOnABuilding = false;
            timeToSelectABuildingTimer = 0;

            float diff = (float)DateTime.UtcNow.Subtract(clickTime).TotalSeconds;
            if(diff < 0.1f && currentSelection != null)
            {
                CancleSelectedPlacedBuilding();
                //if (GetMouseHitEvent(out RaycastHit raycastHit))
                //{

                //}
            }

            
        }
    }

    private void SelectedPlacedBuidling()
    {
        if(currentSelection != null)
        {
            currentSelection.HighlightFloor(true);
            FindAnyObjectByType<ConstructionUIHandler>().ToggleBuidingSelectView(true);
        }
    }


    public void RemoveSelectedBuilding()
    {
        if(currentSelection != null)
        {

            for (int i = 0; i < currentSelection.buildingGridCells.Count; i++)
            {
                currentSelection.buildingGridCells[i].occupied = false;
            }

            createdBuidings.Remove(currentSelection);

            Destroy(currentSelection.gameObject);
            currentSelection = null;


            FindAnyObjectByType<ConstructionUIHandler>().UpdateBuildingItemList(createdBuidings.Select(x => x.BuildingData.Name).ToArray());

        }
    }

    private void CancleSelectedPlacedBuilding()
    {
        if (currentSelection != null)
        {
            currentSelection.HighlightFloor(false);
            FindAnyObjectByType<ConstructionUIHandler>().ToggleBuidingSelectView(false);
        }
    }


    public void PlaceCurrentBuilding()
    {
        if (placableObject == null)
            return;

        placableObject.Place();

        TakeArea(placableObject);

        FindAnyObjectByType<ConstructionUIHandler>().Close();

        createdBuidings.Add(placableObject);

        FindAnyObjectByType<ConstructionUIHandler>().UpdateBuildingItemList(createdBuidings.Select(x => x.BuildingData.Name).ToArray());
    }

    public void RotateCurrentBuilding()
    {
        if(placableObject!= null)
        {
            placableObject.Rotate();
        }
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

    public void TakeArea(PlacableObject _placableObject )
    {
        

        BoundsInt area = new BoundsInt();
        
        area.position = gridLayout.WorldToCell(_placableObject.GetTilePivotPosition());
        area.size = new Vector3Int(_placableObject.Size.x, _placableObject.Size.y, 1);
        area.size = new Vector3Int(area.size.x + (int)(1f / 0.25f), area.size.y + (int)(1f / 0.25f), area.size.z);

        //Debug.Log(area.size);

        Vector3[] points = GetTileBlockPoint(area);
        //Debug.Log(points.Length);

        for (int i = 0; i < points.Length; i++)
        {
            GameObject gameObject = Instantiate(buildingFloor, _placableObject.transform);// GameObject.CreatePrimitive(PrimitiveType.Sphere);
            gameObject.transform.position = points[i] + Vector3.up * 0.002f;
            gameObject.transform.localScale = Vector3.one * 0.25f;

            placableObject.buidlingFloors.Add(gameObject.GetComponent<GridCell>());

            Collider[] colliders = Physics.OverlapSphere(points[i], 0.1f, TileLayer);
            if(colliders.Length > 0)
            {
                //GameObject tile = colliders[0].gameObject;
                colliders[0].GetComponent<GridCell>().occupied = true;
                placableObject.buildingGridCells.Add(colliders[0].GetComponent<GridCell>());
            }
        }

        
    }
}
