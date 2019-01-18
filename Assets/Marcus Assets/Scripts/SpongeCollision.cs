using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpongeCollision : MonoBehaviour
{
    public Text m_Text;

    private Vector3 m_PrevPos;

    public MoveController m_Controller;

    public int m_DamageLevel;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(m_Controller.GetButtonDown(MoveControllerButton.Square))
        //{
        //    transform.parent.GetComponent<RectTransform>().localPosition = new Vector3(transform.parent.GetComponent<RectTransform>().localPosition.x,
        //        transform.parent.GetComponent<RectTransform>().localPosition.y + 5, transform.parent.GetComponent<RectTransform>().localPosition.z);
        //}

        //if (m_Controller.GetButtonDown(MoveControllerButton.X))
        //{
        //    transform.parent.GetComponent<RectTransform>().localPosition = new Vector3(transform.parent.GetComponent<RectTransform>().localPosition.x,
        //         transform.parent.GetComponent<RectTransform>().localPosition.y - 5, transform.parent.GetComponent<RectTransform>().localPosition.z);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Sponge")
            return;

        Sponge sponge = other.GetComponent<Sponge>();
        //play foam particles function, also, if can be improved, please change! :D
        sponge.PlayFoamParticles(true);

        m_PrevPos = other.transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Sponge")
            return;

        if (m_DamageLevel != PlayerDamage.instance.m_DamageLevel)
            return;

        if ((m_PrevPos - other.transform.position).magnitude < 0.1f)
            return;
            
        m_PrevPos = other.transform.position;

        Debug.Log("INNNNNNNNNNN");

        

        Color tempColor = GetComponentInParent<Image>().color;
        tempColor.a -= 4.0f * Time.fixedDeltaTime;

        GetComponentInParent<Image>().color = tempColor;

        if (GetComponentInParent<Image>().color.a <= 0)
        {
            Destroy(other.transform.parent.gameObject);
            Destroy(transform.parent.gameObject);

            PlayerDamage.instance.m_DamageLevel -= 1;

            Debug.Log("DAMAGE: " + PlayerDamage.instance.m_DamageLevel);

        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Sponge")
            return;

        Sponge sponge = other.GetComponent<Sponge>();
        //play foam particles function, also, if can be improved, please change! :D
        sponge.PlayFoamParticles(false);
    }
}