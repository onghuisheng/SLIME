using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilFire : MonoBehaviour, IShootable
{
    public ParticleSystem m_Fire;
    private bool m_onFire;

    private void Start()
    {
        m_onFire = false;
    }

    public void OnShot(ArrowBase arrow)
    {
        if (arrow.arrowType == ArrowBase.ArrowType.Flame && m_onFire == false)
        {
            m_Fire.Play(true);
            m_onFire = true;
        }
        Destroy(arrow.gameObject);
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
