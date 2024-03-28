using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Splines;
using Unity.Mathematics;
using UnityEngine.Serialization;
using Interpolators = UnityEngine.Splines.Interpolators;
using Quaternion = UnityEngine.Quaternion;

public class Car : MonoBehaviour
{
    [SerializeField]
    SplineContainer m_SplineContainer;

    public SplineContainer Container => m_SplineContainer;

    [SerializeField]
    SplineData<float> m_Speed = new SplineData<float>();

    public SplineData<float> Speed => m_Speed;


    float m_CurrentOffset;
    float m_CurrentSpeed;
    float m_SplineLength;
    Spline m_Spline;


    void Start()
    {
        //m_SplineContainer = GetComponent<SplineContainer>();

        Assert.IsNotNull(m_SplineContainer);

        m_Spline = m_SplineContainer.Spline;
        m_SplineLength = m_Spline.GetLength();
        m_CurrentOffset = 0f;
    }


    private void Update()
    {
        if (m_SplineContainer == null) return;

        m_CurrentOffset = (m_CurrentOffset + m_CurrentSpeed * Time.deltaTime / m_SplineLength) % 1f;

        if (m_Speed.Count > 0)
            m_CurrentSpeed = m_Speed.Evaluate(m_Spline, m_CurrentOffset, PathIndexUnit.Normalized, new Interpolators.LerpFloat());
        else
            m_CurrentSpeed = m_Speed.DefaultValue;

        var posOnSplineLocal = SplineUtility.EvaluatePosition(m_Spline, m_CurrentOffset);
        var direction = SplineUtility.EvaluateTangent(m_Spline, m_CurrentOffset);
        var upSplineDirection = SplineUtility.EvaluateUpVector(m_Spline, m_CurrentOffset);
        var right = math.normalize(math.cross(upSplineDirection, direction));

        if(posOnSplineLocal.x != float.NaN )
            transform.position = m_SplineContainer.transform.TransformPoint(posOnSplineLocal * right);
    }
}
