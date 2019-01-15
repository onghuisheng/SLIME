using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeLookCamera : MonoBehaviour
{

    [SerializeField, Range(1,10)]
    private float m_Sensitivity = 3;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Mouse1))
        {
            transform.Rotate(Input.GetAxis("Mouse Y") * m_Sensitivity, -Input.GetAxis("Mouse X") * m_Sensitivity, 0);

            var angles = transform.eulerAngles;
            angles.z = 0;

            transform.eulerAngles = angles;
        }

    }

}
