using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGameOver : MonoBehaviour {

    public Image m_FadeScreen;

	// Use this for initialization
	void Start () {
        //For the commander
		//CommanderSpeaker.Instance.PlaySpeaker()
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
            return;

        if (other.GetComponent<SlimeHitBarricade>())
        {
            StartCoroutine(FadeOut(3.0f));
        }
    }

    IEnumerator FadeOut(float time)
    {
        float m_ElapsedTime = 0;
        
        while (m_ElapsedTime < time)
        {
            float alpha = Mathf.Lerp(m_FadeScreen.color.a, 1.0f, m_ElapsedTime / time);
            m_FadeScreen.color = new Color(m_FadeScreen.color.r, m_FadeScreen.color.g, m_FadeScreen.color.b, alpha);
            m_ElapsedTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
}
