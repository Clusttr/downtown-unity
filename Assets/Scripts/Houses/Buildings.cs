using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum HouseVariations
{
    None = 0,
    VarOne = 1,
    VarTwo = 2,
    VarThree = 3,
}

public enum ConstructionPhases
{
    None = 0,
    Constructing = 1,
    FullyBuilt = 2,
}

public class Buildings : MonoBehaviour
{
    public int currentX;
    public int currentY;
    public HouseVariations type;
    public ConstructionPhases constructionPhases;

    private Vector3 desiredPosition;
    private Vector3 desiredScale;
    private Vector3 desiredRotation;

    private void Update()
    {
        transform.Rotate(new Vector3 (0, 90, 0));
    }

    public virtual void SetRotation(Vector3 rotation, bool force = false)
    {
        desiredRotation = rotation;

        if (force)
            transform.rotation = Quaternion.Euler (desiredRotation);
    }
    public virtual void ConstructionPhase()
    {

    }
}
