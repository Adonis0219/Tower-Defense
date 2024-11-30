using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed : Enemy
{
    [SerializeField]
    float killedCoin = 2;

    protected override void OnDead()
    {
        // Coin »πµÊ
        GameManager.instance.GoodsFactor(killedCoin, this.transform, false);
        // æ»ø° Dollar »πµÊ ¿÷¿Ω
        base.OnDead();
    }
}
