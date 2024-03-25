using UnityEngine;

[CreateAssetMenu(fileName = "BuildingName", menuName = "Building")]
public class BuildingSO : ScriptableObject
{
    public HouseVariations houseVariation;
    public EnviromentType enviromentType;
    public float constructionTimer;
    public GameObject[] constructionPhases;
}
