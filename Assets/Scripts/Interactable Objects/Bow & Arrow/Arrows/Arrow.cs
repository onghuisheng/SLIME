using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Arrow : ArrowBase
{
    private GameObject m_ArrowModel;

    private Vector4 m_DissolveCenter = new Vector4(0, 0, 90, 0);

    private float m_CurrentDissolveDistance = 0;

    bool m_Swapped = false;

    private void Start()
    {
        m_ArrowModel = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (!m_Swapped)
        {
            const float dissolveRate = 2;

            var m_CurrentMat = m_ArrowModel.GetComponent<Renderer>().material;

            m_DissolveCenter = transform.position;
            m_CurrentMat.SetVector("_Center", m_DissolveCenter);

            m_CurrentDissolveDistance += (Time.deltaTime * dissolveRate);
            m_CurrentMat.SetFloat("_Distance", m_CurrentDissolveDistance);

            if (m_CurrentDissolveDistance >= dissolveRate)
            {
                // Quick swap
                Transform realModel = transform.GetChild(1);
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

        AudioManager.Instance.Play3D("arrowhit", transform.position, AudioManager.AudioType.Additive);
        DestroyArrow(); // If collide with environment, remove this arrow after X seconds
    }

    private void OnDestroy()
    {
        Destroy(m_ArrowModel.GetComponent<Renderer>().material);

    }

}