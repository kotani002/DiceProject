using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DiceName;
using Cysharp.Threading.Tasks;

public class DiceManager : SingletonMonoBehaviour<DiceManager>
{
    [SerializeField]
    private List<BaseDice> RollDiceList = new List<BaseDice>();
    [SerializeField]
    private List<BaseDice> StockDiceList = new List<BaseDice>();

    private E_AnimationType e_AnimationType = default;

    //全てのサイコロを一括で振る
    void Start()
    {

    }

    void Update()
    {

    }

    //全てのサイコロを一括で振る
    public async void RollAllDice()
    {
        int i = 1;
        int totalScore = 0;
        e_AnimationType = E_AnimationType.Roll;

        if (RollDiceList.Count == 0)
        {
            return;
        }

        RollDiceList.ForEach(async obj =>
        {
            await obj.Roll();
            totalScore += obj.GetScore();
            i++;
        });

        // TODO:ForEachはUniTaskで待てたはず...
        await UniTask.WaitUntil(() => i >= RollDiceList.Count);
        Debug.Log($"ダイスの合計値は {totalScore} !");
    }
    //全てのサイコロを一括で止める
    public void StopAllDice()
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
