using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemDeath : MonoBehaviour
{

    //PLEASE CHANGE/ ADD THIS TO /DELETE TO MAKE IT BETTER HHHHHH :,O
    //Thanks marrot/hs :,D

    public GameObject m_SlimeParticles; //to be played in event
    public GameObject SlimeBody; //remove this gameobject (in this case slime body)


    public void RemoveSlimeBody()
    {
        if (m_SlimeParticles != null) //if there's slime particles..
        {
            AudioManager.Instance.Play3D("golemdeath", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { randomPitchRange = 0.4f, volume = .5f });

            AudioManager.Instance.Play3D("slimesplatter", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { volume = .2f }, .1f);

            //create temp game obj & play Slime particles
            GameObject temp = Instantiate(m_SlimeParticles, SlimeBody.transform.position, m_SlimeParticles.gameObject.transform.rotation);
            temp.transform.Translate(0, 0.5f, 0);

            //Destroy temp game obj, along with its particles :> (after 3 seconds)
            Destroy(temp, 3);

        }
        //Destroy slime body
        Destroy(SlimeBody);
    }

    //golemcrumble
    public void PlayCrumbleAudio()
    {
        AudioManager.Instance.Play3D("golemcrumble", transform.position, AudioManager.AudioType.Additive);
    }
    
}
