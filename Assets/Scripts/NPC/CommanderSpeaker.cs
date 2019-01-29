using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CommanderSpeaker : SingletonMonoBehaviour<CommanderSpeaker>
{
    private bool m_IsMuted;

    private GameObject m_SpeakerJoint;

    [Range(-5, 5)]
    public float strength = 1;

    [Range(0, 5)]
    public float duration = 1;

    [Range(0, 30)]
    public int vibrato = 10;

    [Range(0, 1)]
    public float elasticity = 1;

    private void Start()
    {
        m_SpeakerJoint = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            BobBob(duration, vibrato, elasticity);
            AudioManager.Instance.Play2D("shortsmallhorn", AudioManager.AudioType.Additive, new AudioSourceData2D() { pitchOverride = 0.6f });
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            BobBob(duration, vibrato, elasticity);
            AudioManager.Instance.Play2D("shortsmallhorn", AudioManager.AudioType.Additive, new AudioSourceData2D() { pitchOverride = 1 });
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            foreach (var enemy in FindObjectsOfType<EnemyHit>())
            {
                enemy.OnShot(null);
            }
        }
    }

    public void ToggleMute(bool toggle)
    {
        m_IsMuted = toggle;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sponge" && !m_IsMuted)
        {
            Sponge sponge = other.transform.parent.GetComponent<Sponge>();

            var controller = MoveController.GetControllerThatHolds(sponge.gameObject);

            if (controller != null)
            {
                controller.DetachCurrentObject(false);
            }

            var childRot = transform.GetChild(0).transform.rotation.eulerAngles;

            sponge.transform.position = transform.position;
            sponge.transform.rotation = transform.rotation;
            sponge.transform.position += sponge.transform.up * 0.2f;
            sponge.transform.Rotate(0, 90, 0, Space.Self);
            sponge.GetComponent<Rigidbody>().isKinematic = true;
            sponge.transform.parent.GetComponent<Collider>().enabled = true;
            sponge.gameObject.SetActive(false);

            m_IsMuted = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Sponge" && m_IsMuted)
        {
            Sponge sponge = other.transform.parent.GetComponent<Sponge>();
            m_IsMuted = false;
            sponge.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    public GameObject PlaySpeaker(string clipAlias, AudioManager.AudioType audioType, float delayInSeconds = 0, System.Action onComplete = null)
    {
        return AudioManager.Instance.Play3D(clipAlias, transform.position, audioType, new AudioSourceData3D() { pitchOverride = 1, volume = 0.5f }, delayInSeconds, onComplete);
    }

    public void BobBob(float duration, int vibrato = 10, float elasticity = 1)
    {
        m_SpeakerJoint.transform.DOComplete();
        m_SpeakerJoint.transform.DOPunchScale(Vector3.one * strength, duration, vibrato, elasticity).SetEase(Ease.Linear);
    }

}
