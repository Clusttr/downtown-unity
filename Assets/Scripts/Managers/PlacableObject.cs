using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlacableObject : MonoBehaviour
{
    public GameObject buildingBody;
    public Material redMat;
    public Material greenMat;

    public bool Placed { get; private set; }
    public Vector3Int Size { get; private set; }
    private Vector3[] vertices;

    private GameObject dummy;

    public void ShowBuildingDummy()
    {
        dummy = Instantiate(buildingBody, transform);
        buildingBody.gameObject.SetActive(false);


        MeshRenderer[] mesh = dummy.GetComponents<MeshRenderer>();
        //int numberOfMaterials = mesh.materials.Length;
        //Debug.Log(numberOfMaterials);

        foreach (MeshRenderer meshRenderer in mesh)
        {
            Material[] mats = meshRenderer.sharedMaterials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = greenMat;
            }
            meshRenderer.sharedMaterials = mats;
        }
    }

    public void RemoveBuildingDummy()
    {
        buildingBody.gameObject.SetActive(true);
        if (dummy != null)
        {
            Destroy(dummy);
        }
    }


    private void GetColliderVertexPostionLocal()
    {
        BoxCollider b = gameObject.GetComponent<BoxCollider>();
        vertices = new Vector3[4];
        vertices[0] = b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f;
        vertices[1] = b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f;
        vertices[2] = b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f;
        vertices[3] = b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f;
    }

    private void CalculateSizeInCells()
    {
        Vector3Int[] _vertices = new Vector3Int[this.vertices.Length];
        for (int i = 0; i < _vertices.Length; i++)
        {
            var worldPos = transform.TransformPoint(vertices[i]);
            vertices[i] = BuildingSystem.current.gridLayout.WorldToCell(worldPos);

        }

        Size = new Vector3Int((int)Mathf.Abs((vertices[0] = vertices[1]).x), (int)Mathf.Abs((vertices[0] - vertices[3]).y), 1);

        
    }

    public Vector3 GetStartPosition()
    {
        return transform.TransformPoint(vertices[0]);
    }

    private void Start()
    {
        GetColliderVertexPostionLocal();
        CalculateSizeInCells();
    }

    public void Rotate()
    {
        transform.Rotate(new Vector3(0, 90, 0));
        Size = new Vector3Int(Size.y, Size.x, 1);

        Vector3[] _vertices = new Vector3[this.vertices.Length];

        for (int i = 0; i < _vertices.Length; i++)
        {
            _vertices[i] = vertices[(i + 1) % vertices.Length]; 
        }

        vertices = _vertices;
    }


    public virtual void Place()
    {
        ObjectDrag drag = gameObject.GetComponent<ObjectDrag>();
        Destroy(drag);

        RemoveBuildingDummy();

        Placed = true;
    }
}
