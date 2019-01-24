using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sponge : GrabbableObject, IStorable {

    public GameObject m_Collider;

    public ParticleSystem m_FoamParticles;

    void Start()
    {
        m_FoamParticles.Stop();
    }

    public override void OnGrab(MoveController currentController)
    {
        base.OnGrab(currentController);
        m_Collider.SetActive(true);
    }

    public override void OnGrabReleased(MoveController currentController)
    {
        base.OnGrabReleased(currentController);
        m_Collider.SetActive(false);
    }

    public void OnStore(BeltSlot slot)
    {
    }

    public void OnUnStore(BeltSlot slot)
    {
    }

    public void PlayFoamParticles()
    {
        m_FoamParticles.Play(true);
    }

    public void StopFoamParticles()
    {
        m_FoamParticles.Stop(true);
    }
}
