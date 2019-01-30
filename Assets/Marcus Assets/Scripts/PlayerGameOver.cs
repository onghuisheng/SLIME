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

        }
    }

    IEnumerator FadeOut(float time)
    {
        float m_ElapsedTime = 0;
        
        Color tempColor = m_FadeScreen.color;

        while (m_ElapsedTime < time)
        {
            tempColor.a = Mathf.Lerp(tempColor.a, 1, m_ElapsedTime / time);
            m_FadeScreen.color = tempColor;
            m_ElapsedTime += Time.deltaTime;
        }

        yield return null;
    }
}
