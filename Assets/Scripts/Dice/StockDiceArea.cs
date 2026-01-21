using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockDiceArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        BaseDice dice = other.gameObject.GetComponent<BaseDice>();
        if(dice == null)
        {
            return;
        }
        DiceManager.Instance.SelectOutRollDice(dice);
    }
}
