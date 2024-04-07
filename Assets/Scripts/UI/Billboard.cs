using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public TMP_Text amountText;
    Camera cameraToLookAt;

    private void Start()
    {
        cameraToLookAt = Camera.main;
        amountText.transform.localScale = new Vector3(-1, 1, 1);
    }

    void Update()
    {
        Vector3 v = cameraToLookAt.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(cameraToLookAt.transform.position + v);
    }


    public void UpdateAmount(int amount)
    {
        amountText.text = $"${amount}";
    }
}
