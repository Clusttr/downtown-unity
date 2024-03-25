using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/BuildingData")]
public class BuildingData : ScriptableObject
{
    public string Name;
    public GameObject prefab;

    public GameObject[] constructionPhases;
}
