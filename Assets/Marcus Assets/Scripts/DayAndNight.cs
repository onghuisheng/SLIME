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

    public ParticleSystem DayParticles;
    public ParticleSystem NightParticles;


    // Use this for initialization
    void Start()
    {
        mainLight = GetComponent<Light>();
        skyMat = RenderSettings.skybox;
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

        mainLight.color = nightdayColor.Evaluate(dot);
        RenderSettings.ambientLight = mainLight.color;

        RenderSettings.fogColor = nightdayFogColor.Evaluate(dot);
        RenderSettings.fogDensity = fogDensityCurve.Evaluate(dot) * fogScale;

        i = ((dayAtmosphereThickness - nightAtmosphereThickness) * dot) + nightAtmosphereThickness;
        skyMat.SetFloat("_AtmosphereThickness", i);

        if (dot > 0) // day
        {
            transform.Rotate(dayRotateSpeed * Time.deltaTime * skyspeed);
            NightLight.SetActive(false);

            DayParticles.Play(true);
            NightParticles.Play(false);

        }
        else // night
        {
            transform.Rotate(nightRotateSpeed * Time.deltaTime * skyspeed);
            NightLight.SetActive(true);

            if(SlimeManager.instance.m_CurrentWave == 1)
            {
                SlimeManager.instance.m_SlimeInWave.Clear();
                SlimeManager.instance.m_GolemSlimeInWave.Clear();
                SlimeManager.instance.m_FinishSpawnWave = false;
            }

            SlimeManager.instance.m_CurrentWave = 2;

            DayParticles.Play(false);
            NightParticles.Play(true);

        }

        if (Input.GetKeyDown(KeyCode.Equals)) skyspeed += 0.5f;
        if (Input.GetKeyDown(KeyCode.Minus)) skyspeed -= 0.5f;


        //Debug.Log("DOT: " + dot);
    }
}