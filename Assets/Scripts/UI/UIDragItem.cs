using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragItem : MonoBehaviour, IDragHandler, IEndDragHandler
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
            }
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        SpawnItemVariation();
        isDragging = true;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        isInstantiated = false;
        GameManager.Instance.testSpawn(hVariation);
        Destroy(variationDummyObj);
        SetUpPrefab();
    }
}
