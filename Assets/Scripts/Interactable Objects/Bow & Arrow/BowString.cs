using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowString : GrabbableObject
{

    [SerializeField]
    private ArrowBase m_BowArrowPrefab;

    [SerializeField]
    private Transform m_ArrowFacingPoint;

    private bool m_IsGrabbing = false;

    private Vector3 m_DefaultLocalPos;

    private Vector3 m_LastLocalPos;

    private Vector3 m_InitialOffset;

    private Vector3 m_CurrentDrawDistance;

    private MoveController m_CurrentController, m_OtherController;

    private ArrowBase m_SpawnedArrow = null;

    #region Bow Aesthetic Stuff
    [SerializeField]
    private Transform m_UpperBowLimb1, m_UpperBowLimb2, m_LowerBowLimb1, m_LowerBowLimb2;

    [SerializeField]
    private Material m_ArrowDisabledMaterial;

    private Material m_Arrow1DefaultMaterial, m_Arrow2DefaultMaterial;

    private Quaternion m_UpperBowLimb1_DefaultRot, m_UpperBowLimb2_DefaultRot, m_LowerBowLimb1_DefaultRot, m_LowerBowLimb2_DefaultRot;

    // Rate the bow bends when drawing arrows
    private const float m_OuterLimbRotationRate = 180;
    private const float m_InnerLimbRotationRate = 180;

    private float m_RemainingVibrateDistance = 0;

    private float m_OriginalColliderRadius;

    MeshRenderer m_ArrowMeshRenderer1 = null, m_ArrowMeshRenderer2 = null;
    #endregion


    private void Awake()
    {
        m_Arrow1DefaultMaterial = m_BowArrowPrefab.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial;
        m_Arrow2DefaultMaterial = m_BowArrowPrefab.transform.GetChild(1).GetComponent<MeshRenderer>().sharedMaterial;

        m_DefaultLocalPos = transform.localPosition;
        m_UpperBowLimb1_DefaultRot = m_UpperBowLimb1.localRotation;
        m_UpperBowLimb2_DefaultRot = m_UpperBowLimb2.localRotation;
        m_LowerBowLimb1_DefaultRot = m_LowerBowLimb1.localRotation;
        m_LowerBowLimb2_DefaultRot = m_LowerBowLimb2.localRotation;

        m_OriginalColliderRadius = GetComponent<SphereCollider>().radius;
    }

    public override void OnGrab(MoveController currentController)
    {
        m_InitialOffset = currentController.transform.position - transform.position;
        m_CurrentController = currentController;
        m_OtherController = currentController.GetOtherController();
        GetComponent<SphereCollider>().radius = m_OriginalColliderRadius * 10;
    }

    public override void OnGrabStay(MoveController currentController)
    {
        base.OnGrabStay(currentController);

        GetComponent<Rigidbody>().position = currentController.transform.position - m_InitialOffset;
        m_IsGrabbing = true;
        BendBow();

        if (m_SpawnedArrow == null && transform.localPosition.y > (m_DefaultLocalPos.y + 0.15f))
        {
            m_SpawnedArrow = Instantiate(m_BowArrowPrefab.gameObject, transform).GetComponent<ArrowBase>();
            m_ArrowMeshRenderer1 = m_SpawnedArrow.transform.GetChild(0).GetComponent<MeshRenderer>();
            m_ArrowMeshRenderer2 = m_SpawnedArrow.transform.GetChild(1).GetComponent<MeshRenderer>();
            AudioManager.Instance.Play3D("bowpull", transform.position, AudioManager.AudioType.Additive);
        }

        if (m_SpawnedArrow != null)
        {
            // Check if the angle is screwed up, change material if it is
            if (IsBowStringFirable())
            {
                m_ArrowMeshRenderer1.material = m_Arrow1DefaultMaterial;
                m_ArrowMeshRenderer2.material = m_Arrow2DefaultMaterial;
            }
            else
            {
                m_ArrowMeshRenderer1.material = m_ArrowDisabledMaterial;
                m_ArrowMeshRenderer2.material = m_ArrowDisabledMaterial;
            }
        }
    }

    public override void OnGrabReleased(MoveController currentController)
    {
        m_IsGrabbing = false;

        float arrowStrength = m_CurrentDrawDistance.magnitude;

        if (m_SpawnedArrow != null)
        {
            if (IsBowStringFirable())
            {
                // Launch the arrow, multiply the strength a bit?
                m_SpawnedArrow.LaunchArrow(arrowStrength * 75);
                AudioManager.Instance.Play3D("arrowwhoosh", transform.position, AudioManager.AudioType.Additive);
            }
            else
            {
                // Destroy the arrow if we cant fire it
                Destroy(m_SpawnedArrow.gameObject);
            }

            m_SpawnedArrow = null;
        }

        transform.localRotation = Quaternion.Euler(90, 0, 0);
        GetComponent<SphereCollider>().radius = m_OriginalColliderRadius;
    }

    private void Update()
    {
        // If Player is not pulling the string, fling it back to the origin
        if (!m_IsGrabbing)
        {
            if (transform.localPosition != m_DefaultLocalPos)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, m_DefaultLocalPos, Time.deltaTime * 10);

                // When the string has fully recovered
                if (transform.localPosition == m_DefaultLocalPos)
                    m_CurrentDrawDistance = Vector3.zero;
            }

            UnbendBow();
        }
        else // Player is pulling the bow string
        {
            var newDrawDistance = (transform.localPosition + m_InitialOffset) - m_DefaultLocalPos;

            if (transform.localPosition.y > m_LastLocalPos.y)
                m_RemainingVibrateDistance += (m_CurrentDrawDistance - newDrawDistance).magnitude;

            if (m_RemainingVibrateDistance > 0.075f) // Threshold
            {
                m_RemainingVibrateDistance = 0;
                m_CurrentController.Vibrate(105, 0.2f); // Change this to adjust controller vibration
                m_OtherController.Vibrate(75, 0.2f);
            }

            m_CurrentDrawDistance = newDrawDistance;
        }

        if (m_SpawnedArrow != null)
        {
            // Constantly face towards the pivot point when an arrow is drawn
            ResetArrowOrientation();
        }
    }

    private void LateUpdate()
    {
        m_LastLocalPos = transform.localPosition;
    }

    // Reset the spawned arrow to make it face the pivot
    private void ResetArrowOrientation()
    {
        m_SpawnedArrow.transform.localPosition = Vector3.zero;
        m_SpawnedArrow.transform.LookAt(m_ArrowFacingPoint.transform.position);
        m_SpawnedArrow.transform.Rotate(90, 0, 0);
    }

    // Reset the bow's bend over time
    private void UnbendBow()
    {
        if (m_UpperBowLimb1.localRotation != m_UpperBowLimb1_DefaultRot)
        {
            m_UpperBowLimb1.localRotation = Quaternion.RotateTowards(m_UpperBowLimb1.localRotation, m_UpperBowLimb1_DefaultRot, m_OuterLimbRotationRate * Time.deltaTime);
            m_LowerBowLimb1.localRotation = Quaternion.RotateTowards(m_LowerBowLimb1.localRotation, m_LowerBowLimb1_DefaultRot, m_OuterLimbRotationRate * Time.deltaTime);
        }

        if (m_UpperBowLimb2.localRotation != m_UpperBowLimb2_DefaultRot)
        {
            m_UpperBowLimb2.localRotation = Quaternion.RotateTowards(m_UpperBowLimb2.localRotation, m_UpperBowLimb2_DefaultRot, m_InnerLimbRotationRate * Time.deltaTime);
            m_LowerBowLimb2.localRotation = Quaternion.RotateTowards(m_LowerBowLimb2.localRotation, m_LowerBowLimb2_DefaultRot, m_InnerLimbRotationRate * Time.deltaTime);
        }
    }

    // Bend the bow, arc is determined by the horizontal draw distance
    private void BendBow()
    {
        float bendAngle = GetHorizontalDrawDistance() * 25;

        if (bendAngle <= 0)
            return;

        bendAngle = Mathf.Clamp(bendAngle, 0, 20);

        m_UpperBowLimb1.localRotation = m_UpperBowLimb1_DefaultRot;
        m_UpperBowLimb1.Rotate(-bendAngle, 0, 0, Space.Self);
        m_UpperBowLimb2.localRotation = m_UpperBowLimb2_DefaultRot;
        m_UpperBowLimb2.Rotate(-bendAngle / 2, 0, 0, Space.Self);
        m_LowerBowLimb1.localRotation = m_LowerBowLimb1_DefaultRot;
        m_LowerBowLimb1.Rotate(bendAngle, 0, 0, Space.Self);
        m_LowerBowLimb2.localRotation = m_LowerBowLimb2_DefaultRot;
        m_LowerBowLimb2.Rotate(bendAngle / 2, 0, 0, Space.Self);
    }

    private float GetHorizontalDrawDistance()
    {
        return transform.localPosition.y;
    }

    private bool IsBowStringFirable()
    {
        float angle = Vector3.Angle((transform.parent.TransformPoint(m_DefaultLocalPos)) - m_ArrowFacingPoint.position, transform.position - m_ArrowFacingPoint.position);
        return (angle < 45) && (transform.localPosition.y > m_DefaultLocalPos.y);
    }

}
