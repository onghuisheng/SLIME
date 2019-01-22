using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeDestruction : MonoBehaviour
{

    public GameObject m_NewBarricadeDamaged;
    public GameObject m_NewBarricadeDestroyed;

    private int m_DamageLevel;
    private int m_BarricadeHealth;

    public GameObject currentBarricade;
    public GameObject tempBarricade;

    // Use this for initialization
    void Start()
    {
        m_BarricadeHealth = 1;
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
            // add particles here ****************************





            // minus health when hp above 0
            if (m_BarricadeHealth > 0) 
            {
                m_BarricadeHealth--;
            }

            // this is where the destruction happens
            else if (m_BarricadeHealth <= 0)
            {
                if (m_DamageLevel == 0) // first damage level, change to damaged barricade
                {
                    if (m_NewBarricadeDamaged)
                        tempBarricade = Instantiate(m_NewBarricadeDamaged, currentBarricade.transform.position, currentBarricade.transform.rotation);

                    m_DamageLevel++;
                }
                else if (m_DamageLevel == 1) // second damage level, change to destroyed
                {
                    if (m_NewBarricadeDestroyed)
                        tempBarricade = Instantiate(m_NewBarricadeDestroyed, currentBarricade.transform.position, currentBarricade.transform.rotation);

                    m_DamageLevel++;
                }

                // this is for normal and damaged
                if (currentBarricade.transform.childCount == 0)
                {
                    Destroy(currentBarricade);
                }
                // this is for destroyed
                else
                {
                    for (int i = 0; i < currentBarricade.transform.childCount; i++)
                    {
                        GameObject m_Child = currentBarricade.transform.GetChild(i).gameObject;
                        m_Child.AddComponent<Rigidbody>();
                        Destroy(currentBarricade, 5);

                    }

                    Destroy(this.gameObject); // destroy this gameobject since not needed anymore
                }


                currentBarricade = tempBarricade;
                m_BarricadeHealth = 2;
            }
        }
    }
}
