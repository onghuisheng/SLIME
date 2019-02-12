using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CommanderSpeaker : SingletonMonoBehaviour<CommanderSpeaker>
{
    private bool m_IsMuted;

    [SerializeField]
    private GameObject m_SpeakerJoint;

    [Range(-5, 5)]
    public float strength = 1;

    [Range(0, 5)]
    public float duration = 1;

    [Range(0, 30)]
    public int vibrato = 10;

    [Range(0, 1)]
    public float elasticity = 1;

    private Tweener m_CurrentSpeakerTween;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            BobBob(0, duration, vibrato, elasticity);
            PlaySpeaker("shortsmallhorn", AudioManager.AudioType.Additive, 0);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            BobBob(0, duration, vibrato, elasticity);
            PlaySpeaker("shortsmallhorn", AudioManager.AudioType.Additive, 0);
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

            StopSpeaker();

            if (m_CurrentSpeakerTween != null)
                m_CurrentSpeakerTween.Complete();

            m_IsMuted = true;
        }
    }

    public void StopSpeaker()
    {
        var audios = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audio in audios)
        {
            if (audio.name.Contains("npc_"))
            {
                audio.Stop();
                Destroy(audio.gameObject);
            }
        }
    }

    public GameObject PlaySpeaker(string clipAlias, AudioManager.AudioType audioType, float delayInSeconds = 0, System.Action onComplete = null)
    {
        var clip = AudioManager.Instance.GetAudioClip(clipAlias);

        return AudioManager.Instance.Play3D(clipAlias, transform.position, audioType, new AudioSourceData3D() { pitchOverride = 1, volume = ((m_IsMuted) ? 0 : 0.5f) }, delayInSeconds, onComplete, () =>
        {
            if (!m_IsMuted)
                StartCoroutine(BobBob(delayInSeconds, clip.length));
        });
    }

    private IEnumerator BobBob(float delay, float duration, int vibrato = 10, float elasticity = 1)
    {
        if (delay != 0)
            yield return new WaitForSeconds(delay);

        if (m_CurrentSpeakerTween != null)
            m_CurrentSpeakerTween.Complete();

        m_CurrentSpeakerTween = m_SpeakerJoint.transform.DOPunchScale(Vector3.one * strength, duration, vibrato, elasticity).SetEase(Ease.Linear);
    }

}
