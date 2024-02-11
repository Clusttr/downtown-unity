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
public class Buildings : MonoBehaviour
{
    public int currentX;
    public int currentY;
    public HouseVariations type;

    private Vector3 desiredPosition;
    private Vector3 desiredScale;
}
