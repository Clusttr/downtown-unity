using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/BuildingData")]
public class BuildingData : ScriptableObject
{
    public string Name;
    public GameObject prefab;
    public float contructionTime = 60;
    public Vector2Int CellSize;

    public GameObject[] constructionPhases;

}
