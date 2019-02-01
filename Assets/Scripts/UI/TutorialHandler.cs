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

    private enum Instruction
    {
        None,
        RingBell,
        FireBow,

    }


    private void Start()
    {
        if (m_FadeOutOnStart)
        {
            StartCoroutine(FadeManager.Instance.FadeOut(1));
        }
        
        // Start tutorial
        StartCoroutine(TutorialRoutine());
    }

    private IEnumerator TutorialRoutine()
    {
        CommanderSpeaker.Instance.PlaySpeaker("npc_tutorial11", AudioManager.AudioType.Queue, 1);
        CommanderSpeaker.Instance.PlaySpeaker("npc_tutorial12", AudioManager.AudioType.Queue, 2, () =>
        {
            StartCoroutine(FadeManager.Instance.FadeOut(3, 1, () =>
            {
                CommanderSpeaker.Instance.PlaySpeaker("npc_tutorial3", AudioManager.AudioType.Additive, 0, () =>
                {
                    m_Bell.OnItemDrop += OnBellItemDrop;
                });
            }));
        });

        yield break;
    }

    void OnBellItemDrop(List<GameObject> items)
    {
        m_Bell.OnItemDrop -= OnBellItemDrop;
        m_IsBellRung = true;
        CommanderSpeaker.Instance.PlaySpeaker("npc_tutorial3", AudioManager.AudioType.Additive, 1);
    }

}
