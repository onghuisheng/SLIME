using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PlayerGameOver : MonoBehaviour
{

    public Image m_FadeScreen;

    // Use this for initialization
    void Start()
    {
        //For the commander
        //CommanderSpeaker.Instance.PlaySpeaker()
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerDamage.instance.m_DamageLevel == 3)
        {
            StartCoroutine(FadeOut(5.0f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
            return;

        if (other.GetComponent<SlimeHitBarricade>())
        {
            StartCoroutine(FadeOut(1.0f));
        }
    }

    IEnumerator FadeOut(float time)
    {

        var tween = m_FadeScreen.DOFade(255, time);
        yield return tween.WaitForCompletion();
        
        var asyncLoad = SceneManager.LoadSceneAsync("Main Menu");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
