using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilBottle : GrabbableObject {

    public GameObject m_OilPuddle;
    public GameObject m_OilParticles;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if(collision.relativeVelocity.magnitude > 5)
        {
            GameObject temp = Instantiate(m_OilPuddle, transform.position + new Vector3(0, 0.1f, 0), m_OilPuddle.transform.rotation);
            GameObject tempParticles = Instantiate(m_OilParticles, transform.position + new Vector3(0, 0.1f, 0), m_OilParticles.transform.rotation);

            Destroy(this.gameObject);
            Destroy(temp, 20.0f);
            Destroy(tempParticles, 3.0f);
        }
    }
}
