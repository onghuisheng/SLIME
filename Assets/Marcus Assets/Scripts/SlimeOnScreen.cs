using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeOnScreen : MonoBehaviour {

    public Vector3 m_OriginalScale;

	// Use this for initialization
	void Start () {
        GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, 0.1f);
        transform.localScale = new Vector3(transform.localScale.x * 2f, transform.localScale.y * 2f, transform.localScale.z);
        StartCoroutine(SizeUp(1.0f));
	}
	
	// Update is called once per frame
	void Update () {

	}

    IEnumerator SizeUp(float time)
    {
        float m_ElapsedTime = 0;

        while (m_ElapsedTime < time)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, m_OriginalScale, m_ElapsedTime / time);
            float tempA = Mathf.Lerp(GetComponent<Image>().color.a, 1, (m_ElapsedTime / time) * 1.5f);
            GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, tempA);

            m_ElapsedTime += Time.deltaTime;

            yield return null;
        }
        yield return null;
    }
}
