using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour, IShootable
{
    
    [SerializeField]
    private SlimeBase m_SlimeBase;
    public int Damage;

    public virtual void OnShot(ArrowBase arrow)
    {
        m_SlimeBase.DeductHealth(Damage); // deduct 1hp when hit

        if (m_SlimeBase.GetHealth() <= 0) // die when 0 or lesser hp
        {
            m_SlimeBase.toDespawn = true;
            SlimeManager.instance.GetComponent<SlimeManager>().Remove();
        }
    }
}
