using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BarricadeDestruction : MonoBehaviour
{
    public GameObject m_NewBarricadeDamaged;
    public GameObject m_NewBarricadeDestroyed;

    private int m_DamageLevel;
    private int m_BarricadeHealth;

    public GameObject currentBarricade;
    public GameObject tempBarricade;

    
    public GameObject woodParticles;
    public GameObject dustParticles;

    public GameObject m_Player;

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
            // add wood particles here ****************************
            GameObject tempWood = Instantiate(woodParticles, transform.position, woodParticles.transform.rotation);
            Destroy(tempWood, 5.0f);
            
            // minus health when hp above 0
            if (m_BarricadeHealth > 0) 
            {
                m_BarricadeHealth--;
            }

            // this is where the destruction happens
            else if (m_BarricadeHealth <= 0)
            {
                // add Dust particles here ****************************
                GameObject tempDust = Instantiate(dustParticles, transform.position, dustParticles.transform.rotation);
                Destroy(tempDust, 5.0f);

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
                        //Destroy(currentBarricade, 5);

                    }
                    currentBarricade.GetComponent<Collider>().enabled = false;
                    other.GetComponent<SlimeHitBarricade>().m_Parent.GetComponent<SlimeBase>().anim.SetBool("IsAttack", false);
                    other.GetComponent<SlimeHitBarricade>().m_Parent.GetComponent<NavMeshAgent>().enabled = true;
                    other.GetComponent<SlimeHitBarricade>().m_Parent.GetComponent<Movement>().m_Player = m_Player;

                    Destroy(this.gameObject); // destroy this gameobject since not needed anymore
                }


                currentBarricade = tempBarricade;
                m_BarricadeHealth = 1;
            }
        }
    }
}
