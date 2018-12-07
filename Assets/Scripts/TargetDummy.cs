using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TargetDummy : MonoBehaviour, IShootable
{

    Vector3 m_OriginalPos;

    private void Start()
    {
        m_OriginalPos = transform.position;
        transform.DOMoveX(-10, Random.Range(2.0f, 4.0f)).SetSpeedBased(true).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    public void OnShot(ArrowBase arrow)
    {
        transform.DOMoveY(-1, 1).OnComplete(() =>
        {
            transform.DOMoveY(m_OriginalPos.y, 1).SetDelay(3);
        });
    }

}
