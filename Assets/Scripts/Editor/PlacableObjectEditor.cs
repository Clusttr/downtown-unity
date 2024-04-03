using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlacableObject))]
public class PlacableObjectEditor : Editor
{

    private PlacableObject placableObject;

    private void OnEnable()
    {
        placableObject = (PlacableObject)target;
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Test Bounds"))
        {
            placableObject.TestBounds();
        }
    }
}
