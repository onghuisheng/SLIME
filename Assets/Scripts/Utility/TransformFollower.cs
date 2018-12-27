using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFollower : MonoBehaviour
{

    [SerializeField]
    private Transform m_Target;

    [SerializeField]
    private bool m_FollowX, m_FollowY, m_FollowZ, m_FollowRotX, m_FollowRotY, m_FollowRotZ;

    [SerializeField]
    private Vector3 m_PositionOffset, m_LocalRotationOffset;

    [SerializeField]
    private bool m_OffsetTowardsCamera;

    [SerializeField]
    private float m_OffsetTowardsCameraDistance;

    MoveController m_Controller1;

    private void Awake()
    {
        m_Controller1 = FindObjectOfType<MoveController>();
    }

    private void FixedUpdate()
    {
        if (m_Target != null)
        {
            Vector3 newPos = transform.position;
            Vector3 newRot = transform.localRotation.eulerAngles;

            if (m_FollowX)
                newPos.x = m_Target.transform.position.x + m_PositionOffset.x;
            if (m_FollowY)
                newPos.y = m_Target.transform.position.y + m_PositionOffset.y;
            if (m_FollowZ)
                newPos.z = m_Target.transform.position.z + m_PositionOffset.z;
            if (m_FollowRotX)
                newRot.x = m_Target.transform.localRotation.eulerAngles.x + m_LocalRotationOffset.x;
            if (m_FollowRotY)
                newRot.y = m_Target.transform.localRotation.eulerAngles.y + m_LocalRotationOffset.y;
            if (m_FollowRotZ)
                newRot.z = m_Target.transform.localRotation.eulerAngles.z + m_LocalRotationOffset.z;

            transform.position = newPos;
            transform.localRotation = Quaternion.Euler(newRot);

            if (m_Controller1)
            {
                if (m_Controller1.GetButtonDown(MoveControllerButton.Circle))
                {
                    m_PositionOffset.x -= 0.025f;
                    Debug.LogFormat("X: {0} , Z: {1}", m_PositionOffset.x, m_PositionOffset.z);
                }
                if (m_Controller1.GetButtonDown(MoveControllerButton.Triangle))
                {
                    m_PositionOffset.z -= 0.025f;
                    Debug.LogFormat("X: {0} , Z: {1}", m_PositionOffset.x, m_PositionOffset.z);
                }

                if (m_Controller1.GetButtonDown(MoveControllerButton.X))
                {
                    m_PositionOffset.x += 0.025f;
                    Debug.LogFormat("X: {0} , Z: {1}", m_PositionOffset.x, m_PositionOffset.z);
                }
                if (m_Controller1.GetButtonDown(MoveControllerButton.Square))
                {
                    m_PositionOffset.z += 0.025f;
                    Debug.LogFormat("X: {0} , Z: {1}", m_PositionOffset.x, m_PositionOffset.z);
                }
                if (m_Controller1.GetButtonDown(MoveControllerButton.MiddleButton))
                {
                    m_PositionOffset.y += 0.025f;
                    Debug.LogFormat("X: {0} , Y: {1} , Z: {2}", m_PositionOffset.x, m_PositionOffset.y, m_PositionOffset.z);
                }
                if (m_Controller1.GetButtonDown(MoveControllerButton.Start))
                {
                    m_PositionOffset.y -= 0.025f;
                    Debug.LogFormat("X: {0} , Y: {1} , Z: {2}", m_PositionOffset.x, m_PositionOffset.y, m_PositionOffset.z);
                }

            }

        }
    }

}