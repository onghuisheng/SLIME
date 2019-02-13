using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;
using DG.Tweening;


public class Runes : GrabbableObject
{

    public ParticleSystem m_HoldParticles;
    public ParticleSystem m_TeleportParticles;
    public float m_WaitBeforeTeleport;
    private float m_DefaultWaitBeforeTeleport;
    public SlimeManager.GameType m_LevelToLoad;

    [SerializeField]
    private GameObject m_LoadingLayer;
    [SerializeField]
    private SlimeDeath m_SlimeDeath;
    [SerializeField]
    private Text m_Tips;

    private Material m_Material;
    private Color m_DefaultMaterialColor;

    private bool m_IsUsed = false;
    public bool isUsed {
        get { return m_IsUsed; }
    }

    private bool m_IsHolding = false;

    private static AudioSource m_BubbleAudio;

    //TO-DO:
    //if holding play hold particles
    //start timer (m_WaitBeforeTeleport)
    //when timer reaches 0, play teleport particles, maybe screenshake & bloom too
    //after teleport particles end, change scene

    // Use this for initialization
    void Start()
    {
        m_Material = GetComponent<MeshRenderer>().material;
        m_DefaultMaterialColor = m_Material.GetColor("_EmissionColor");
        m_HoldParticles.Stop();
        m_TeleportParticles.Stop();

        m_DefaultWaitBeforeTeleport = m_WaitBeforeTeleport;

        m_BubbleAudio = null;
    }

    public override void OnGrab(MoveController currentController) //run once when picked up
    {
        base.OnGrab(currentController);
        m_HoldParticles.Play();
        m_IsHolding = true;

        if (m_BubbleAudio == null)
            m_BubbleAudio = AudioManager.Instance.Play2D("bubbles", AudioManager.AudioType.Additive).GetComponent<AudioSource>();
    }

    public override void OnGrabReleased(MoveController currentController)
    {
        base.OnGrabReleased(currentController);
        m_HoldParticles.Stop();
        m_IsHolding = false;

        m_WaitBeforeTeleport = m_DefaultWaitBeforeTeleport;

        // Prevent audio from stopping if holding another rune
        var otherRune = currentController.GetCurrentHandObject(true);

        if (!m_IsUsed && m_BubbleAudio != null && (otherRune == null || otherRune.GetComponent<Runes>() == null))
        {
            if (otherRune != null)
            {
                var actualRune = otherRune.GetComponent<Runes>();
                if (actualRune != null && actualRune.isUsed)
                    return;
            }

            m_BubbleAudio.Stop();
            m_BubbleAudio = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsHolding)
        {
            m_WaitBeforeTeleport -= Time.deltaTime;

            if (m_WaitBeforeTeleport <= 0 && !m_IsUsed)
            {
                m_IsUsed = true;

                foreach (var rune in FindObjectsOfType<Runes>())
                {
                    if (rune != this)
                        rune.enabled = false;
                }

                m_TeleportParticles.Play(true);

                StartCoroutine(FlashLoadingRoutine());
                StartCoroutine(FlashOutRoutine());
            }

            m_Material.SetColor("_EmissionColor", m_DefaultMaterialColor * (Mathf.PingPong(Time.time, 1) + .25f));
        }

    }

    IEnumerator FlashLoadingRoutine()
    {
        yield return new WaitForSeconds(9);

        //enable loading layer
        m_LoadingLayer.SetActive(true);
        m_Tips.DOFade(1, 1);
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

                var tween = m_Tips.DOFade(0, 1.5f);

                yield return tween.WaitForCompletion();
                asyncLoad.allowSceneActivation = true;
                yield break;
            }

            yield return null;
        }

    }

    private void OnDestroy()
    {
        if (m_Material != null)
            Destroy(m_Material);
    }

}