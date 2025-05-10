using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : PoolObject
{
    [SerializeField]
    TextMeshPro damageText;
    Player player;
    public Transform target;

    [Header("# Info")]
    [SerializeField]
    float speed;

    bool isCritBullet = false;
    bool isBouncBullet = false;


    int bounceCount;

    // �Ѿ��� �⺻ �����
    float bulletDmg;
    // �Ѿ��� ���� �����
    float finalDmg;

    [Header("# Detect")]
    // ���� ������ ��ġ ����
    Vector3 prePos;
    
    Collider2D enemy;

    // Ž���� ����
    Collider2D[] detectedEnemies;

    [SerializeField]
    /// <summary>
    /// �̹� ���� ����
    /// </summary>
    List<Collider2D> hitEnemies = new List<Collider2D>();


    private void Awake()
    {
        player = GameManager.instance.player;
    }

    private void OnEnable()
    {
        speed = 5;

        isBouncBullet = GameManager.instance.IsChanceTrue(player.BounceChance);
        // �Ű������� ũȮ�� �־����Ƿ� -> ũ��Ƽ���ΰ�?
        isCritBullet = GameManager.instance.IsChanceTrue(player.CritChance);

        bulletDmg = player.Damage;

        bounceCount = player.BounceCount;

        // ���� �� �ʱ�ȭ ���ֱ�
        hitEnemies.Clear();
    }

    // ������ ���� �ε���
    int detectedIndex;

    // Update is called once per frame
    void Update()
    {
        // ���ư��� ���߿� Ÿ���� ������ٸ�
        if (!target.gameObject.activeSelf)
            ReturnPool();

        //if (!target.gameObject.activeSelf)
        //{
        //    // �ٿ �Ѿ��� �ƴ϶�� ��������
        //    if (!isBouncBullet) ReturnPool();
        //    else // �ٿ �Ѿ��̶�� ���� Ÿ�� ã�ư���
        //    {
        //        detectedEnemies = Physics2D.OverlapCircleAll(transform.position,
        //                            player.BounceRange / 10, player.enemyMask);
        //        target = detectedEnemies[0].transform;
        //    }
        //}

            transform.up = target.position - transform.position;

        transform.Translate(Vector3.up * speed * Time.deltaTime);

        Vector3 posChange = prePos - transform.position;

        // ���� �����Ӱ� ���� ������ ������ ���̿� �´� ��ü ã��
        RaycastHit2D[] hitObjs = 
            Physics2D.RaycastAll(transform.position, posChange, Vector2.Distance(prePos, transform.position));

        // ���� ��ü �� ���� �ִٸ�
        if (Array.Find(hitObjs, x => x.transform.tag == "Enemy"))
        {
            // ���� collider2d�� ���ͼ� ����
            enemy = Array.Find(hitObjs, x => x.transform.tag == "Enemy").transform.GetComponent<Collider2D>();
            EnemyHit();
        }
    }

    private void LateUpdate()
    {
        prePos = transform.position;
    }

    void EnemyHit()
    {
        IHit hitObj = enemy.GetComponent<IHit>();

        BulletDmgFormula();

        // hitObj�� Hit�Լ� ����
        hitObj.Hit(finalDmg);

        // �̹� ���� ���� �ڽ� �߰�
        hitEnemies.Add(enemy);

        // �÷��̾� ���� �ߵ�
        player.LifeSteal(finalDmg);

        // �ε��� ���� ��Ƽ�갡 �������� �ʴٸ� -> ����ִٸ�
        if (enemy.transform.gameObject.activeSelf == true) SpawnUpText(finalDmg);

        // �ٿ �Ѿ��̰�, �ٿ Ƚ���� �����ִٸ�
        if (isBouncBullet && bounceCount > 0)
        {
            Bounce(enemy);
        }
        else
        {
            ReturnPool();
        }
    }

    void Bounce(Collider2D collision)   
    {
        // �Ѿ� �ӵ� �÷��ֱ�
        speed = 10;

        bounceCount--;

        // �ٿ���� ���� �� ��� ã��
        detectedEnemies = Physics2D.OverlapCircleAll(transform.position, 
            player.BounceRange / 10, player.enemyMask);

        // Ÿ���� ���ٸ� ����Ǯ �� ��
        if (detectedEnemies.Length == 0)
        {
            ReturnPool();
            return;
        }

        // ����� ������ ����
        SortArrayByNearest();

        // ���� ���� ���� �ʾҴٸ�
        if (detectedEnemies[0] == collision)
        {
            // ���� ���� ���� ���� ���� ó�� ���� ���ۿ� ���ٸ�
            if (detectedEnemies.Length == 1)
            {
                ReturnPool();
            }
            else
            {
                // Ÿ���� �켱 �����ְ�
                target = detectedEnemies[1].transform;

                // �� Ÿ���� ���� ���ʹ����� �˻�
                for (int i = 1; i < detectedEnemies.Length; i++)
                {
                    // �̹� �¾Ҵ� ���� ����Ʈ�� ������� �ʴٸ�
                    if (!hitEnemies.Contains(detectedEnemies[i]))
                    {
                        target = detectedEnemies[i].transform;
                        break;
                    }
                }
            }
        }
        else // ���� ���� �׾��ٸ�
        {
            // ���� ������ Ÿ�� ����
            target = detectedEnemies[0].transform;

            for (int i = 0; i < detectedEnemies.Length; i++)
            {
                if (!hitEnemies.Contains(detectedEnemies[i]))
                {
                    target = detectedEnemies[i].transform;
                    detectedIndex = i;
                    break;
                }
            }
        }
    }

    void BulletDmgFormula()
    {
        // �÷��̾ ������ ���� ���
        if (player == null)
            return;

        // �Ѿ��� ����� �� �Ѿ��� ��ġ = ���� ���� ��ġ
        // �÷��̾���� �Ÿ� -> ������/���� ����
        float dist = Vector3.Distance(transform.position, player.transform.position) * 10;

        // �Ÿ� ������ ����
        float distDmg = bulletDmg * Mathf.Pow(player.DmgPerMeter, dist);
        // ũ�� ������ ����
        finalDmg = isCritBullet ? distDmg * player.CritFactor : distDmg;
    }

    void SpawnUpText(float dmg)
    {
        GameObject tempDamageText = PoolManager.instance.GetPool(PoolObejectType.DamageText);
        tempDamageText.transform.position = this.transform.position;

        TextMeshPro tempDamageTextMesh = tempDamageText.GetComponent<TextMeshPro>();
        tempDamageTextMesh.text = dmg.ToString("F2");
        tempDamageTextMesh.color = isCritBullet ? Color.red : Color.white;
    }

    void SortArrayByNearest()
    {      
        Transform nearestTarget = detectedEnemies[0].transform;
        float nearestDist = Vector3.Distance(nearestTarget.position, transform.position);
        float checkDist;

        for (int i = 1; i < detectedEnemies.Length; i++)
        {
            // üũ�� �Ÿ� ����
            checkDist = Vector3.Distance(detectedEnemies[i].transform.position, transform.position);

            // üũ �Ÿ��� �� ª�ٸ�
            if (checkDist < nearestDist)
            {
                // �迭�� ù��°�� �ڽ��� �ٲ���
                Swap(i);
            }
        }
    }

    void Swap(int swapIndex)
    {
        Collider2D temp = detectedEnemies[0];
        detectedEnemies[0] = detectedEnemies[swapIndex];
        detectedEnemies[swapIndex] = temp;
    }
}
