using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour {

    public static PlayerDamage instance;

    [HideInInspector]
    public int m_DamageLevel;
    
	// Use this for initialization
	void Start () {
        if (instance == null)
            instance = GetComponent<PlayerDamage>();

        m_DamageLevel = 0;
	}
	
}
