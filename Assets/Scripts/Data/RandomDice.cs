using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RandomDice : MonoBehaviour
{
    private int value;
    public Text tex = null;

    //乱数の生成
    void Start()
    {
        //現在時刻を参照してシード値を決定(シード値を適当な値にしたい)
        UnityEngine.Random.InitState(DateTime.UtcNow.Millisecond);
    }

    //ダイスの目の決定
    public void SetDice()
    {
        //均等なダイス(1から6までの間で生成)
        value = UnityEngine.Random.Range(1,7);
        //Debug.Log(value);

        //0から99の間で乱数を生成
        value = UnityEngine.Random.Range(0,100);
        Debug.Log($"実際に出た乱数{value}");
        Select();
    }

    //抽選機
    //100分率に変換してサイコロの出目を調整
    void Select()
    {
        //1/100
        if(value <= -1) //乱数で出てきた値が10以下なら 1 と判断
        {
            View(1);
        }
        //1/100
        else if(value <= -1) //乱数で出てきた値が20以下 2 と判断
        {
            View(2);
        }
        //1/100
        else if(value <= -1) //乱数で出てきた値が30以下なら 3 と判断
        {
            View(3);
        }
        //1/100
        else if(value <= -1) //乱数で出てきた値が40以下なら 4 と判断
        {
            View(4);
        }
        //1/100
        else if(value <= -1) //乱数で出てきた値が50以下なら 5 と判断
        {
            View(5);
        }
        //95/100
        else//乱数で出てきた値がその他(51以上)なら 6 と判断
        {
            View(6);
        }
    }

    //結果の表示
    void View(int value)
    {
        tex.text = value.ToString();
        Debug.Log($"ダイスで{value}が出た!");
    }
}
