using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider collider)
    {
        EnemyHit enemyhit = collider.GetComponent<EnemyHit>();
        if (enemyhit != null)
        {
            enemyhit.Damage = 9999;

            SlimeBase slimeBase = collider.GetComponent<SlimeBase>();

            if (slimeBase == null || slimeBase.slimeType != SlimeBase.SlimeType.Catapult)
                enemyhit.m_OnAnim = false;

            enemyhit.OnShot(null);

        }


    }
}
