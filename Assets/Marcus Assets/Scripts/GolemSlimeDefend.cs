using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemSlimeDefend : MonoBehaviour {

    public Animator anim;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<SlimeBase>().GetHealth() < GetComponent<SlimeBase>().m_MaxHealth && !anim.GetBool("IsDefending"))
        {
            anim.SetBool("IsDefending", true);
        }
    }
}
