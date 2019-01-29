using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Runes : GrabbableObject {

    public ParticleSystem m_HoldParticles;
    public ParticleSystem m_TeleportParticles;
    public float m_WaitBeforeTeleport;
    public string m_LevelToLoad;

    private bool isHolding = false;

    //TO-DO:
    //if holding play hold particles
    //start timer (m_WaitBeforeTeleport)
    //when timer reaches 0, play teleport particles, maybe screenshake & bloom too
    //after teleport particles end, change scene

    // Use this for initialization
    void Start () {
        m_HoldParticles.Stop();
        m_TeleportParticles.Stop();
    }

    public override void OnGrab(MoveController currentController) //run once when picked up
    {
        base.OnGrab(currentController);
        m_HoldParticles.Play();
        isHolding = true;
    }

    public override void OnGrabReleased(MoveController currentController)
    {
        base.OnGrabReleased(currentController);
        m_HoldParticles.Stop();
        m_TeleportParticles.Stop();
        isHolding = false;
    }

    // Update is called once per frame
    void Update () {
        if(isHolding)
        {
            m_WaitBeforeTeleport -= Time.deltaTime;
            if (m_WaitBeforeTeleport <= 0)
            {
                m_TeleportParticles.Play(true);
                //insert fade out here
                //insert scene change here, after finish particles & fade out
                SceneManager.LoadScene(m_LevelToLoad);
            }
        }
    }

}
