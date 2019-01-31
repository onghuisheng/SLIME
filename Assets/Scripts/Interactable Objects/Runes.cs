using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

public class Runes : GrabbableObject
{

    public ParticleSystem m_HoldParticles;
    public ParticleSystem m_TeleportParticles;
    public float m_WaitBeforeTeleport;
    public SlimeManager.GameType m_LevelToLoad;

    [SerializeField]
    private GameObject m_LoadingLayer;
    [SerializeField]
    private SlimeDeath m_SlimeDeath;

    private bool isUsed = false;
    private bool isHolding = false;

    //TO-DO:
    //if holding play hold particles
    //start timer (m_WaitBeforeTeleport)
    //when timer reaches 0, play teleport particles, maybe screenshake & bloom too
    //after teleport particles end, change scene

    // Use this for initialization
    void Start()
    {
        m_HoldParticles.Stop();
        m_TeleportParticles.Stop();
    }

    public override void OnGrab(MoveController currentController) //run once when picked up
    {
        base.OnGrab(currentController);
        m_HoldParticles.Play();
        isHolding = true;
    }

    public override void OnGrabReleased(MoveController currentController)
    {
        base.OnGrabReleased(currentController);
        m_HoldParticles.Stop();
        isHolding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHolding)
        {
            m_WaitBeforeTeleport -= Time.deltaTime;

            if (m_WaitBeforeTeleport <= 0 && !isUsed)
            {
                isUsed = true;

                foreach (var rune in FindObjectsOfType<Runes>())
                {
                    if (rune != this)
                        rune.enabled = false;
                }

                m_TeleportParticles.Play(true);

                StartCoroutine(FlashLoadingRoutine());
                StartCoroutine(FlashOutRoutine());
            }
        }

    }

    IEnumerator FlashLoadingRoutine()
    {
        yield return new WaitForSeconds(9);

        //enable loading layer
        m_LoadingLayer.SetActive(true);
    }

    IEnumerator FlashOutRoutine()
    {
        yield return new WaitForSeconds(3);

        var ppBehaviour = Camera.main.GetComponent<PostProcessingBehaviour>();
        var bloomSettings = ppBehaviour.profile.bloom.settings;
        ppBehaviour.profile.bloom.enabled = true;

        float intensityTarget = 300, flashPeakTime = 7.0f;

        float intensityDelta = (intensityTarget - bloomSettings.bloom.intensity) / flashPeakTime;

        float currentFlashDuration = 0;

        bloomSettings.bloom.intensity = 0;
        bloomSettings.bloom.threshold = 0;
        bloomSettings.bloom.radius = 4;
        ppBehaviour.profile.bloom.settings = bloomSettings;

        while (currentFlashDuration < flashPeakTime)
        {
            currentFlashDuration += Time.deltaTime;
            bloomSettings.bloom.radius = Mathf.Clamp(bloomSettings.bloom.radius + Time.deltaTime, 0, 7);

            bool intenseDone = true;

            if (bloomSettings.bloom.intensity < intensityTarget)
            {
                bloomSettings.bloom.intensity += intensityDelta * Time.deltaTime;
                intenseDone = false;
            }

            ppBehaviour.profile.bloom.settings = bloomSettings;

            if (intenseDone)
            {
                break;
            }
            else
            {
                yield return null;
            }
        }

        SlimeManager.m_GameType = m_LevelToLoad;

        //insert fade out here
        //insert scene change here, after finish particles & fade out
        var asyncLoad = SceneManager.LoadSceneAsync("Gameplay");
        asyncLoad.allowSceneActivation = false; //wont auto change when loaded

        asyncLoad.completed += (AsyncOperation ops) =>
        {
            bloomSettings.bloom.intensity = 0;
            bloomSettings.bloom.threshold = 0;
            bloomSettings.bloom.radius = 4;
            ppBehaviour.profile.bloom.settings = bloomSettings;
            ppBehaviour.profile.bloom.enabled = false;
        };

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                //if finished loading, make slimes dead
                m_SlimeDeath.RemoveFromScene();

                yield return new WaitForSeconds(5f);
                asyncLoad.allowSceneActivation = true;
                yield break;
            }

            yield return null;
        }

    }

}