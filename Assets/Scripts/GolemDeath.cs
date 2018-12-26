using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemDeath : MonoBehaviour {

    //PLEASE CHANGE/ ADD THIS TO /DELETE TO MAKE IT BETTER HHHHHH :,O
    //Thanks marrot :,D

    public GameObject m_SlimeParticles; //to be played in event
    public GameObject SlimeBody; //remove this gameobject (in this case slime body)


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RemoveSlimeBody()
    {
        if(m_SlimeParticles != null) //if there's slime particles..
        {
            //create temp game obj & play Slime particles
           GameObject temp= Instantiate(m_SlimeParticles, SlimeBody.transform.position, m_SlimeParticles.gameObject.transform.rotation);
            //Destroy temp game obj, along with its particles :> (after 3 seconds)
            Destroy(temp, 3);

        }
        //Destroy slime body
        Destroy(SlimeBody);
    }
}
