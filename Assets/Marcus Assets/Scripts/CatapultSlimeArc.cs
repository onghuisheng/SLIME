using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultSlimeArc : MonoBehaviour {

    public GameObject m_CatapultSlime;
    public GameObject m_StartingObject;
    public GameObject m_TargetObject;
    public List<GameObject> m_Slime;

    public enum Color
    {
        None = 0,
        Blue,
        Purple,
    }

    public Color m_ColorOfMarker = Color.None;

    // Use this for initialization
    void Start () {

		if(m_ColorOfMarker == Color.Blue)
        {
            m_TargetObject = GameObject.FindGameObjectWithTag("BlueDrop");
        }
        else if (m_ColorOfMarker == Color.Purple)
        {
            m_TargetObject = GameObject.FindGameObjectWithTag("PurpleDrop");
        }


    }

    // Update is called once per frame
    void Update () {
		
	}

    public void ShootSlime()
    {
        Vector3 velocity = CalculateVelocity(m_StartingObject.transform.position, m_TargetObject.transform.position, 2f);

        //m_CatapultSlime.transform.rotation = Quaternion.LookRotation(velocity);
        m_CatapultSlime.transform.LookAt(m_TargetObject.transform.position);
        
        Random.InitState((int)System.DateTime.Now.Ticks); // make random more random
        int m_RandomSlime = Random.Range(0, m_Slime.Count);

        GameObject temp = Instantiate(m_Slime[m_RandomSlime], m_StartingObject.transform.position, Quaternion.identity);
        temp.GetComponent<Rigidbody>().velocity = velocity;

    }

    Vector3 CalculateVelocity(Vector3 Start, Vector3 End, float Time)
    {
        // finding the distance
        Vector3 distance = End - Start;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0;

        float sY = distance.y; // vertical dist
        float sXZ = distanceXZ.magnitude;  // horizontal dist

        float velocityX = sXZ / Time;
        float velocityY = sY / Time + 0.5f * Mathf.Abs(Physics.gravity.y) * Time;

        Vector3 Result = distanceXZ.normalized;
        Result *= velocityX;
        Result.y = velocityY;

        return Result;
    }
}
