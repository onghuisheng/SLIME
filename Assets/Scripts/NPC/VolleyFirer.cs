using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolleyFirer : MonoBehaviour
{

    public ArrowBase m_ArrowPrefab;

    public Transform[] m_Destinations;

    [Range(1.0f, 10.0f)]
    public float m_Interval = 5.0f;

    [Range(1, 5)]
    public int m_ArrowQuantity = 1;


    private void Start()
    {
        StartCoroutine(FireArrowRoutine());
    }

    IEnumerator FireArrowRoutine()
    {
        yield return new WaitForSeconds(m_Interval);

        foreach (var destination in m_Destinations)
        {
            var arrow = Instantiate(m_ArrowPrefab, transform.position, Quaternion.identity, transform);
            arrow.BuffArrow(ArrowBase.ArrowType.Flame);
            
        }

        FireArrowRoutine();
    }


}
