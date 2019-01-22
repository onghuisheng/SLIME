using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolleyFirer : MonoBehaviour
{

    public ArrowBase m_ArrowPrefab;

    public GameObject m_ArtilleryPrefab;

    public Transform[] m_Destinations;

    [Range(1.0f, 60.0f)]
    public float m_Interval = 5.0f;

    [Range(0, 60)]
    public float m_DelayedStart = 1;

    [Range(0, 3)]
    public float m_DelayedStartRandom = 2;

    [Range(1, 5)]
    public int m_ArrowQuantity = 1;

    [Range(1.0f, 10.0f)]
    public float m_ArrowTravelDuration = 1;

    [Range(1, 5)]
    public float m_RandomRadius = 2;

    private bool m_IsBombing = false;

    private void Start()
    {
        StartCoroutine(FireArrowRoutine());
    }

    IEnumerator FireArrowRoutine()
    {
        yield return new WaitForSeconds(m_DelayedStart + Random.Range(-m_DelayedStartRandom, m_DelayedStartRandom));

        while (true)
        {
            AudioManager.Instance.Play3D("bowpull", transform.position, AudioManager.AudioType.Additive);

            yield return new WaitForSeconds(1);

            for (int i = 0; i < m_ArrowQuantity; ++i)
            {
                FireArrow();
                yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
            }

            yield return new WaitForSeconds(m_Interval);
        }
    }

    public void FireArrow()
    {
        var arrow = Instantiate(m_ArrowPrefab, transform.position, Quaternion.identity, transform);
        arrow.BuffArrow(ArrowBase.ArrowType.FlameVolley);

        var pos = m_Destinations[Random.Range(0, m_Destinations.Length - 1)].transform.position;
        pos.x += Random.Range(-m_RandomRadius, m_RandomRadius);
        pos.z += Random.Range(-m_RandomRadius, m_RandomRadius);

        arrow.LaunchArrowWithArc(pos, m_ArrowTravelDuration);

        AudioManager.Instance.Play3D("arrowwhoosh", transform.position, AudioManager.AudioType.Additive);
    }

    public void FireArtillery()
    {
        if (!m_IsBombing)
        {
            m_IsBombing = true;
            StartCoroutine(ArtilleryRoutine(Random.Range(7, 15)));
        }
    }

    private IEnumerator ArtilleryRoutine(int bombCount)
    {
        const float time = 3;
        int bombsLeft = bombCount;

        while (bombsLeft > 0)
        {
            var projectile = Instantiate(m_ArtilleryPrefab, transform.position, Quaternion.identity, transform);

            float radius = 4;

            var pos = m_Destinations[Random.Range(0, m_Destinations.Length - 1)].transform.position;
            pos.x += Random.Range(-m_RandomRadius * radius, m_RandomRadius * radius);
            pos.z += Random.Range(-m_RandomRadius * radius, m_RandomRadius * radius);

            // finding the distance
            Vector3 distance = pos - transform.position;
            Vector3 distanceXZ = distance;
            distanceXZ.y = 0;

            float sY = distance.y; // vertical dist
            float sXZ = distanceXZ.magnitude;  // horizontal dist

            float velocityX = sXZ / time;
            float velocityY = sY / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

            Vector3 result = distanceXZ.normalized;
            result *= velocityX;
            result.y = velocityY;
            projectile.GetComponent<Rigidbody>().AddForce(result, ForceMode.VelocityChange);

            AudioManager.Instance.Play3D("whee", transform.position, AudioManager.AudioType.Additive);

            bombsLeft--;

            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
        }

        m_IsBombing = false;
    }

}
