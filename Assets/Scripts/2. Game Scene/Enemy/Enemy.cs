using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : PoolObject, IHit
{
    [SerializeField]
    protected float killedDollar = 1;

    [SerializeField]
    public float moveSpd;

    [SerializeField]
    float baseMaxHp;
    
    float BaseMaxHp
    {
        get { return baseMaxHp; }
        set
        {
            baseMaxHp = value;
        }
    }

    [SerializeField]
    float currentHp;

    float CurrentHp
    {
        get { return currentHp; }
        set
        {
            currentHp = value;

            if (currentHp <= 0)
            {
                OnDead();
            }
        }
    }
    
    /// <summary>
    /// 기본 충돌 데미지
    /// </summary>
    [SerializeField]
    float baseCollDamage;

    float collDamage;

    public void Hit(float damage)
    {
        CurrentHp -= damage;
    }

    private void OnEnable()
    {
        // 소환될 때 배수만큼 곱해주기
        CurrentHp = BaseMaxHp * GameManager.instance.waveHpFactor;
        collDamage = baseCollDamage * GameManager.instance.waveDmgFactor;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * moveSpd * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IHit hitObj = collision.gameObject.GetComponent<IHit>();

        if (hitObj != null)
        {
            StartCoroutine(hitObj.OnCollisionHit(collDamage));
        }
    }

    public IEnumerator OnCollisionHit(float damage)
    {
        yield return null;
    }

    protected virtual void OnDead()
    {
        // Dollar 획득
        GameManager.instance.GoodsFactor(killedDollar, this.transform, true);
        ReturnPool();
    }
}
