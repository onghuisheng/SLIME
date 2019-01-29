using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryHorn : MonoBehaviour
{

    private float m_HoldDuration = 0;
    private bool m_IsUsed = false;
    private bool m_IsDisabled = false;

    private Bonfire m_Bonfire;

    private void Start()
    {
        m_Bonfire = FindObjectOfType<Bonfire>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!m_IsDisabled && other.tag == "MainCamera")
        {
            m_HoldDuration += Time.fixedDeltaTime;

            if (m_HoldDuration >= 1)
            {
                UseHorn();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            m_IsDisabled = false;
            m_HoldDuration = 0;
        }
    }

    public void UseHorn()
    {
        m_IsDisabled = true;

        if (m_IsUsed)
        {
            AudioManager.Instance.Play3D("smallhorn", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { pitchOverride = 3 });
            return;
        }
        else
        {
            AudioManager.Instance.Play3D("smallhorn", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { pitchOverride = (m_Bonfire.isFirstShot) ? 3 : 1 });

            if (m_Bonfire.isFirstShot == false)
            {
                CommanderSpeaker.Instance.PlaySpeaker("npc_artillery", AudioManager.AudioType.Additive, 3.5f, () =>
                {
                    AudioManager.Instance.Play3D("arrowlit", transform.position, AudioManager.AudioType.Additive, 0.5f);

                    CommanderSpeaker.Instance.PlaySpeaker("npc_fire", AudioManager.AudioType.Additive, 1.25f, () =>
                    {
                        foreach (var firer in FindObjectsOfType<VolleyFirer>())
                        {
                            firer.FireArtillery();
                        }

                        m_HoldDuration = 0;
                    });
                });

                m_IsUsed = true;
            }
        }
    }

}
