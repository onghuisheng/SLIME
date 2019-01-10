using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour, IShootable
{

    [SerializeField]
    private ParticleSystem m_FlameParticles;    // flame particle
    
    [SerializeField, Range(1,100)]
    private float m_FlamingDuration;

    private GameObject m_bonfireloop;

    bool isLighted = false;
    
    public void OnShot(ArrowBase arrow)
    {
        if (arrow.arrowType == ArrowBase.ArrowType.Flame && !isLighted) //if its flame arrow
        {
            ToggleFire(true);
        }
    }

    public void ToggleFire(bool toggle)
    {
        isLighted = toggle;

        if (toggle)
        {
            AudioManager.Instance.Play3D("bonfirelit", transform.position, AudioManager.AudioType.Additive);

           m_bonfireloop= AudioManager.Instance.Play3D("bonfireloop", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { loop = true });

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
            ToggleFire(!isLighted);
        }
    }
}