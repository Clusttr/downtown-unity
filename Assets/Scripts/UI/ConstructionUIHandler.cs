using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class ConstructionUIHandler : MonoBehaviour
{

    public RectTransform confirmationUI;
    public RectTransform constructionUI;
    public RectTransform buildingSelectionUI;

    public Button constructionBarButton;
    public UIFlippable flipabbleImage;

    public BuildableUiItem[] buildableUiItems;

    public Vector3 openPosition;

    private bool isOpen = true;

    private void Start()
    {
        openPosition = confirmationUI.transform.localPosition;

        constructionBarButton.onClick.AddListener(() =>
        {
            ToggleView();
        });

        ToggleView();

        Debug.Log(constructionUI.position);

        Close();
        ToggleBuidingSelectView(false);
    }


    public void ToggleView()
    {
        if (isOpen)
        {
            isOpen = false;
            constructionUI.DOMoveY(0, 0.2f);
            flipabbleImage.vertical = true;
        }
        else
        {
            constructionUI.DOMoveY(100, .2f);
            isOpen = true;
            flipabbleImage.vertical = false;
        }
        flipabbleImage.GetComponent<Graphic>().SetVerticesDirty();
    }



    public void Open()
    {
        confirmationUI.DOLocalMoveX(openPosition.x, 1).SetEase(Ease.OutBounce);
    }

    public void Close() 
    {
        confirmationUI.DOLocalMoveX(openPosition.x + 100, 1).SetEase(Ease.OutBounce);
    }

    public void ToggleBuidingSelectView(bool open)
    {
        if(open)
        {
            buildingSelectionUI.DOLocalMoveX(openPosition.x, 1).SetEase(Ease.OutBounce);
        }
        else
        {
            buildingSelectionUI.DOLocalMoveX(openPosition.x + 100, 1).SetEase(Ease.OutBounce);
        }
    }


    public void OnRotate()
    {
        BuildingSystem.current.RotateCurrentBuilding();
    }

    public void OnComfirmClicked()
    {
        BuildingSystem.current.PlaceCurrentBuilding();
        Close();
    }

    public void OnCancelClicked()
    {
        BuildingSystem.current.CancelPlacement();
        Close();
    }

    public void OnDeleteClicked()
    {
        ToggleBuidingSelectView(false);

        BuildingSystem.current.RemoveSelectedBuilding();
    }

    public void UpdateBuildingItemList(string[] buildings)
    {

        for (int i = 0; i < buildableUiItems.Length; i++)
        {
            buildableUiItems[i].gameObject.SetActive(true);
            for (int x = 0; x < buildings.Length; x++)
            {
                if (buildableUiItems[i].buildingData.Name == buildings[x])
                {
                    buildableUiItems[i].gameObject.SetActive(false);
                    continue;
                }
            }

            
        }
    }
}
