using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oil : MonoBehaviour {

    public float m_LifeTime;
    private Vector3 m_DesiredScale;

    // Use this for initialization
    void Start()
    {
        m_LifeTime = 0.0f;
        Random.InitState((int)System.DateTime.Now.Ticks); // make random more random
        m_DesiredScale = new Vector3(Random.Range(1.0f, 3.0f), transform.localScale.y, Random.Range(1.0f, 3.0f));
        
    }

    // Update is called once per frame
    void Update()
    {
        m_LifeTime += Time.deltaTime;

        if (m_LifeTime > 20.0f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.0f, 0.0f, 0.0f), 1 * Time.deltaTime);

            if (transform.localScale.x > 0.1f)
                Destroy(this);
        }

        else
        {
            if(transform.localScale.x != m_DesiredScale.x &&
                transform.localScale.z != m_DesiredScale.z)
            transform.localScale = Vector3.Lerp(transform.localScale, m_DesiredScale, 1 * Time.deltaTime);

        }
    }
}
