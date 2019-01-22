using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeDestruction : MonoBehaviour
{

    public GameObject m_NewBarricadeDamaged;
    public GameObject m_NewBarricadeDestroyed;

    private int m_DamageLevel;
    private int m_BarricadeHealth;

    // Use this for initialization
    void Start()
    {
        m_BarricadeHealth = 2;
        m_DamageLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
            return;

        if (other.GetComponent<SlimeHitBarricade>())
        {
            if (m_BarricadeHealth > 0)
            {
                m_BarricadeHealth--;
            }
            else
            {
                if (m_DamageLevel == 0)
                {
                    if (m_NewBarricadeDamaged)
                        Instantiate(m_NewBarricadeDamaged, transform.position, transform.rotation);

                    m_DamageLevel++;
                }
                else if (m_DamageLevel == 1)
                {
                    if (m_NewBarricadeDestroyed)
                        Instantiate(m_NewBarricadeDestroyed, transform.position, transform.rotation);

                    m_DamageLevel++;
                }

                if (transform.childCount == 0)
                {
                    Destroy(this.gameObject/*, 5*/);
                }

                else
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        GameObject m_Child = transform.GetChild(i).gameObject;
                        m_Child.AddComponent<Rigidbody>();
                        Destroy(this.gameObject, 5);

                    }
                }

                m_BarricadeHealth = 2;
            }
        }
    }
}
