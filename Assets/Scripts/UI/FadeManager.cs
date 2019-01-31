using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
    [SerializeField]
    private UnityEngine.UI.Image _image;

    private bool _isFading = false;
    public bool IsFading {
        get { return _isFading; }
        private set { _isFading = value; }
    }

    public Color _fadeColor = Color.black;
    private Color _defoColor = Color.black;
    public Color ColorReset()
    {
        return _fadeColor = _defoColor;
    }

    private void Awake()
    {
        _defoColor = _fadeColor;
    }

    public IEnumerator FadeIn(float interval)
    {
        this.IsFading = true;
        _image.color = _fadeColor;

        var tween = _image.DOFade(1, interval);
        yield return tween.WaitForCompletion();

        this.IsFading = false;
    }

    public IEnumerator FadeIn(float interval, Action callback)
    {
        yield return StartCoroutine(FadeIn(interval));

        if (callback != null)
        {
            callback();
        }
    }

    public IEnumerator FadeOut(float interval, float delay = 0, Action onComplete = null)
    {
        yield return new WaitForSeconds(delay);

        this.IsFading = true;

        var tween = _image.DOFade(0, interval);
        yield return tween.WaitForCompletion();

        if (onComplete != null)
            onComplete.Invoke();

        this.IsFading = false;
    }

    public IEnumerator Fading(float inTime, float outTime, Action callback = null)
    {
        yield return StartCoroutine(FadeIn(inTime, callback));
        yield return StartCoroutine(FadeOut(outTime));
    }
}