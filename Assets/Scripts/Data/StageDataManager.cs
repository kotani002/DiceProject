using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using LittleEmperor.StageData;
using DG.Tweening;

public class StageDataManager : SingletonMonoBehaviour<StageDataManager>
{
    [SerializeField]
    private List<StageData> stageDataList = new List<StageData>();
    public List<StageData> StageDataList => stageDataList;

    public int GameTotalScore = 0;

    [SerializeField]
    private TextMeshProUGUI turnText = null;
    [SerializeField]
    private TextMeshProUGUI targetValueText = null;
    [SerializeField]
    private TextMeshProUGUI nowValueText = null;
    [SerializeField]
    private TextMeshProUGUI averageValueText = null;
    [SerializeField]
    private TextMeshProUGUI maxValueText = null;
    [SerializeField]
    private TextMeshProUGUI minValueText = null;
    [SerializeField]
    private TextMeshProUGUI coinValueText = null;

    private StageData nowStageData = null;
    public StageData NowStageData => nowStageData;

    public async UniTask SetUp()
    {
        turnText.text = "<mspace=1.1em>5</mspace>";
        NowStageData.ProgressTurn = NowStageData.MaxTurn;
        targetValueText.text = NowStageData.ClearScore.ToString();
        coinValueText.text = "0";
    }

    public void SetNowStageData(int i)
    {
        nowStageData = stageDataList[i];
    }

    public void StatusUpdate()
    {
        int maxValue = 0;
        int minValue = 0;

        DiceManager.Instance.RollDiceList.ForEach((diceData) =>
        {
            maxValue += diceData.Max;
            minValue += diceData.Min;
        });

        int nowNumber = int.Parse(maxValueText.text);
        int updateNumber = maxValue;

        DOTween.To(() => nowNumber, x => nowNumber = x, updateNumber, 0.5f)
                .OnUpdate(() =>
                {
                    maxValueText.text = nowNumber.ToString("#,0");
                })
                .SetEase(Ease.OutQuad);
                

        int minNowNumber = int.Parse(minValueText.text);
        int minUpdateNumber = minValue;

        DOTween.To(() => minNowNumber, y => minNowNumber = y, minUpdateNumber, 0.5f)
                .OnUpdate(() =>
                {
                    minValueText.text = minNowNumber.ToString("#,0");
                })
                .SetEase(Ease.OutQuad);
    }

    public bool ProgressTurnOverCheck()
    {
        NowStageData.ProgressTurn--;
        turnText.text = NowStageData.ProgressTurn.ToString();
        TextAnimation(turnText.gameObject);

        if (NowStageData.ProgressTurn <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TotalScoreAnimation()
    {

        int nowNumber = int.Parse(nowValueText.text);
        int updateNumber = GameTotalScore;

        DOTween.To(() => nowNumber, x => nowNumber = x, updateNumber, 0.5f)
                .OnUpdate(() =>
                {
                    nowValueText.text = nowNumber.ToString("#,0");
                })
                .SetEase(Ease.OutQuad);

        TextAnimation(nowValueText.gameObject);
    }

    private void TextAnimation(GameObject gameObject)
    {
        gameObject.transform.localScale = Vector3.one * 0.2f;
        gameObject.transform.DOScale(1f, 0.6f)
            .SetEase(Ease.OutBack, 5f);
    }
}
