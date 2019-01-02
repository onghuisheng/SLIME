using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChuteLever : StationaryObject
{

    public override bool hideControllerOnGrab { get { return true; } }

    float m_DefaultLocalY;
    const float m_ChuteCloseTime = 3;

    float m_LastControllerY;

    // Use this for initialization
    void Start()
    {
        m_DefaultLocalY = transform.localPosition.y;
    }

    public override void OnGrab(MoveController currentController)
    {
        base.OnGrab(currentController);
        m_LastControllerY = currentController.transform.position.y;
        StopAllCoroutines();
    }

    public override void OnGrabStay(MoveController currentController)
    {
        base.OnGrabStay(currentController);

        if (transform.localPosition.y >= m_DefaultLocalY && transform.localPosition.y <= 2)
        {
            float moveDelta = currentController.transform.position.y - m_LastControllerY;
            transform.Translate(0, moveDelta, 0, Space.Self);
            m_LastControllerY = currentController.transform.position.y;

            Vector3 clampPos = transform.localPosition;
            clampPos.y = Mathf.Clamp(clampPos.y, m_DefaultLocalY, 2);
            transform.localPosition = clampPos;
        }
    }

    public override void OnGrabReleased(MoveController currentController)
    {
        base.OnGrabReleased(currentController);
        StopAllCoroutines();
        // Uncomment to drop chute down
        // StartCoroutine(CloseChute(m_ChuteCloseTime));
    }

    private IEnumerator CloseChute(float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.DOLocalMoveY(m_DefaultLocalY, 1).SetSpeedBased().SetEase(Ease.OutExpo);
    }

}
