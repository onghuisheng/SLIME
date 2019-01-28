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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {

            AudioManager.Instance.Play3D("smallhorn", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { pitchOverride = 5 });
            // UseHorn();
        }
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
                AudioManager.Instance.Play3D("npc_artillery", CommanderSpeaker.Instance.transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { pitchOverride = 1, volume = 0.5f }, 3.5f, () =>
                {
                    AudioManager.Instance.Play3D("arrowlit", transform.position, AudioManager.AudioType.Additive, 0.5f);

                    AudioManager.Instance.Play3D("npc_fire", CommanderSpeaker.Instance.transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { pitchOverride = 1, volume = 0.5f }, 1.25f, () =>
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
