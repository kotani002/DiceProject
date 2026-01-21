using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class FadeAnimation
{
    [SerializeField]
    private Image bg = null;

    // 画面全体にフェードをかける
    public async UniTask FadeIn(Action action = null)
    {
        bg.gameObject.SetActive(true);
        await bg.DOFade(endValue: 1f, duration: 0.75f).SetEase(Ease.InCubic);
        if(action != null)
        {
            action();
        }
    }
    public async UniTask FadeOut(Action action = null)
    {
        await bg.DOFade(endValue: 0f, duration: 0.75f).SetEase(Ease.InCubic);
        bg.gameObject.SetActive(false);
        if(action != null)
        {
            action();
        }
    }
}
