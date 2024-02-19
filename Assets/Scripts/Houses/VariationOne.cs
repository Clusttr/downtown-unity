using System.Collections;
using UnityEngine;

public class VariationOne : Buildings
{
    
    public override void SetUpData()
    {
        base.SetUpData();

        Debug.Log("Hello from Child class");
    }

}
