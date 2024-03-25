using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{

    private Vector3 offset;
    public bool isDraging;


    private void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if(BuildingSystem.GetMouseHitEvent(out RaycastHit raycastHit))
            {
                isDraging = false;
            }
        }

        if (isDraging)
        {
            Vector3 pos = BuildingSystem.GetMouseWorldPosition() + offset;
            transform.position = BuildingSystem.current.SnapCoordinateToGrid(pos);
        }
    }

    public void StartDrag()
    {
        offset = transform.position - BuildingSystem.GetMouseWorldPosition();
    }


    private void OnMouseDown()
    {
        StartDrag();
    }

    private void OnMouseDrag()
    {
        isDraging = true;
    }

    private void OnMouseUp()
    {
        isDraging = false;
    }
}
