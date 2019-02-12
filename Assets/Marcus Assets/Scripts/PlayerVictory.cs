using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PlayerVictory : MonoBehaviour
{

    [SerializeField]
    private GameObject m_CreditsLayer;

    [SerializeField]
    private GameObject m_CreditsCanvas;

    public GameObject m_FireWorks;
    private RocketKaboom m_Kaboom;
    public bool m_Spawn;

    // Use this for initialization
    void Start()
    {
        m_Spawn = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (SlimeManager.instance.m_GameFinish == true && m_Spawn == false)
        {
            FindObjectOfType<TutorialHandler>().StopAllCoroutines();
            CommanderSpeaker.Instance.StopSpeaker();
            AudioManager.Instance.StopAllCoroutines();
            CommanderSpeaker.Instance.PlaySpeaker("npc_victory", AudioManager.AudioType.Additive, 1.5f, () =>
            {
                m_Kaboom = Instantiate(m_FireWorks, m_FireWorks.transform.position, m_FireWorks.transform.rotation).GetComponent<RocketKaboom>();
                StartCoroutine(CreditsRoutine());
            });

            m_Spawn = true;

        }

        if (FindObjectOfType<MoveController>().GetButtonDown(MoveControllerButton.Circle))
        {
            SlimeManager.instance.m_GameFinish = true;
        }
    }

    IEnumerator CreditsRoutine()
    {
        yield return new WaitForSeconds(8);

        var gameOver = FindObjectOfType<PlayerGameOver>();

        const float fadeTime = 3;

        gameOver.m_FadeScreen.DOColor(FadeManager.Instance._fadeColor, fadeTime);
        gameOver.m_FadeScreen.DOFade(1, fadeTime).OnComplete(() =>
        {
            m_Kaboom.gameObject.SetActive(false);
            // Camera.main.enabled = false;
            m_CreditsLayer.SetActive(true);
            AudioManager.Instance.Play2D("credits", AudioManager.AudioType.Additive, new AudioSourceData2D() { randomPitchRange = 0 });
            m_CreditsCanvas.GetComponent<RectTransform>().DOLocalMoveY(7, 20).SetEase(Ease.Linear).OnComplete(() =>
            {
                gameOver.KillPlayer(3, 2, false);
            });
        });

    }

}
