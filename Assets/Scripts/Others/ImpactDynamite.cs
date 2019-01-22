using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactDynamite : MonoBehaviour
{

    [SerializeField]
    private Dynamite m_Dynamite;

    private void OnCollisionEnter(Collision collision)
    {
        m_Dynamite.PlayImpactExplosion();
    }

}
