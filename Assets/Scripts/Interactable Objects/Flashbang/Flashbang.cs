using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashbang : GrabbableObject
{

    [SerializeField]
    private FlashbangRing m_FlashbangRing;

    [SerializeField]
    private GameObject m_FlashParticle;

    public override void OnGrab(MoveController currentController)
    {
        base.OnGrab(currentController);
        m_FlashbangRing.GetComponent<Collider>().enabled = true;
    }

    public override void OnGrabReleased(MoveController currentController)
    {
        base.OnGrabReleased(currentController);
        m_FlashbangRing.GetComponent<Collider>().enabled = false;
    }

    public void StartFuse()
    {
        GameObject audio = AudioManager.Instance.Play3D("flashbangsizzle", transform.position, AudioManager.AudioType.Additive);
        StartCoroutine(FuseRoutine(audio.GetComponent<AudioSource>().clip.length * 0.95f));
    }

    private IEnumerator FuseRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        AudioManager.Instance.Play3D("flashbangboom", transform.position, AudioManager.AudioType.Additive);
        Instantiate(m_FlashParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
}