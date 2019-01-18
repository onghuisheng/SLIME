using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sponge : GrabbableObject, IStorable {

    public GameObject m_Collider;

    public ParticleSystem m_FoamParticles;

    bool isFoaming = false;


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

    public void PlayFoamParticles(bool toggle)
    {
        isFoaming = toggle;

        if (toggle)
        {
            //play particles with it's children
            m_FoamParticles.Play(true);
        }
        else
            m_FoamParticles.Stop();
        
    }
}
