using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


// Ding ding
public class Bell : MonoBehaviour, IShootable
{

    [SerializeField]
    private GameObject m_ChuteCheckerRedPanel;

    [SerializeField, Header("Random items that might fall from ringing the bell")]
    private List<GameObject> m_ItemDropList;

    [SerializeField, Range(1, 60)]
    private int m_RingCooldown = 5;

    [SerializeField]
    private Transform m_ItemDropPosition;

    [SerializeField]
    private Transform m_HandleTransform;

    private Vector3 m_InitialOffset;

    private float m_ChuteCheckerInitialPositionY;

    private float m_NextSpawnTime = 0;

    private float m_NextDingTime = 0;

    private int m_CurrentDingCount = 0;

    private float m_CurrentLatchTime = 0;

    public System.Action OnBellRung;

    private void Start()
    {
        m_ChuteCheckerInitialPositionY = m_ChuteCheckerRedPanel.transform.localPosition.y;
    }

    public void DropItem()
    {
        if (Time.time < m_NextSpawnTime || m_ItemDropList.Count == 0)
            return;

        m_ChuteCheckerRedPanel.transform.DOKill();
        m_ChuteCheckerRedPanel.transform.DOLocalMoveY(-0.1349f, 0.5f).SetEase(Ease.OutExpo);

        AudioManager.Instance.Play3D("chuterelease", m_ItemDropPosition.position, AudioManager.AudioType.Additive);

        // Spawn a random object from the list with a randomized rotation
        foreach (var obj in m_ItemDropList)
        {
            Instantiate(obj, m_ItemDropPosition.position, Quaternion.Euler(Random.Range(0, 360.0f), Random.Range(0, 360.0f), Random.Range(0, 360.0f)));
        }

        m_NextSpawnTime = Time.time + m_RingCooldown;

        m_ChuteCheckerRedPanel.transform.DOLocalMoveY(m_ChuteCheckerInitialPositionY, 0.5f).SetDelay(m_RingCooldown).SetEase(Ease.OutExpo); // move panel back up after cd

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (Time.time > m_NextDingTime)
        {
            Vector3 rVelocity;

            if (collision.transform.IsChildOf(transform.parent))
                rVelocity = m_HandleTransform.GetComponent<Rigidbody>().velocity;
            else
                rVelocity = collision.relativeVelocity;

            AudioManager.Instance.Play3D("ding", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { relativeVelocity = (rVelocity * 2), volume = 0, pitchOverride = 0.8f });
            m_NextDingTime = Time.time + 0.5f;

            m_CurrentDingCount++;

            if (m_CurrentDingCount > 1)
            {
                m_CurrentDingCount = 0;
                DropItem();
            }

            if (OnBellRung != null)
                OnBellRung.Invoke();
        }
    }

    public void OnShot(ArrowBase arrow)
    {
        DropItem();
        AudioManager.Instance.Play3D("ding", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { volume = .5f, pitchOverride = 0.8f });

        if (OnBellRung != null)
            OnBellRung.Invoke();
    }

}