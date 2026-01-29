using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollDiceArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        BaseDice dice = other.gameObject.GetComponent<BaseDice>();
        if(dice == null)
        {
            return;
        }
        DiceManager.Instance.SelectRollDice(dice);

        StageDataManager.Instance.StatusUpdate();
    }
}
