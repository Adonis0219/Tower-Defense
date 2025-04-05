using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : PoolObject, IHit
{
    [SerializeField]
    protected float killedDollar = 1;

    [SerializeField]
    public float moveSpd;

    [SerializeField]
    float baseMaxHp;
    
    Player player;

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

    ////////// �˹�
    bool isKnockBack = false;
    Rigidbody2D rb;
    WaitForSeconds wait;

    // ���� ���� ����
    bool isSlowed = false;

    
    public void Hit(float damage)
    {
        CurrentHp -= damage;
    }

    private void Awake()
    {
        player = GameManager.instance.player;
        rb = GetComponent<Rigidbody2D>();
        wait = new WaitForSeconds(.1f);
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
        if (isKnockBack) return;

        float range = Vector2.Distance(player.transform.position, this.transform.position);

        // ���ο찡 �� �ɷȰ�, ���� ���� ������ ���ο� ����
        if (!isSlowed && (player.Range / 10 > range))
        {
            CardData card = CardManager.instance.cardDatas[(int)CardID.���ӿ���];

            moveSpd *= (1 - (card.value[card.curLv] / 100));

            isSlowed = true;
        }

        // ������ �̵�
        transform.Translate(Vector3.up * moveSpd * Time.deltaTime);
    }

    IHit hitObj;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hitObj = collision.gameObject.GetComponent<IHit>();

        if (hitObj != null) StartCoroutine(Attack());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �˹� �ߵ����� �ʾҴٸ� �󸮸���
        if (!GameManager.instance.IsChanceTrue(player.KnockbackChance))
            return;

        StartCoroutine(KnockBack());
    }

    IEnumerator Attack()
    {
        while (gameObject.activeSelf)
        {
            hitObj.Hit(collDamage);
            // ���� �ݻ� �����
            CurrentHp -= maxHp * (player.ThronsPer * .01f);

            yield return new WaitForSeconds(2f);
        }
    }

    protected virtual void OnDead()
    {
        // �ڷ�ƾ�� ������ ���� ������ isKnockback�� true ����
        isKnockBack = false;

        // Dollar ȹ��
        GameManager.instance.GoodsFactor(killedDollar, this.transform, true);
        ReturnPool();
    }

    IEnumerator KnockBack()
    {
        isKnockBack = true;
        rb.AddForce(-transform.up * player.KnockbackForce, ForceMode2D.Impulse);

        yield return wait;

        rb.velocity = Vector2.zero;
        isKnockBack = false;
    }
}
