using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AnimationManager : SingletonMonoBehaviour<AnimationManager>
{
    [SerializeField]
    private WindowAnimation windowAnime = null;
    [SerializeField]
    private FadeAnimation fadeAnimation = null;

    public WindowAnimation GetWindowAnime => windowAnime;
    public FadeAnimation GetFadeAnimation => fadeAnimation;

    bool animationStopFlag = false;

    public async void GameStartAnimation()
    {
        animationStopFlag = true;
        await UniTask.WaitUntil(() => animationStopFlag == false);
        await windowAnime.WindowShowUp();
    }

    public async UniTask WindowShowDown()
    {
        await windowAnime.WindowShowDown();
    }

    public void AnimationFlag()
    {
        animationStopFlag = !animationStopFlag;
    }
}
