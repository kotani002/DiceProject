using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LittleEmperor.TurnStateName.Enums;
using Cysharp.Threading.Tasks;
using LittleEmperor.TurnStateName.GamePrefabName;

public class DiceGameManager : SingletonMonoBehaviour<DiceGameManager>
{
    // ゲームの流れを管理するマネージャークラス
    // 1.目標値とターン数の表示
    // 2.ダイスをストックエリアに補充
    // 3.抽選が始まるまで待機
    // 4.抽選が終わったら、リザルト表示
    // 5.ターン終了処理
    [SerializeField]
    private InGameTurnState nowState = InGameTurnState.SetUp;

    public async void DiceGame()
    {
        // セットアップ終了(Start)まで待機
        await SetUp();
        // 目標値の表示とターン数の表示
        await UniTask.WaitUntil(() => nowState == InGameTurnState.Start);

        // DiceSelectまで待機
        await UniTask.WaitUntil(() => nowState == InGameTurnState.DiceSelect);

        // リザルト終了まで表示
        await UniTask.WaitUntil(() => nowState == InGameTurnState.Result);

        StageDataManager.Instance.TotalScoreAnimation();

        //経過ターン数が指定の値を超えた場合
        if(StageDataManager.Instance.ProgressTurnOverCheck())
        {
            //敗北処理
        }else
        {
            //トータルスコアが目標値を超えたか確認
            if(StageDataManager.Instance.GameTotalScore >= StageDataManager.Instance.NowStageData.ClearScore)
            {
                //ステージクリアリザルトへ
            }

            //超えていない場合は次のターンに進行
            DiceGameLoop();
            nowState = InGameTurnState.DiceSelect;
        }
    }

    public async void DiceGameLoop()
    {
        // DiceSelectまで待機
        await UniTask.WaitUntil(() => nowState == InGameTurnState.DiceSelect);

        // リザルト終了まで表示
        await UniTask.WaitUntil(() => nowState == InGameTurnState.Result);
        
        StageDataManager.Instance.TotalScoreAnimation();

        //経過ターン数が指定の値を超えた場合
        if(StageDataManager.Instance.ProgressTurnOverCheck())
        {
            //敗北処理
        }else
        {
            //トータルスコアが目標値を超えたか確認
            if(StageDataManager.Instance.GameTotalScore >= StageDataManager.Instance.NowStageData.ClearScore)
            {
                //ステージクリアリザルトへ
            }

            //超えていない場合は次のターンに進行
            DiceGameLoop();
            nowState = InGameTurnState.DiceSelect;
        }
    }

    public void StateDiceSelectChange()
    {
        nowState = InGameTurnState.DiceSelect;
    }

    public void StateDiceChange(InGameTurnState inGameTurnState)
    {
        nowState = inGameTurnState;
    }

    //ステージのデータをViewに反映させる
    private async UniTask SetUp()
    {
        await StageDataManager.Instance.SetUp();
        nowState = InGameTurnState.Start;
    }

}
