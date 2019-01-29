using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour, IShootable
{

    [SerializeField]
    private ParticleSystem m_FlameParticles;    // flame particle

    [SerializeField, Range(1, 100)]
    private float m_FlamingDuration;

    private GameObject m_bonfireloop;

    bool isLighted = false;

    public bool isFirstShot = true;

    //this..is kinda... ew.. so please improve this :,UUU
    public GameObject m_SpawnPointGO1;
    public GameObject m_SpawnPointGO2;
    public GameObject m_SpawnPointGO3;
    public GameObject m_ArchersGO;


    public void OnShot(ArrowBase arrow)
    {
        if ((arrow.arrowType == ArrowBase.ArrowType.Flame || arrow.arrowType == ArrowBase.ArrowType.FlameVolley) && !isLighted) //if its flame arrow
        {
            ToggleFire(true);

            //If it is first time, send in the wave
            if (isFirstShot)
            {
                StartWave();
            }
        }
    }

    public void StartWave()
    {
        //this..is kinda... ew.. so please improve this :,UUU

        //Play horn audio only if the game objects are inactive
        //This isnt working im not too sure why. :,u
        foreach (var bonfire in FindObjectsOfType<Bonfire>())
        {
            bonfire.isFirstShot = false;
        }

        AudioManager.Instance.Play2D("warhorn", AudioManager.AudioType.Additive, new AudioSourceData2D() { pitchOverride = 1 }, 2, () =>
          {
            //set gameobjects as active
            if (m_SpawnPointGO1 && m_SpawnPointGO2 && m_SpawnPointGO3 && m_ArchersGO)
              {
                  m_SpawnPointGO1.SetActive(true);
                  m_SpawnPointGO2.SetActive(true);
                  m_SpawnPointGO3.SetActive(true);
                  m_ArchersGO.SetActive(true);
              }
          });

    }

    public void ToggleFire(bool toggle)
    {
        isLighted = toggle;

        if (toggle)
        {
            AudioManager.Instance.Play3D("bonfirelit", transform.position, AudioManager.AudioType.Additive);

            m_bonfireloop = AudioManager.Instance.Play3D("bonfireloop", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { loop = true });

            m_FlameParticles.Play(true);
            GetComponent<Collider>().enabled = false;
            Invoke("StopFire", m_FlamingDuration);
        }
        else
        {
            m_FlameParticles.Stop(true);
            GetComponent<Collider>().enabled = true;
        }
    }

    private void StopFire()
    {
        ToggleFire(false);
        Destroy(m_bonfireloop);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartWave();
        }
    }
}