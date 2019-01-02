using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemSlimeDefend : MonoBehaviour {

    public Animator anim;
    private float m_Timer;

    // Use this for initialization
    void Start()
    {
        m_Timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetBool("IsDefending") == true)
        {
            m_Timer += Time.deltaTime;
        }

        if(m_Timer > 5.0f)
        {
            anim.SetBool("IsDefending", false);
            m_Timer = 0.0f;
        }
    }

    public void ChangeToDefend()
    {
        if (GetComponent<SlimeBase>().GetHealth() < GetComponent<SlimeBase>().m_MaxHealth && GetComponent<SlimeBase>().GetHealth() > 0 && !anim.GetBool("IsDefending") && !anim.GetBool("IsDead") 
            && !anim.GetBool("IsAttack"))
        {
            anim.SetBool("IsDefending", true);
        }
    }
}
