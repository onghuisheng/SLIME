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

    private void LateUpdate()
    {
        if (m_Target != null)
        {
            Vector3 newPos = transform.localPosition;
            Vector3 newRot = transform.localRotation.eulerAngles;

            if (m_FollowX)
                newPos.x = m_Target.transform.localPosition.x + m_PositionOffset.x;
            if (m_FollowY)
                newPos.y = m_Target.transform.localPosition.y + m_PositionOffset.y;
            if (m_FollowZ)
                newPos.z = m_Target.transform.localPosition.z + m_PositionOffset.z;
            if (m_FollowRotX)
                newRot.x = m_Target.transform.localRotation.eulerAngles.x + m_LocalRotationOffset.x;
            if (m_FollowRotY)
                newRot.y = m_Target.transform.localRotation.eulerAngles.y + m_LocalRotationOffset.y;
            if (m_FollowRotZ)
                newRot.z = m_Target.transform.localRotation.eulerAngles.z + m_LocalRotationOffset.z;

            transform.localPosition = newPos;
            transform.localRotation = Quaternion.Euler(newRot);

            if (m_OffsetTowardsCamera)
            {
                Vector3 dir = Camera.main.transform.forward;
                dir.y = 0;

                transform.localPosition -= dir * m_OffsetTowardsCameraDistance;
            }
        }
    }

}
