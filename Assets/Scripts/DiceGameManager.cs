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
    private InGameTurnState nowState = InGameTurnState.SetUp;

    Vector2 test = new Vector2();

    private Vector3 _nowMousePosi = new Vector3(0,0,100); // 現在のマウスのワールド座標

    // Stateパターンの練習
    // Stateが切り替わるまでwaitで待機する形式でターン処理を進める
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //RaycastHit hit;
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "Dice")
                {
                    var vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    vec.z = 1f;
                    // 現在のマウスのワールド座標を更新
                    hit.collider.gameObject.transform.position = vec;
                }
            }
        }
    }

    public async void DiceGame()
    {
        // セットアップ終了(Start)まで待機
        // 目標値の表示とターン数の表示
        await UniTask.WaitUntil(() => nowState == InGameTurnState.Start);

        // DiceSelectまで待機
        await UniTask.WaitUntil(() => nowState == InGameTurnState.DiceSelect);

        // リザルト終了まで表示
        await UniTask.WaitUntil(() => nowState == InGameTurnState.Result);
    }

    public void StateDiceSelectChange()
    {
        nowState = InGameTurnState.DiceSelect;
    }

}
