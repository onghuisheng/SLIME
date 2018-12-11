using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour {

    public GameObject m_SlimeManager;

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);
        m_SlimeManager.GetComponent<SlimeManager>().Remove();

    }
}
