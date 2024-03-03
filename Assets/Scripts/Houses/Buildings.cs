using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum HouseVariations
{
    None = 0,
    VarOne = 1,
    VarTwo = 2,
    VarThree = 3,
    GreenSoil = 4,
    RedSoil = 5,
    BrownSoil = 6,
    Road1 = 7,
    Road2 = 8,
    Road3 = 9,
    Road4 = 10,
}

public enum EnviromentType
{
    Building,
    Floor
}

public enum ConstructionPhases
{
    None = 0,
    Phase1 = 1,
    Phase2 = 2,
    Phase3 = 3,
}

public abstract class Buildings : MonoBehaviour
{
    public int currentX;
    public int currentY;
    public HouseVariations type;
    public BuildingSO buildingSO;
    public ConstructionPhases constructionPhases;
    public int phaseIndex;

    private void Start()
    {

    }

    public virtual void SetUpData()
    {
        Debug.Log("HELLO");
        buildingSO = ResourceManager.Instance.GetBuildingData(type);
        phaseIndex = 1;
        constructionPhases = ConstructionPhases.Phase1;
        SetUpConstructionState(constructionPhases);
    }

    public void SetUpConstructionState(ConstructionPhases constructionPhases)
    {
        switch (constructionPhases)
        {
            case ConstructionPhases.None:
                break;
            case ConstructionPhases.Phase1:
                StartCoroutine(OnBeginPhase1Construction());
                break;
            case ConstructionPhases.Phase2:
                StartCoroutine(OnBeginPhase2Construction());
                break;
            case ConstructionPhases.Phase3:
                OnFinishConstruction();
                break;
        }
    }

    public IEnumerator OnBeginPhase1Construction()
    {
        float totalTime = buildingSO.constructionTimer / 2;
        float timeElasped = 0f;

        GameObject constructionP1 = Instantiate(buildingSO.constructionPhases[0], transform);

        while (totalTime > timeElasped)
        {
            timeElasped += 1f;
            Debug.Log("Constructing " + timeElasped);

            yield return new WaitForSeconds(1);
        }

        Debug.Log("Finished Counting");

        phaseIndex++;

        constructionPhases = ConstructionPhases.Phase2;

        Destroy(constructionP1);

        SetUpConstructionState(constructionPhases);

        yield return null;
    }

    public IEnumerator OnBeginPhase2Construction()
    {
        float totalTime = buildingSO.constructionTimer / 2;
        float timeElasped = 0f;

        GameObject constructionP2 = Instantiate(buildingSO.constructionPhases[1], transform);

        while (totalTime > timeElasped)
        {
            timeElasped += 1f;
            Debug.Log("Constructing " + timeElasped);

            yield return new WaitForSeconds(1);
        }

        Debug.Log("Finished Counting");

        phaseIndex++;

        constructionPhases = ConstructionPhases.Phase3;

        Destroy(constructionP2);

        SetUpConstructionState(constructionPhases);

        yield return null;
    }

    public void OnFinishConstruction()
    {
        GameObject obj =  Instantiate(buildingSO.constructionPhases[2], transform);

        if (buildingSO.enviromentType == EnviromentType.Floor)
        {
            obj.transform.localPosition = new Vector3(0, -0.001f, 0);
        }
    }


}
