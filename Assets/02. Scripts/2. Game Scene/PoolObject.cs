using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    [SerializeField]
    protected PoolObejectType poolType;

    // 반환함수
    protected void ReturnPool()
    {
        PoolManager.instance.SetPool(gameObject, poolType);
    }
}
