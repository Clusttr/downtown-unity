using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragItem : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    
    [SerializeField] private HouseVariations hVariation = HouseVariations.VarOne;

    private GameObject houseVariationPrefab;
    private GameObject variationDummyObj;
    private bool isInstantiated = false;
    private bool isDragging = false;

    private void Start()
    {
        SetUpPrefab();
    }

    private void SetUpPrefab()
    {
        houseVariationPrefab = GameManager.Instance.GetHousePrefabViaVariation(hVariation);
    }

    private void SpawnItemVariation()
    {

        if ( houseVariationPrefab != null && !isInstantiated)
        {
            variationDummyObj = Instantiate( houseVariationPrefab );
            isInstantiated = true;
        }
    }

    private void Update()
    {
        if(isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit raycastHit) )
            {
                variationDummyObj.transform.position = raycastHit.point;
                if (!GameManager.Instance.CanDropBuildingHere())
                {
                    Debug.Log("Can't drop here");
                    UpdateDummyColor(DummyUnitColor.Red);
                }
                else
                {
                    UpdateDummyColor(DummyUnitColor.Green);
                }
                
            }
        }

        if(isDragging && Input.GetMouseButtonDown(1))
        {
            Destroy(variationDummyObj);
            isDragging = false;
            isInstantiated = false;
        }

        if(isDragging && Input.GetKeyDown(KeyCode.Space))
        {
            variationDummyObj.transform.Rotate(new Vector3(0, 90, 0));
            houseVariationPrefab.transform.Rotate(new Vector3(0, 90, 0));
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        SpawnItemVariation();
        isDragging = true;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (GameManager.Instance.CanDropBuildingHere())
        {
            GameManager.Instance.SetUpUnit(hVariation);
        }

        Destroy(variationDummyObj);
        isDragging = false;
        isInstantiated = false;    
    }

    public void UpdateDummyColor(DummyUnitColor color)
    {
        MeshRenderer[] mesh = variationDummyObj.GetComponents<MeshRenderer>();
        //int numberOfMaterials = mesh.materials.Length;
        //Debug.Log(numberOfMaterials);

        foreach(MeshRenderer meshRenderer in mesh)
        {
            Material[] mats = meshRenderer.sharedMaterials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = GameManager.Instance.UpdateDummyUnitMaterial(color);
            }
            meshRenderer.sharedMaterials = mats;
        }

        //for (int i = 0; i < numberOfMaterials; i++)
        //{
        //    mesh.materials[i] = GameManager.Instance.UpdateDummyUnitMaterial(color);

        //    Debug.Log("Tada GOt Here");
        //}
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }
}
