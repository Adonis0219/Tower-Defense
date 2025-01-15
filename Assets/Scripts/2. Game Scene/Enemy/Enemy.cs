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
    
    public float BaseMaxHp
    {
        get { return baseMaxHp; }
        set
        {
            baseMaxHp = value;
        }
    }

    public float maxHp;

    [SerializeField]
    float currentHp;

    public float CurrentHp
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
        maxHp = BaseMaxHp * GameManager.instance.waveHpFactor;
        // 소환될 때 배수만큼 곱해주기
        CurrentHp = maxHp;
        collDamage = baseCollDamage * GameManager.instance.waveDmgFactor;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * moveSpd * Time.deltaTime);
    }

    IHit hitObj;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hitObj = collision.gameObject.GetComponent<IHit>();

        if (hitObj != null) StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        while (gameObject.activeSelf)
        {
            hitObj.Hit(collDamage);
            // 가시 반사 대미지
            CurrentHp -= maxHp * (GameManager.instance.player.ThronsPer * .01f);

            yield return new WaitForSeconds(2f);
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

    public IEnumerator OnCollisionHit(float damage, Collision2D enemy)
    {
        throw new System.NotImplementedException();
    }
}
