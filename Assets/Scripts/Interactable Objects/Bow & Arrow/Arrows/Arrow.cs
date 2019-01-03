using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Arrow : ArrowBase
{
    private GameObject m_ArrowModel;

    float m_DissolveDistance = 0;

    bool m_Swapped = false;

    private void Start()
    {
        m_ArrowModel = transform.GetChild(0).gameObject;
        m_ArrowModel.GetComponent<Renderer>().material.SetFloat("_Distance", 0);
    }

    private void Update()
    {
        if (!m_Swapped)
        {
            m_DissolveDistance = Mathf.Clamp(m_DissolveDistance + Time.deltaTime * 15, 0, 15);
            m_ArrowModel.GetComponent<Renderer>().material.SetFloat("_Distance", m_DissolveDistance);

            if (m_DissolveDistance >= 15)
            {
                // Quick swap
                Transform realModel = transform.GetChild(1);
                m_ArrowModel.transform.GetChild(0).parent = realModel;
                realModel.gameObject.SetActive(true);
                m_ArrowModel.SetActive(false);
                m_Swapped = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_IsFired)
            return;

        IShootable shootable = other.GetComponent<IShootable>();

        if (shootable != null)
        {
            transform.parent = other.transform; //set as parent before calling onshot, if anything goes wrong, shift this line below OnShot HAHAH
            shootable.OnShot(this); // Interface callback
        }

        DestroyArrow(); // If collide with environment, remove this arrow after X seconds
    }

}