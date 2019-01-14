using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour {

    //init
    void Start()
    {
        AudioManager.Instance.Play3D("flameloop", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { loop = true, volume = 0.5f });
    }

    private void OnTriggerEnter(Collider other)
    {
        ArrowBase arrow = other.GetComponent<ArrowBase>();

        if (arrow != null && !arrow.isFired)
        {
            arrow.BuffArrow(ArrowBase.ArrowType.Flame);

            AudioManager.Instance.Play3D("arrowlit", transform.position, AudioManager.AudioType.Additive);
        }

        Dynamite dynamite = other.GetComponent<Dynamite>();

        if (dynamite != null)
        {
            dynamite.DynamiteLit();
        }

    }

}
