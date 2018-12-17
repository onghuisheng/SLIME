using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour, IShootable
{
    
    [SerializeField]
    private SlimeBase m_SlimeBase;

    public virtual void OnShot(ArrowBase arrow)
    {
        m_SlimeBase.toDespawn = true;
        SlimeManager.instance.GetComponent<SlimeManager>().Remove();
    }
}
