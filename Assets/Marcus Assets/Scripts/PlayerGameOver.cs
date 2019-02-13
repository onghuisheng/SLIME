using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PlayerGameOver : MonoBehaviour
{

    public Image m_FadeScreen;

    private bool m_IsFading = false;

    private bool m_IsPlayerDead = false;

    [SerializeField]
    private bool m_IsGodmode = false;


    // Update is called once per frame
    void Update()
    {
        if (!m_IsGodmode && PlayerDamage.instance.m_DamageLevel == 4 && !m_IsFading)
        {
            m_IsFading = true;
            KillPlayer();
        }
    }

    public void KillPlayer(float fadeTime = 1, float startDelay = 0, bool playCommanderVoice = true)
    {
        if (m_IsPlayerDead)
            return;

        m_IsPlayerDead = true;
        StartCoroutine(FadeOut(fadeTime, startDelay, playCommanderVoice));
    }

    IEnumerator FadeOut(float time, float startDelay, bool playCommanderVoice)
    {
        yield return new WaitForSeconds(startDelay);

        CommanderSpeaker.Instance.StopSpeaker();
        AudioManager.Instance.StopAllCoroutines();

        AudioSource audio = null;

        string audioStr = RandomEx.RandomString("npc_death1", "npc_death2", "npc_death3");

        const float commandDelay = 0.5f;

        if (playCommanderVoice)
            audio = CommanderSpeaker.Instance.PlaySpeaker(audioStr, AudioManager.AudioType.Additive, commandDelay).GetComponent<AudioSource>();

        Tweener tween = null;

        if (playCommanderVoice)
            tween = m_FadeScreen.DOFade(1, AudioManager.Instance.GetAudioClip(audioStr).length + commandDelay);
        else
            tween = m_FadeScreen.DOFade(1, time);

        yield return tween.WaitForCompletion();

        var asyncLoad = SceneManager.LoadSceneAsync("Main Menu");
        asyncLoad.allowSceneActivation = false;

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            // wait until audio finished playing
            if (asyncLoad.progress >= 0.9f && audio == null)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }

}
