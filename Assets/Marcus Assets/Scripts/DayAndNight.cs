using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour {

    public Gradient nightdayColor;

    public float maxIntensity = 3f;
    public float minIntensity = 0f;
    public float minPoint = -0.2f;

    public float maxAmbient = 1f;
    public float minAmbient = 0f;
    public float minAmbientPoint = -0.2f;

    public Gradient nightdayFogColor;
    public AnimationCurve fogDensityCurve;
    public float fogScale = 1f;

    public float dayAtmosphereThickness = 0.4f;
    public float nightAtmosphereThickness = 0.07f;

    public Vector3 dayRotateSpeed;
    public Vector3 nightRotateSpeed;

    [SerializeField]
    float skyspeed = 1;

    Light mainLight;
    Skybox sky;
    UnityEngine.Material skyMat;

    public GameObject NightLight;

    [HideInInspector]
    public float tRange;
    //[HideInInspector]
    public float dot;
    [HideInInspector]
    public float i;

    private float prevDot;

    public List<ParticleSystem> DayParticles;
    public List<ParticleSystem> NightParticles;

    private GameObject m_NightAudioLoop;
    private GameObject m_DayAudioLoop;


    // Use this for initialization
    void Start()
    {
        mainLight = GetComponent<Light>();
        skyMat = RenderSettings.skybox;

        foreach (ParticleSystem ps in DayParticles)
        {
            ps.Play(true);
        }

        foreach (ParticleSystem ps in NightParticles)
        {
            ps.Stop(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        tRange = 1 - minPoint;
        dot = Mathf.Clamp01((Vector3.Dot(mainLight.transform.forward, Vector3.down) - minPoint) / tRange);
        i = ((maxIntensity - minIntensity) * dot) + minIntensity;

        mainLight.intensity = i;

        tRange = i - minAmbientPoint;
        dot = Mathf.Clamp01((Vector3.Dot(mainLight.transform.forward, Vector3.down) - minAmbientPoint) / tRange);
        i = (maxAmbient - minAmbient * dot) + minAmbient;

        RenderSettings.ambientIntensity = i;
        RenderSettings.reflectionIntensity = i;

        mainLight.color = nightdayColor.Evaluate(dot);
        RenderSettings.ambientLight = mainLight.color;

        RenderSettings.fogColor = nightdayFogColor.Evaluate(dot);
        RenderSettings.fogDensity = fogDensityCurve.Evaluate(dot) * fogScale;

        i = ((dayAtmosphereThickness - nightAtmosphereThickness) * dot) + nightAtmosphereThickness;
        skyMat.SetFloat("_AtmosphereThickness", i);

        if (dot > 0) // day
        {
            transform.Rotate(dayRotateSpeed * Time.deltaTime * skyspeed);

            if (prevDot <= 0)
                OnDayStart();
        }
        else // night
        {
            transform.Rotate(nightRotateSpeed * Time.deltaTime * skyspeed);

            if(SlimeManager.instance.m_CurrentWave == 1)
            {
                SlimeManager.instance.m_SlimeInWave.Clear();
                SlimeManager.instance.m_GolemSlimeInWave.Clear();
                SlimeManager.instance.m_FinishSpawnWave = false;
            }

            SlimeManager.instance.m_CurrentWave = 2;

            if (prevDot > 0)
                OnNightStart();
        }

        if (Input.GetKeyDown(KeyCode.Equals)) skyspeed += 0.5f;
        if (Input.GetKeyDown(KeyCode.Minus)) skyspeed -= 0.5f;


        // Must be at the bottom
        prevDot = dot;
        //Debug.Log("DOT: " + dot);
    }

    private void OnDayStart()
    {
        NightLight.SetActive(false);

        AudioManager.Instance.Play2D("rooster", AudioManager.AudioType.Additive);

        m_DayAudioLoop = AudioManager.Instance.Play2D("dayambient", AudioManager.AudioType.Additive, new AudioSourceData2D() { loop = true });

        foreach (ParticleSystem ps in DayParticles)
        {
            ps.Play(true);
        }

        foreach (ParticleSystem ps in NightParticles)
        {
            ps.Stop(true);
        }
    }

    private void OnNightStart()
    {
        // NightLight.SetActive(true);

        AudioManager.Instance.Play2D("wolf", AudioManager.AudioType.Additive);

        Destroy(m_DayAudioLoop);

        foreach (ParticleSystem ps in DayParticles)
        {
            ps.Stop(true);
        }

        foreach (ParticleSystem ps in NightParticles)
        {
            ps.Play(true);
        }
    }

}