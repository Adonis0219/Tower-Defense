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
    float maxHp;
    
    float MaxHp
    {
        get { return maxHp; }
        set
        {
            maxHp = value;
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

    [SerializeField]
    float collDamage;       // �浹 ������

    public void Hit(float damage)
    {
        CurrentHp -= damage;
    }

    private void OnEnable()
    {
        CurrentHp = MaxHp;
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

    /// <summary>
    /// ���̺갡 ������ ü���� ������ŭ �����ִ� �Լ�
    /// </summary>
    /// <param name="factor">������ ����</param>
    public void WaveHpFactor(float factor)
    {
        CurrentHp *= factor;
    }

    public void WaveDamFactor(float factor)
    {
        collDamage *= factor;
    }

    protected virtual void OnDead()
    {
        // Dollar ȹ��
        GameManager.instance.GoodsFactor(killedDollar, this.transform, true);
        ReturnPool();
    }
}
