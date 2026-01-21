using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalDice : BaseDice
{
    public override void Lottery(int value)
    {
        switch (value)
        {
            case 0:
            SetScore(1);
            break;

            case 1:
            SetScore(2);
            break;

            case 2:
            SetScore(3);
            break;

            case 3:
            SetScore(4);
            break;

            case 4:
            SetScore(5);
            break;

            case 5:
            SetScore(6);
            break;

            default:
            SetScore(0);
            break;
        }
    }
}
