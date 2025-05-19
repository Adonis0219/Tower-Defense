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
    /// �⺻ �浹 ������
    /// </summary>
    [SerializeField]
    float baseDamage;

    float damage;

    // �÷��̾���� �Ÿ�
    float range;

    ////////// �˹�
    bool isKnockBack = false;

    // ���� ���� ����
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


        // ���ο� ī�� �������ְ�
        // ���ο찡 �� �ɷȰ�, ���� ���� ������ ���ο� ����
        if (PlayDataManager.Instance.CheckCard(CardID.���ӿ���) && !isSlowed && (player.Range / 10 > range))
        {
            Slow();
        }

        // ���� ���������� ����
        transform.up = player.transform.position - transform.position;

        // ������ �̵�
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
        // ��ȯ�� �� �����ŭ �����ֱ�
        CurrentHp = maxHp;
    }

    public void Hit(float damage)
    {
        // ���� �� �´� �Ҹ�
        if (gameObject.activeSelf)
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Hit);
        CurrentHp -= damage;
    }

    IHit hitObj;

    Coroutine atkCoru;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hitObj = collision.gameObject.GetComponent<IHit>();

        // ������ �ε����� �� �߻� x
        if (hitObj != null && collision.transform.name == "Tower") atkCoru = StartCoroutine(Attack());
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

    void Slow()
    {
        CardData card = CardManager.instance.cardDatas[(int)CardID.���ӿ���];

        moveSpd *= (1 - (card.value[card.curLv] / 100));

        isSlowed = true;
    }

    public void OnDead()
    {
        if (atkCoru != null)
            // ������ ���� �۵����� �ʵ��� ���߱�
            StopCoroutine(atkCoru);

        // ������ �׾��� ��
        if (isBoss)
        {
            // ���� �״� �Ҹ�
            GameManager.instance.boss = null;
            GameManager.instance.bossKillCount++;
            GameManager.instance.bossHpPanel.SetActive(false);
        }
        else
        {
            // ���� �� �״� �Ҹ�
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Die);
        }


        // �ڷ�ƾ�� ������ ���� ������ isKnockback�� true ����
        isKnockBack = false;

        // Dollar ȹ��
        GameManager.instance.GoodsFactor(killedDollar, this.transform, true);

        // �Ϲ����� �ƴ� ��
        if (killedCoin != 0)
            // Coin ȹ��
            GameManager.instance.GoodsFactor(killedCoin, this.transform, false);


        ReturnPool();
    }
}
