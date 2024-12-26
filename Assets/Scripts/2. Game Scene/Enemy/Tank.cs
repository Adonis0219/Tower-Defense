using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Enemy
{
    [SerializeField]
    float killedCoin = 4;

    protected override void OnDead()
    {
        // Coin ȹ��
        GameManager.instance.GoodsFactor(killedCoin, this.transform, false);
        // �ȿ� Dollar ȹ�� ����
        base.OnDead();
    }
}
