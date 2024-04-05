using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildableUiItem : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerExitHandler, IPointerUpHandler
{
    public BuildingData buildingData;

    private Vector3 startClickPoint;
    private bool pointerDown;
    private bool stopDragOnThisFrame;


    private void DropBuiding()
    {
        BuildingSystem.current.InitializeWithGameObject(buildingData.prefab);
        transform.root.GetComponent<ConstructionUIHandler>().ToggleView();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {


        //

        //
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        stopDragOnThisFrame = false;
        startClickPoint = eventData.position;
        pointerDown = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerDown = false;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (pointerDown == false || stopDragOnThisFrame == true) return;

        float disY = (eventData.position.y - startClickPoint.y);
        float disX = (eventData.position.x - startClickPoint.x);
        if (disX > 20)
        {
            stopDragOnThisFrame = true;
        }

        if (disY > 5)
        {
            pointerDown = false;
            DropBuiding();
            Debug.Log("Drag");
            Web3Functions.Instance.InsertBuilding();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDown = false;
    }
}
