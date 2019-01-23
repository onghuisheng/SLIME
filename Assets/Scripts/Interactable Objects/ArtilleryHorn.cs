using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryHorn : MonoBehaviour
{

    private float m_HoldDuration = 0;
    private bool m_IsUsed = false;
    
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {
            UseHorn();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!m_IsUsed && other.tag == "MainCamera")
        {
            m_HoldDuration += Time.fixedDeltaTime;

            if (m_HoldDuration >= 1)
            {
                UseHorn();
            }
        }
    }

    public void UseHorn()
    {
        if (m_IsUsed)
        {
            AudioManager.Instance.Play3D("smallhorn", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { pitchOverride = 3 });
            return;
        }
        else
        {
            AudioManager.Instance.Play3D("smallhorn", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { pitchOverride = 1 });
            AudioManager.Instance.Play3D("npc_artillery", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { pitchOverride = 1 }, 3.5f, () =>
             {
                 AudioManager.Instance.Play3D("arrowlit", transform.position, AudioManager.AudioType.Additive, 0.5f);

                 AudioManager.Instance.Play3D("npc_fire", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { pitchOverride = 1 }, 1.25f, () =>
                 {
                     foreach (var firer in FindObjectsOfType<VolleyFirer>())
                     {
                         firer.FireArtillery();
                     }
                 });
             });

            m_IsUsed = true;
        }
    }

}
