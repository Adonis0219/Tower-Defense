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
    /// �⺻ �浹 ������
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
        // ��ȯ�� �� �����ŭ �����ֱ�
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
            // ���� �ݻ� �����
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
        // Dollar ȹ��
        GameManager.instance.GoodsFactor(killedDollar, this.transform, true);
        ReturnPool();
    }

    public IEnumerator OnCollisionHit(float damage, Collision2D enemy)
    {
        throw new System.NotImplementedException();
    }
}
