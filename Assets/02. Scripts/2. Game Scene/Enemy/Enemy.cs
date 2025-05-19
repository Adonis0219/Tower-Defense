using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : PoolObject, IHit
{
    Player player;
    Rigidbody2D rb;
    WaitForSeconds wait;

    [Header("# Info")]
    [SerializeField]
    EnemyData myData;

    public bool isBoss = false;

    [Header("# Status")]
    public float killedDollar;

    public float killedCoin;

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
    float baseDamage;

    float damage;

    // 플레이어와의 거리
    float range;

    ////////// 넉백
    bool isKnockBack = false;

    // 저속 오라 감지
    bool isSlowed = false;

    private void Awake()
    {
        player = GameManager.instance.player;
        rb = GetComponent<Rigidbody2D>();
        wait = new WaitForSeconds(.1f);
    }

    private void OnEnable()
    {
        InitSet();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || isKnockBack) return;


        range = Vector2.Distance(player.transform.position, this.transform.position);


        // 슬로우 카드 장착돼있고
        // 슬로우가 안 걸렸고, 범위 내에 들어오면 슬로우 적용
        if (PlayDataManager.Instance.CheckCard(CardID.저속오라) && !isSlowed && (player.Range / 10 > range))
        {
            Slow();
        }

        // 방향 지속적으로 설정
        transform.up = player.transform.position - transform.position;

        // 앞으로 이동
        transform.Translate(Vector3.up * moveSpd * Time.deltaTime);
    }

    void InitSet()
    {
        killedDollar = myData.killedDollar;
        killedCoin = myData.killedCoin;
        moveSpd = myData.moveSpeed;
        BaseMaxHp = myData.baseMaxHp;
        baseDamage = myData.baseDmg;

        damage = baseDamage * GameManager.instance.waveDmgFactor;
        maxHp = BaseMaxHp * GameManager.instance.waveHpFactor;
        // 소환될 때 배수만큼 곱해주기
        CurrentHp = maxHp;
    }

    public void Hit(float damage)
    {
        // 맞을 땐 맞는 소리
        if (gameObject.activeSelf)
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Hit);
        CurrentHp -= damage;
    }

    IHit hitObj;

    Coroutine atkCoru;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hitObj = collision.gameObject.GetComponent<IHit>();

        // 적끼리 부딪혔을 때 발생 x
        if (hitObj != null && collision.transform.name == "Tower") atkCoru = StartCoroutine(Attack());
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
            hitObj.Hit(damage);
            // 가시 반사 대미지 (퍼센트값이므로 .01f하여 적용)
            CurrentHp -= maxHp * (player.ThronsPer * .01f);

            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator KnockBack()
    {
        isKnockBack = true;
        rb.AddForce(-transform.up * player.KnockbackForce, ForceMode2D.Impulse);

        yield return wait;

        rb.velocity = Vector2.zero;
        isKnockBack = false;
    }

    void Slow()
    {
        CardData card = CardManager.instance.cardDatas[(int)CardID.저속오라];

        moveSpd *= (1 - (card.value[card.curLv] / 100));

        isSlowed = true;
    }

    public void OnDead()
    {
        if (atkCoru != null)
            // 죽으면 공격 작동하지 않도록 멈추기
            StopCoroutine(atkCoru);

        // 보스가 죽었을 때
        if (isBoss)
        {
            // 보스 죽는 소리
            GameManager.instance.boss = null;
            GameManager.instance.bossKillCount++;
            GameManager.instance.bossHpPanel.SetActive(false);
        }
        else
        {
            // 죽을 땐 죽는 소리
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Die);
        }


        // 코루틴이 끝나기 전에 죽으면 isKnockback이 true 상태
        isKnockBack = false;

        // Dollar 획득
        GameManager.instance.GoodsFactor(killedDollar, this.transform, true);

        // 일반적이 아닐 때
        if (killedCoin != 0)
            // Coin 획득
            GameManager.instance.GoodsFactor(killedCoin, this.transform, false);


        ReturnPool();
    }
}
