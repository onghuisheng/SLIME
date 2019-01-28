using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilBottle : GrabbableObject, IStorable, IShootable
{

    public GameObject m_OilPuddle;
    public GameObject m_OilParticles;


    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.relativeVelocity.magnitude > 5)
        {
            BreakBottle();
        }
    }

    public void BreakBottle()
    {
        GameObject temp = Instantiate(m_OilPuddle, transform.position + new Vector3(0, 0.1f, 0), m_OilPuddle.transform.rotation);
        GameObject tempParticles = Instantiate(m_OilParticles, transform.position /*+ new Vector3(0, 0.1f, 0)*/, m_OilParticles.transform.rotation);

        //im not too sure where to put the sound of the oil bottle breaking, but i can assume the bottle game object disapperars at Destroy(this.gameObject)?
        //i probably am very wrong, and if so, please change this part :>
        AudioManager.Instance.Play3D("bottlebreak", transform.position, AudioManager.AudioType.Additive);


        Destroy(this.gameObject);
        //Destroy(temp, 20.0f);
        Destroy(tempParticles, 3.0f);
    }

    public void OnStore(BeltSlot slot)
    {
    }

    public void OnUnStore(BeltSlot slot)
    {
    }

    public void OnShot(ArrowBase arrow)
    {
        BreakBottle();
    }

}
