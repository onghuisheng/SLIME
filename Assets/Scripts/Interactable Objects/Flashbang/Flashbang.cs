using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class Flashbang : GrabbableObject
{

    [SerializeField]
    private FlashbangRing m_FlashbangRing;

    [SerializeField]
    private GameObject m_FlashParticle;

    bool m_IsLighted = false;

    public override void OnGrab(MoveController currentController)
    {
        base.OnGrab(currentController);
        m_FlashbangRing.GetComponent<Collider>().enabled = true;
    }

    public override void OnGrabReleased(MoveController currentController)
    {
        base.OnGrabReleased(currentController);

        if (m_IsLighted == false)
            m_FlashbangRing.GetComponent<Collider>().enabled = false;
    }

    public void StartFuse()
    {
        GameObject audio = AudioManager.Instance.Play3D("flashbangsizzle", transform.position, AudioManager.AudioType.Additive);
        StartCoroutine(FuseRoutine(audio.GetComponent<AudioSource>().clip.length * 0.95f));
        m_IsLighted = true;
    }

    private IEnumerator FuseRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Flashbang booms
        //var cat = GetComponent<PostProcessingBehaviour>();
        //var bloomer = cat.profile.bloom.settings;
        //bloomer.bloom.threshold = Mathf.PingPong(Time.time, 1.1f);
        //bloomer.bloom.intensity = Mathf.PingPong(Time.time, 3);
        //cat.profile.bloom.settings = bloomer;

        Debug.DrawRay(transform.position, Camera.main.transform.position - transform.position, Color.yellow, 5);

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Camera.main.transform.position - transform.position, out hitInfo) && hitInfo.transform.tag == "MainCamera")
        {
            // TODO: Check angle and flash the player
            Debug.Log("Hit");
        }

        AudioManager.Instance.Play3D("flashbangboom", transform.position, AudioManager.AudioType.Additive);
        Instantiate(m_FlashParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            StartFuse();
        }
    }

}