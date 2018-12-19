using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemSlimeDefend : MonoBehaviour {

    public Animator anim;

    // Use this for initialization
    void Start()
    {
        anim.SetBool("IsDefending", true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeToDefend()
    {
        if (GetComponent<SlimeBase>().GetHealth() < GetComponent<SlimeBase>().m_MaxHealth && GetComponent<SlimeBase>().GetHealth() > 0 && !anim.GetBool("IsDefending") && !anim.GetBool("IsDead"))
        {
            anim.SetBool("IsDefending", true);
        }
    }
}
