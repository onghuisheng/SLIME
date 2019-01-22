using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryHorn : GrabbableObject
{

    private bool m_Grabbed = false;
    private float m_HoldDuration = 0;
    private bool m_IsUsed = false;

    public override void OnGrab(MoveController currentController)
    {
        // base.OnGrab(currentController);
        transform.GetComponent<Collider>().isTrigger = true;
        m_Grabbed = true;
    }

    public override void OnGrabReleased(MoveController currentController)
    {
        // base.OnGrabReleased(currentController);
        transform.GetComponent<Collider>().isTrigger = false;
        m_Grabbed = false;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {
            m_HoldDuration += Time.fixedDeltaTime;

            AudioManager.Instance.Play3D("warhorn", transform.position, AudioManager.AudioType.Additive, 0);
            AudioManager.Instance.Play3D("npc_artillery", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { randomPitchRange = 0 }, 3.5f, () =>
            {
                AudioManager.Instance.Play3D("arrowlit", transform.position, AudioManager.AudioType.Additive, 0.5f);

                AudioManager.Instance.Play3D("npc_fire", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { randomPitchRange = 0 }, 1.25f, () =>
                {
                    foreach (var firer in FindObjectsOfType<VolleyFirer>())
                    {
                        firer.FireArtillery();
                    }
                });
            });
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!m_IsUsed && m_Grabbed && other.tag == "MainCamera")
        {
            Debug.Log("Cat");

            m_HoldDuration += Time.fixedDeltaTime;

            if (m_HoldDuration >= 3)
            {
                m_IsUsed = true;

                AudioManager.Instance.Play3D("warhorn", transform.position, AudioManager.AudioType.Additive, 0);
                AudioManager.Instance.Play3D("npc_artillery", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { randomPitchRange = 0 }, 3.5f, () =>
                 {
                     AudioManager.Instance.Play3D("arrowlit", transform.position, AudioManager.AudioType.Additive, 0.5f);

                     AudioManager.Instance.Play3D("npc_fire", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { randomPitchRange = 0 }, 1.25f, () =>
                     {
                         foreach (var firer in FindObjectsOfType<VolleyFirer>())
                         {
                             firer.FireArtillery();
                         }
                     });
                 });
            }
        }
    }

}
