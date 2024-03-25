using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionUIHandler : MonoBehaviour
{

    public RectTransform confirmationUI;

    public Vector3 openPosition;

    private void Start()
    {
        openPosition = confirmationUI.transform.localPosition;
        Close();
    }

    public void Open()
    {
        confirmationUI.DOLocalMoveX(openPosition.x, 1).SetEase(Ease.OutBounce);
    }

    public void Close() 
    {
        confirmationUI.DOLocalMoveX(openPosition.x + 100, 1).SetEase(Ease.OutBounce);
    }


    public void OnComfirmClicked()
    {
        BuildingSystem.current.PlaceCurrentBuilding();
    }

    public void OnCancelClicked()
    {
        BuildingSystem.current.CancelPlacement();
    }
}
