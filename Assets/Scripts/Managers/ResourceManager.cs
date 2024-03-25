using System.Linq;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public BuildingSO GetBuildingData(HouseVariations houseType)
    {
        BuildingSO building = Resources.LoadAll<BuildingSO>("Scriptables/Buildings").Where(e => e.houseVariation == houseType).FirstOrDefault();

        return building;
    }
}
