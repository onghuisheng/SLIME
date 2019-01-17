using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDeath : MonoBehaviour
{

    public SlimeBase m_SlimeBase;
    public GameObject Slime;
    public GameObject m_Canvas;
    
    public GameObject m_SlimeOnScreen1;
    public GameObject m_SlimeOnScreen2;
    public GameObject m_SlimeOnScreen3;

    public bool m_Despawn;

    // Use this for initialization
    void Start()
    {
        m_Despawn = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RemoveFromScene()
    {
        if (m_SlimeBase.m_DeathParticles != null)
        {
            AudioManager.Instance.Play3D("basicdeath", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { randomPitchRange = 0.4f, volume = .5f });

            AudioManager.Instance.Play3D("slimesplatter", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { volume = .2f }, .1f);

            GameObject temp = Instantiate(m_SlimeBase.m_DeathParticles, Slime.transform.position, m_SlimeBase.m_DeathParticles.gameObject.transform.rotation);
            Destroy(temp, 3);

            if(m_Despawn)
                SetScreen();
        }

        Destroy(Slime);
    }

    public void SetScreen()
    {
        if (PlayerDamage.instance.m_DamageLevel == 0)
        {
            Instantiate(m_SlimeOnScreen1, m_Canvas.transform);
            PlayerDamage.instance.m_DamageLevel++;
        }
        else if (PlayerDamage.instance.m_DamageLevel == 1)
        {
            Instantiate(m_SlimeOnScreen2, m_Canvas.transform);
            PlayerDamage.instance.m_DamageLevel++;
        }
        else if (PlayerDamage.instance.m_DamageLevel == 2)
        {
            Instantiate(m_SlimeOnScreen3, m_Canvas.transform);
            PlayerDamage.instance.m_DamageLevel++;
        }
    }
}
