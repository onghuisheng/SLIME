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


    // Update is called once per frame
    void Update()
    {
        if (PlayerDamage.instance.m_DamageLevel == 4 && !m_IsFading)
        {
            m_IsFading = true;
            StartCoroutine(FadeOut(1));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null || m_IsFading)
            return;

        if (other.GetComponent<SlimeHitBarricade>())
        {
            m_IsFading = true;
            KillPlayer();
        }
    }

    public void KillPlayer()
    {
        StartCoroutine(FadeOut(1));
    }

    IEnumerator FadeOut(float time)
    {
        CommanderSpeaker.Instance.StopSpeaker();
        AudioManager.Instance.StopAllCoroutines();
        var audio = CommanderSpeaker.Instance.PlaySpeaker(RandomEx.RandomString("npc_death1", "npc_death2", "npc_death3"), AudioManager.AudioType.Additive, 0.5f).GetComponent<AudioSource>();

        var tween = m_FadeScreen.DOFade(1, time);
        yield return tween.WaitForCompletion();

        var asyncLoad = SceneManager.LoadSceneAsync("Main Menu");
        asyncLoad.allowSceneActivation = false;

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            // wait until audio finished playing
            if (asyncLoad.progress >= 0.9f && (audio == null || !audio.isPlaying))
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }

}
