using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DiceName;
using Cysharp.Threading.Tasks;
using LittleEmperor.TurnStateName.Enums;

public class DiceManager : SingletonMonoBehaviour<DiceManager>
{
    [SerializeField]
    private List<BaseDice> _rollDiceList = new List<BaseDice>();
    public List<BaseDice> RollDiceList => _rollDiceList;
    [SerializeField]
    private List<BaseDice> _stockDiceList = new List<BaseDice>();
    private List<BaseDice> StockDiceList => _stockDiceList;

    private E_AnimationType e_AnimationType = default;

    //全てのサイコロを一括で振る
    public async void RollAllDice()
    {
        int i = 1;
        int totalScore = 0;
        e_AnimationType = E_AnimationType.Roll;

        RollDiceList.ForEach(async obj =>
        {
            await obj.Roll();
            totalScore += obj.GetScore();
            i++;
        });

        // 全てのダイスが停止したらStateを変更する

        if (RollDiceList.Count != 0)
        {
            await UniTask.WaitUntil(() => i >= RollDiceList.Count);
            Debug.Log($"ダイスの合計値は {totalScore} !");
            StageDataManager.Instance.GameTotalScore += totalScore;
            DiceGameManager.Instance.StateDiceChange(InGameTurnState.Result);
            return;
        }
    }
    //全てのサイコロを一括で止める
    public async void StopAllDice()
    {
        RollDiceList.ForEach(async obj =>
        {
            obj.RollStop();
        });
    }
    public void SelectRollDice(BaseDice baseDice)
    {
        bool checkFlag = false;
        //もし同名のオブジェクトが選択されていた場合は登録しない
        RollDiceList.ForEach(DiceObjet =>
        {
            if (DiceObjet.gameObject.name == baseDice.gameObject.name)
            {
                checkFlag = true;
                return;
            }
        });

        if (checkFlag == false)
        {
            StockDiceList.Remove(baseDice);
            RollDiceList.Add(baseDice);
        }
    }
    public void SelectOutRollDice(BaseDice baseDice)
    {
        bool checkFlag = false;
        //もし同名のオブジェクトが選択されていた場合は登録しない
        StockDiceList.ForEach(DiceObjet =>
        {
            if (DiceObjet.gameObject.name == baseDice.gameObject.name)
            {
                checkFlag = true;
                return;
            }
        });

        if (checkFlag == false)
        {
            StockDiceList.Add(baseDice);
            RollDiceList.Remove(baseDice);
        }
    }

    public void DiceReset()
    {

    }
}
