using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OilCollision : MonoBehaviour, IShootable
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
            AudioManager.Instance.Play3D("bonfirelit", transform.position, AudioManager.AudioType.Additive);
            m_Fire.Play(true);
            m_onFire = true;
        }
        Destroy(arrow.gameObject);
    }

    private void InvokeFire()
    {
        if (m_onFire)
            return;

        m_Fire.Play(true);
        m_onFire = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
            return;

        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<NavMeshAgent>().speed /= 10;
        }

        if(other.transform.GetComponent<OilStep>() != null)
        {
            other.transform.GetComponent<OilStep>().PlayOilStep(other.transform.position);
        }

        if(m_onFire)
        {
            StartCoroutine(DamageOverTime(other.gameObject, 1));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == null)
            return;

        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<NavMeshAgent>().speed *= 10;

            if (m_onFire)
            {
                StopCoroutine(DamageOverTime(other.gameObject, 1));
            }
        }

        if (other.transform.GetComponent<OilStep>() != null)
        {
            other.transform.GetComponent<OilStep>().PlayOilDrip();
        }
    }

    IEnumerator DamageOverTime(GameObject enemy, float delayTime)
    {
        if (enemy != null)
        {
            while (enemy != null)
            {
                if (enemy.GetComponentInChildren<EnemyHit>() == null)
                {
                    break;
                }

                enemy.GetComponentInChildren<EnemyHit>().Damage = 1;
                enemy.GetComponentInChildren<EnemyHit>().OnShot(null);

                yield return new WaitForSeconds(delayTime);
            }

            yield return null;
        }
    }
}
