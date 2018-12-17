using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.tag != "Enemy")
                return;
            
            other.GetComponent<SlimeBase>().toDespawn = true;
            SlimeManager.instance.GetComponent<SlimeManager>().Remove();
        }
    }
}
