using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilDice : BaseDice
{
    public override void Lottery(int value)
    {
        switch (value)
        {
            case 0:
            SetScore(0);
            break;

            case 1:
            SetScore(0);
            break;

            case 2:
            SetScore(0);
            break;

            case 3:
            SetScore(6);
            break;

            case 4:
            SetScore(6);
            break;

            case 5:
            SetScore(6);
            break;

            default:
            Debug.Log("例外");
            break;
        }
    }
}
