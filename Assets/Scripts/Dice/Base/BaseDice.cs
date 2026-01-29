using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiceName;
using System;
using DG.Tweening;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;

public class BaseDice : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private int _value = 0;
    private int _score = 0;
    public int Min = 0;
    public int Max = 6;

    [SerializeField]
    private List<Vector3> _diceVector = new List<Vector3>();

    public E_DiceType DiceType;
    private E_AnimationType e_AnimationType = default;

    private bool moveDice = false;

    private void Awake()
    {
        UnityEngine.Random.InitState(DateTime.UtcNow.Millisecond);
    }

    //サイコロを振る
    public async Task Roll()
    {
        //均等なダイス(1から6までの間で生成)
        int value = UnityEngine.Random.Range(0, 6);
        //角度設定
        Quaternion Quo = Quaternion.Euler(_diceVector[value]);
        //Animation再生
        await RollAnimation();
        //ランダム回転+振動
        await this.transform.DORotateQuaternion(Quo, 0.1f);
        Lottery(value);
        //現在の位置から指定の角度に変更
    }

    public async void RollStop()
    {
        e_AnimationType = E_AnimationType.Stop;
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
    }

    //Rollアニメーション
    private async UniTask RollAnimation()
    {
        //アニメーションタイプをRollに変更
        e_AnimationType = E_AnimationType.Roll;
        //Stopされるまで
        await UniTask.WaitUntil(() => e_AnimationType == E_AnimationType.Stop);
    }

    void Update()
    {
        if (e_AnimationType == E_AnimationType.Roll)
        {
            //ダイスをランダムに回す
            RandomRollAnimation();
        }

        if (moveDice)
        {
            var vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            vec.z = 0.9f;
            this.transform.position = vec;
        }
    }

    private void RandomRollAnimation()
    {
        var angle = UnityEngine.Random.Range(0, 360);
        this.transform.Rotate(angle, angle, angle);
    }

    public virtual void Lottery(int value)
    {

    }

    //スコアを返す
    public int GetScore()
    {
        return _score;
    }
    public void SetScore(int value)
    {
        _score = value;
    }

    //ポインターに追従する
    public void OnPointerDown(PointerEventData eventData)
    {
        moveDice = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        var vec = this.transform.position;
        vec.z = 1f;
        this.transform.position = vec;
        moveDice = false;
    }
}
