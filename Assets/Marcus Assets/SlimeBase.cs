using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBase : MonoBehaviour {

    public int m_MaxHealth; // max health
    public int m_MaxAttack; // max attack

    private int m_Health; // health
    private int m_Attack; // attack

    enum States
    {
        Wait,
        Walk,
        Attack,
        Defend,
    }

    private States currState;
    private States nextState;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
