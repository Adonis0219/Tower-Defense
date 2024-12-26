using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Enemy
{
    [SerializeField]
    float killedCoin = 4;

    protected override void OnDead()
    {
        // Coin »πµÊ
        GameManager.instance.GoodsFactor(killedCoin, this.transform, false);
        // æ»ø° Dollar »πµÊ ¿÷¿Ω
        base.OnDead();
    }
}
