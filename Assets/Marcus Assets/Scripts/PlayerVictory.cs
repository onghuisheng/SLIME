using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVictory : MonoBehaviour
{

    public GameObject m_FireWorks;
    public bool m_Spawn;

    // Use this for initialization
    void Start()
    {
        m_Spawn = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (SlimeManager.instance.m_GameFinish == true && m_Spawn == false)
        {
            AudioManager.Instance.Play3D("npc_victory", transform.position, AudioManager.AudioType.Additive);

            m_Spawn = true;
            Instantiate(m_FireWorks, m_FireWorks.transform.position, m_FireWorks.transform.rotation);

        }

    }
}
