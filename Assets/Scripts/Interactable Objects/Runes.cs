using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runes : MonoBehaviour {

    public ParticleSystem m_HoldParticles;
    public ParticleSystem m_TeleportParticles;

    public float m_WaitBeforeTeleport;

	// Use this for initialization
	void Start () {
        m_HoldParticles.Stop();
	}
	
	// Update is called once per frame
	void Update () {

	}

    //if holding play hold particles
    //start timer (m_WaitBeforeTeleport)
    //when timer reaches 0, play teleport particles, maybe screenshake & bloom too
    //after teleport particles end, change scene

}
