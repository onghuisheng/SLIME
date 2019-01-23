﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : MonoBehaviour
{
    //HI HUI SHENG- PLS CHANGE THIS TO BE BETTER IF YOU THINK IT'S NOT OPTIMAL :,DDDD th e n k  s

    //TODO:
    //Move spark down the fuse (Animation)
    //When spark reaches base of fuse: (End of animation)
    //play explosion
    //if explosion range hits enemy kill them

    public Animator DynamiteAnimator;
    public GameObject m_ExplosionParticle;
    public GameObject m_DynamiteObject;

    //OnTriggerEnter with torch flame
    //DynamiteAnimator.SetBool("isLighted", true);

    private string m_ExplodeAlias = "dynexp";

    public void PlayExplosion()
    {
        //if there is slime particles
        if (m_ExplosionParticle != null)
        {
            //explosion audio
            var aData = new AudioSourceData3D();

            if (m_ExplodeAlias != "dynexp")
            {
                aData.minDistance = 5;
                aData.maxDistance = 50;
            }

            AudioManager.Instance.Play3D(m_ExplodeAlias, transform.position, AudioManager.AudioType.Additive, aData);

            //Create temp GO and instantiate in it
            GameObject temp = Instantiate(m_ExplosionParticle, m_DynamiteObject.transform.position, m_ExplosionParticle.gameObject.transform.rotation);
            Destroy(temp, 3); //Destroy after 3 secs
        }
        Destroy(m_DynamiteObject); //get rid of all of them
    }

    public void PlayImpactExplosion()
    {
        m_ExplodeAlias = "dynexp2";
        PlayExplosion();
    }

    public void DynamiteLit()
    {
        DynamiteAnimator.SetBool("isLighted", true);
        Invoke("PlayExplosion", 3); // How long before the dynamite explodes, use invoke because not using event
    }

}
