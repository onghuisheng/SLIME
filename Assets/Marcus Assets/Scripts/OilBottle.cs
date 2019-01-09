using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilBottle : GrabbableObject {

    public GameObject m_OilPuddle;

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
            GameObject temp = Instantiate(m_OilPuddle, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);

            Destroy(this.gameObject);
            Destroy(temp, 20.0f);
        }
    }
}
