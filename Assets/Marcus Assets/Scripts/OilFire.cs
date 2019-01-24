using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilFire : MonoBehaviour, IShootable
{
    public ParticleSystem m_Fire;
    private bool m_onFire;

    public void OnShot(ArrowBase arrow)
    {
        m_Fire.Play(true);
        m_onFire = true;
    }

    public void OnTriggerStay(Collider other)
    {
        if (!m_onFire)
            return;

        DamageOverTime(other.gameObject, 3);
    }

    IEnumerator DamageOverTime(GameObject enemy, float delayTime)
    {
        if (enemy != null)
        {
            enemy.GetComponent<EnemyHit>().Damage = 1;
            enemy.GetComponent<EnemyHit>().OnShot(null);

            yield return new WaitForSeconds(delayTime);
        }
    }
}
