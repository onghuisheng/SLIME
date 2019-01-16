using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpongeCollision : MonoBehaviour
{
    public Text m_Text;

    private Vector3 m_PrevPos;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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


    //private void OnTriggerEnter(Collider other)
    //{
    //}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Sponge")
            return;

        if ((m_PrevPos - other.transform.position).magnitude < 0.1f)
            return;
            
        m_PrevPos = other.transform.position;

        Color tempColor = GetComponentInParent<Image>().color;
        tempColor.a -= 0.2f;

        GetComponentInParent<Image>().color = tempColor;

        if (GetComponentInParent<Image>().color.a <= 0)
        {
            Destroy(other.transform.parent.gameObject);
            Destroy(transform.parent.gameObject);
        }

    }

}