using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildableUiItem : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public BuildingData buildingData;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag");

        BuildingSystem.current.InitializeWithGameObject(buildingData.prefab);

    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
