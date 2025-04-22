using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyType
{
    Normal, Speed, Range, Tank, Boss, Length
}

public class Enemy : PoolObject, IHit
{
    Player player;
    Rigidbody2D rb;
    WaitForSeconds wait;

    [Header("# Info")]
    [SerializeField]
    EnemyData myData;

    public EnemyData MyData
    {
        get
        {
            return myData;
        }
        set
        {
            myData = value;

            InitSet();
        }
    }
    
    public EnemyType myType;

    [SerializeField]
    public SpriteRenderer sprRen;

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
    /// �⺻ �浹 ������
    /// </summary>
    [SerializeField]
    float baseDamage;

    float damage;

    ////////// �˹�
    bool isKnockBack = false;

    // ���� ���� ����
    bool isSlowed = false;

    private void Awake()
    {
        player = GameManager.instance.player;
        rb = GetComponent<Rigidbody2D>();
        sprRen = GetComponent<SpriteRenderer>();
        wait = new WaitForSeconds(.1f);
    }

    private void OnEnable()
    {
        damage = baseDamage * GameManager.instance.waveDmgFactor;
        maxHp = BaseMaxHp * GameManager.instance.waveHpFactor;
        // ��ȯ�� �� �����ŭ �����ֱ�
        CurrentHp = maxHp;
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

    void InitSet()
    {
        myType = MyData.type;
        sprRen.sprite = MyData.sprite;
        transform.localScale = Vector3.one * MyData.scale;

        killedDollar = MyData.killedDollar;
        killedCoin = MyData.killedCoin;
        moveSpd = MyData.moveSpeed;
        BaseMaxHp = MyData.baseMaxHp;
        baseDamage = MyData.baseDmg;
    }

    public void Hit(float damage)
    {
        CurrentHp -= damage;
    }

    IHit hitObj;

    private void OnCollisionStay2D(Collision2D collision)
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
            hitObj.Hit(damage);
            // ���� �ݻ� ����� (�ۼ�Ʈ���̹Ƿ� .01f�Ͽ� ����)
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

    public void OnDead()
    {
        // �ڷ�ƾ�� ������ ���� ������ isKnockback�� true ����
        isKnockBack = false;

        // Dollar ȹ��
        GameManager.instance.GoodsFactor(killedDollar, this.transform, true);
        // Coin ȹ��
        GameManager.instance.GoodsFactor(killedCoin, this.transform, false);
        ReturnPool();
    }
}
