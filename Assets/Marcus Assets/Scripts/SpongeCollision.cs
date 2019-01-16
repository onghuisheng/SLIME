using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpongeCollision : MonoBehaviour
{

    public Text m_Text;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (GetComponentInParent<Image>().color.a <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag != "Sponge")
    //        return;

    //    if(collision.relativeVelocity.magnitude > 0)
    //    {
    //        Color tempColor = GetComponentInParent<Image>().color;
    //        tempColor.a -= 0.4f;

    //        GetComponentInParent<Image>().color = tempColor;
    //    }
    //}


    private void OnTriggerEnter(Collider other)
    {
        m_Text.text = other.name;

        if (other.tag != "Sponge")
            return;
        
        Color tempColor = GetComponentInParent<Image>().color;
        tempColor.a -= 0.4f;

        GetComponentInParent<Image>().color = tempColor;

    }
}