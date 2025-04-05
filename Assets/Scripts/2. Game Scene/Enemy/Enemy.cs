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
    /// 기본 충돌 데미지
    /// </summary>
    [SerializeField]
    float baseCollDamage;

    float collDamage;

    ////////// 넉백
    bool isKnockBack = false;
    Rigidbody2D rb;
    WaitForSeconds wait;

    // 저속 오라 감지
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
        // 소환될 때 배수만큼 곱해주기
        CurrentHp = maxHp;
        collDamage = baseCollDamage * GameManager.instance.waveDmgFactor;
    }

    // Update is called once per frame
    void Update()
    {
        if (isKnockBack) return;

        float range = Vector2.Distance(player.transform.position, this.transform.position);

        // 슬로우가 안 걸렸고, 범위 내에 들어오면 슬로우 적용
        if (!isSlowed && (player.Range / 10 > range))
        {
            CardData card = CardManager.instance.cardDatas[(int)CardID.저속오라];

            moveSpd *= (1 - (card.value[card.curLv] / 100));

            isSlowed = true;
        }

        // 앞으로 이동
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
        // 넉백 발동하지 않았다면 얼리리턴
        if (!GameManager.instance.IsChanceTrue(player.KnockbackChance))
            return;

        StartCoroutine(KnockBack());
    }

    IEnumerator Attack()
    {
        while (gameObject.activeSelf)
        {
            hitObj.Hit(collDamage);
            // 가시 반사 대미지
            CurrentHp -= maxHp * (player.ThronsPer * .01f);

            yield return new WaitForSeconds(2f);
        }
    }

    protected virtual void OnDead()
    {
        // 코루틴이 끝나기 전에 죽으면 isKnockback이 true 상태
        isKnockBack = false;

        // Dollar 획득
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
