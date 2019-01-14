using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpongeCollision : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Sponge")
            return;

        if(collision.relativeVelocity.magnitude > 0)
        {
            Color tempColor = GetComponentInParent<Image>().color;
            tempColor.a -= 0.4f;

            GetComponentInParent<Image>().color = tempColor;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag != "Sponge")
        //    return;

        Destroy(transform.parent.gameObject);
    }
}
