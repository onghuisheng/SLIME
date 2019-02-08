using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialHandler : MonoBehaviour
{
    [SerializeField]
    private bool m_FadeOutOnStart;

    [SerializeField]
    private Bell m_Bell;

    [SerializeField]
    private Bow m_Bow;

    private bool m_IsBellRung = false;
    private bool m_IsBowPicked = false;
    private bool m_IsEnded = false;
    public bool isTutorialDone { get { return (m_IsBellRung && m_IsBowPicked); } }


    private void Start()
    {
        if (m_FadeOutOnStart)
        {
            StartCoroutine(FadeManager.Instance.FadeOut(2));
        }

        // Start tutorial
        TutorialRoutine();
    }

    private void TutorialRoutine()
    {
        CommanderSpeaker.Instance.PlaySpeaker("npc_tutorial11", AudioManager.AudioType.Queue, 1); // hey 
        CommanderSpeaker.Instance.PlaySpeaker("npc_tutorial12", AudioManager.AudioType.Queue, 2, () => // hey stop day dreaming
        {
            StartCoroutine(FadeManager.Instance.FadeOut(3, 1, () =>
            {
                CommanderSpeaker.Instance.PlaySpeaker("npc_tutorial2", AudioManager.AudioType.Additive, 0, () => // ring the bell on the right
                {
                    m_Bell.OnBellRung += OnBellRung;
                    StartCoroutine(RepeatRoutine(8, true, "npc_tutorial2"));
                });
            }));
        });
    }

    void OnBellRung()
    {
        m_Bell.OnBellRung -= OnBellRung;
        m_IsBellRung = true;

        StopAllCoroutines();
        AudioManager.Instance.StopAllCoroutines();
        CommanderSpeaker.Instance.StopSpeaker();

        if (MoveController.GetControllerThatHolds(m_Bow.gameObject) != null) // If player is holding the bow, skip the bow tutorial
        {
            m_IsBowPicked = true;
            StopAllCoroutines();
            AudioManager.Instance.StopAllCoroutines();
            CommanderSpeaker.Instance.StopSpeaker();
            CommanderSpeaker.Instance.PlaySpeaker("npc_tutorial51", AudioManager.AudioType.Additive, 1, () => // when you're ready, light up your arrow
            {
                StartCoroutine(RepeatRoutine(8, false, "npc_tutorial52"));
            });
        }
        else
        {
            CommanderSpeaker.Instance.PlaySpeaker(((Random.Range(0, 2) == 1) ? "npc_goodjob" : "npc_welldone"), AudioManager.AudioType.Additive, 0.5f, () =>
            {
                CommanderSpeaker.Instance.PlaySpeaker(((Random.Range(0, 2) == 1) ? "npc_tutorial31" : "npc_tutorial32"), AudioManager.AudioType.Additive, 1, () => // your weapon is on your left
                {
                    m_Bow.OnBowGrabbed += OnBowGrabbed;
                    StartCoroutine(RepeatRoutine(8, true, "npc_tutorial31", "npc_tutorial32"));
                });
            });
        }
    }

    void OnBowGrabbed(GameObject bow)
    {
        m_Bow.OnBowGrabbed -= OnBowGrabbed;
        m_IsBowPicked = true;

        StopAllCoroutines();
        AudioManager.Instance.StopAllCoroutines();
        CommanderSpeaker.Instance.StopSpeaker();

        CommanderSpeaker.Instance.PlaySpeaker(((Random.Range(0, 2) == 1) ? "npc_goodjob" : "npc_welldone"), AudioManager.AudioType.Additive, 0, () =>
        {
            CommanderSpeaker.Instance.PlaySpeaker("npc_tutorial51", AudioManager.AudioType.Additive, 1, () => // when you're ready, light up your arrow
            {
                StartCoroutine(RepeatRoutine(8, false, "npc_tutorial52"));
            });
        });
    }

    IEnumerator RepeatRoutine(float startDelay, bool useWhat, params string[] repeatClipAlias)
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            if (useWhat)
            {
                CommanderSpeaker.Instance.PlaySpeaker("npc_what", AudioManager.AudioType.Additive, 0, () =>
                {
                    string clipAlias = repeatClipAlias[Random.Range(0, repeatClipAlias.Length)];
                    CommanderSpeaker.Instance.PlaySpeaker(clipAlias, AudioManager.AudioType.Additive, 0.5f);
                });
            }
            else
            {
                string clipAlias = repeatClipAlias[Random.Range(0, repeatClipAlias.Length)];
                CommanderSpeaker.Instance.PlaySpeaker(clipAlias, AudioManager.AudioType.Additive, 0.5f);
            }

            yield return new WaitForSeconds(startDelay);
        }
    }

    public void EndTutorial()
    {
        m_IsEnded = true;

        m_Bow.OnBowGrabbed = null;
        m_Bell.OnBellRung = null;

        StopAllCoroutines();
        AudioManager.Instance.StopAllCoroutines();

        CommanderSpeaker.Instance.StopSpeaker();

        if (!isTutorialDone)
            CommanderSpeaker.Instance.PlaySpeaker(((Random.Range(0, 2) == 1) ? "npc_tutorialalt1" : "npc_tutorialalt2"), AudioManager.AudioType.Additive, 0);
    }

}
