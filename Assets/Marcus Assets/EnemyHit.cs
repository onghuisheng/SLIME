using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour, IShootable
{
    
    public virtual void OnShot(ArrowBase arrow)
    {
        GetComponentInParent<SlimeBase>().toDespawn = true;
        SlimeManager.instance.GetComponent<SlimeManager>().Remove();
    }
}
