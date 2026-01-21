using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;
using LittleEmperor.GamePrefabsData;
using LittleEmperor.TurnStateName.GamePrefabName;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    private List<GamePrefabs> gamePrefabList = null;
    private GamePrefabState gamePrefabState = GamePrefabState.Title;

    void Start()
    {
        GameMainCycle();
    }

    private async UniTask GameMainCycle()
    {
        ChangeTitleGameState();
        await ChangeGamePrefab();
        var animationManager = AnimationManager.Instance;

        //ステートがタイトルで無い時
        await UniTask.WaitUntil(() => gamePrefabState != GamePrefabState.Title);
        await animationManager.GetFadeAnimation.FadeIn();
        await ChangeGamePrefab();
        await animationManager.GetFadeAnimation.FadeOut();

        //ステートがタイトルで無い時
        await UniTask.WaitUntil(() => gamePrefabState != GamePrefabState.StageSelect);
        await animationManager.GetFadeAnimation.FadeIn();
        AnimationManager.Instance.WindowShowDown();
        await ChangeGamePrefab();
        
        //メインゲームのルールを動作させる
        DiceGameManager.Instance.DiceGame();
        await animationManager.GetFadeAnimation.FadeOut();
        AnimationManager.Instance.GameStartAnimation();

    }

    private async UniTask ChangeGamePrefab()
    {
        gamePrefabList.ForEach(gamePrefab =>
        {
            PrefabsSetActive(gamePrefab,false);
        });
        PrefabsSetActive(gamePrefabList[(int)gamePrefabState],true);
    }

    /// <summary>
    /// メインゲームのステートを変更する
    /// </summary>
    /// <param name="state"></param>
    public void ChangeTitleGameState()
    {
        gamePrefabState = GamePrefabState.Title;
    }

    public void ChangeStageSelectGameState()
    {
        gamePrefabState = GamePrefabState.StageSelect;
    }
    
    public void ChangeStage1State()
    {
        gamePrefabState = GamePrefabState.MainGame;
    }

    private void PrefabsSetActive(GamePrefabs prefabs, bool flag)
    {
        foreach (var prefab in prefabs.gameObjects)
        {
            prefab.SetActive(flag);
        }
    }
}
