using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class Flashbang : GrabbableObject
{

    [SerializeField]
    private FlashbangRing m_FlashbangRing;

    [SerializeField]
    private GameObject m_FlashbangBody;

    [SerializeField]
    private GameObject m_FlashParticle;

    [SerializeField]
    private GameObject m_ConfuseParticle;

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

        Transform playerHead = Camera.main.transform;

        Debug.DrawRay(transform.position, playerHead.position - transform.position, Color.yellow, 5);

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, playerHead.position - transform.position, out hitInfo, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("UI"))) && hitInfo.transform.tag == playerHead.tag)
        {
            Vector3 playerHeadDir = transform.position - playerHead.position;
            // playerHeadDir.y = transform.position.y; // Flatten the Y axis

            Vector3 playerDir = playerHead.forward;
            // playerDir.y = transform.position.y;

            // Player flashed
            float playerAngle = Vector3.Angle(playerHeadDir, playerDir);
            StartCoroutine(FlashRoutine(playerAngle));
        }

        // Enemy flashed
        var enemies = FindObjectsOfType<SlimeBase>();

        foreach (var enemy in enemies)
        {
            if (enemy && (enemy.transform.position - transform.position).magnitude < 30)
            {
                enemy.ApplyConfusion(10, m_ConfuseParticle);
                Debug.Log("Enemyhit: " + enemy.transform.name);
            }
        }

        AudioManager.Instance.Play3D("flashbangboom", transform.position, AudioManager.AudioType.Additive);
        Instantiate(m_FlashParticle, transform.position, Quaternion.identity);

        // Hide the collider temporarily
        m_FlashbangBody.SetActive(false);
        GetComponent<Collider>().enabled = false;
    }

    IEnumerator FlashRoutine(float angleBetweenPlayer)
    {
        const float thresholdMid = 90;
        const float thresholdSmall = 140;

        var ppBehaviour = Camera.main.GetComponent<PostProcessingBehaviour>();
        var bloomSettings = ppBehaviour.profile.bloom.settings;

        float intensityTarget = 0.5f, thresholdTarget = 1.1f, flashPeakTime = 0.1f, flashDuration = 2;

        if (angleBetweenPlayer >= thresholdSmall) // Small flash
        {
            intensityTarget = 3;
            thresholdTarget = 0;
            flashDuration = 1;
        }
        else if (angleBetweenPlayer >= thresholdMid) // Mid flash
        {
            intensityTarget = 7;
            thresholdTarget = 0;
        }
        else // Max flashthresholdMax
        {
            intensityTarget = 15;
            thresholdTarget = 0;
        }

        Debug.Log(angleBetweenPlayer + " degrees and Intensity: " + intensityTarget);

        float intensityDelta = (intensityTarget - bloomSettings.bloom.intensity) / flashPeakTime;
        float thresholdDelta = (bloomSettings.bloom.threshold - thresholdTarget) / flashPeakTime;

        float currentFlashDuration = 0;

        while (currentFlashDuration < flashPeakTime)
        {
            currentFlashDuration += Time.deltaTime;

            bool intenseDone = true;
            bool thresholdDone = true;

            if (bloomSettings.bloom.intensity < intensityTarget)
            {
                bloomSettings.bloom.intensity += intensityDelta * Time.deltaTime;
                intenseDone = false;
            }

            if (bloomSettings.bloom.threshold > thresholdTarget)
            {
                bloomSettings.bloom.threshold -= thresholdDelta * Time.deltaTime;
                thresholdDone = false;
            }

            ppBehaviour.profile.bloom.settings = bloomSettings;

            if (intenseDone && thresholdDone)
            {
                break;
            }
            else
            {
                yield return null;
            }
        }

        yield return new WaitForSeconds(flashDuration);

        StartCoroutine(UnFlashRoutine(ppBehaviour, 2));
    }

    IEnumerator UnFlashRoutine(PostProcessingBehaviour ppBehaviour, float unflashDuration)
    {
        const float intensityTarget = 0.5f, thresholdTarget = 1.0f;

        var bloomSettings = ppBehaviour.profile.bloom.settings;

        float intensityDelta = (bloomSettings.bloom.intensity - intensityTarget) / unflashDuration;
        float thresholdDelta = (thresholdTarget - bloomSettings.bloom.threshold) / unflashDuration;

        while (true)
        {
            bool intenseDone = true;
            bool thresholdDone = true;

            if (bloomSettings.bloom.intensity > intensityTarget)
            {
                bloomSettings.bloom.intensity -= intensityDelta * Time.deltaTime;
                intenseDone = false;
            }

            if (bloomSettings.bloom.threshold < thresholdTarget)
            {
                bloomSettings.bloom.threshold += thresholdDelta * Time.deltaTime;
                thresholdDone = false;
            }

            ppBehaviour.profile.bloom.settings = bloomSettings;

            if (intenseDone && thresholdDone)
            {
                Destroy(gameObject);
                break;
            }
            else
            {
                yield return null;
            }
        }
    }

    public KeyCode key;

    private void Update()
    {
        if (Input.GetKeyUp(key))
        {
            StartFuse();
        }
    }

}