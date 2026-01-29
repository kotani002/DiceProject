using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;
using TMPro;

public class DiceRollButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool diceRollFlag = false;
    [SerializeField]
    private TextMeshProUGUI rollText = null;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (diceRollFlag == false)
        {
            RollAnime();
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {

    }

    private async UniTask RollAnime()
    {
        diceRollFlag = true;
        DiceManager.Instance.RollAllDice();
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        DiceManager.Instance.StopAllDice();
        diceRollFlag = false;
    }
}
