using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bullet : PoolObject
{
    [SerializeField]
    float speed;

    [SerializeField]
    TextMeshPro damageText;

    Player player;

    bool isCritBullet = false;
    bool isBouncBullet;
    int bounceCount;

    float bulletDmg;

    public Transform target;

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Delete());

        speed = 15;

        isBouncBullet = GameManager.instance.IsChanceTrue(player.BounceChance);
        // �Ű������� ũȮ�� �־����Ƿ� -> ũ��Ƽ���ΰ�?
        isCritBullet = GameManager.instance.IsChanceTrue(player.CritChance);
        bulletDmg = player.Damage;

        bounceCount = player.BounceCount;
    }

    [SerializeField]
    //float lineSize = 1000f;

    // Update is called once per frame
    void Update()
    {
        transform.up = target.position - transform.position;
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    // 2�� �ڿ� �ڽ��� ����
    IEnumerator Delete()
    {
        yield return new WaitForSeconds(4f);
        ReturnPool();
    }

    Collider2D[] colliders;
    List<Collider2D> hitEnemies = new List<Collider2D>();

    // �Ѿ��� ���� �����
    float finalDmg;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� ���濡�Լ� IHit�� �����´�
        IHit hitObj = collision.GetComponent<IHit>();

        if (hitObj == null)
            return;

        BulletDmgFormula();

        // hitObj�� Hit�Լ� ����
        hitObj.Hit(finalDmg);

        // �̹� ���� ���� �ڽ� �߰�
        hitEnemies.Add(collision);

        // �÷��̾� ���� �ߵ�
        player.LifeSteal(finalDmg);

        // �ε��� ���� ��Ƽ�갡 �������� �ʴٸ� -> ����ִٸ�
        if (collision.transform.gameObject.activeSelf == true) SpawnUpText(finalDmg);

        // �ٿ �Ѿ��̰�, �ٿ Ƚ���� �����ִٸ�
        if (isBouncBullet && bounceCount > 0)
        {
            Bounce(collision);
        }
        else
        {
            ReturnPool();
        }
    }

    void Bounce(Collider2D collision)   
    {
        bounceCount--;

        // �ٿ���� ���� �� ��� ã��
        colliders = Physics2D.OverlapCircleAll(transform.position, 
            player.BounceRange / 10, player.enemyMask);

        // Ÿ���� ���ٸ� ����Ǯ �� ��
        if (colliders.Length == 0)
        {
            ReturnPool();
            return;
        }

        // ����� ������ ����
        SortArrayByNearest();

        // ���� ���� ���� �ʾҴٸ�
        if (colliders[0] == collision)
        {
            // ���� ���� ���� ���� ���� ó�� ���� ���ۿ� ���ٸ�
            if (colliders.Length == 1)
            {
                ReturnPool();
            }
            else
            {
                // Ÿ���� �켱 �����ְ�
                target = colliders[1].transform;

                // �� Ÿ���� ���� ���ʹ����� �˻�
                for (int i = 1; i < colliders.Length; i++)
                {
                    // �̹� �¾Ҵ� ���� ����Ʈ�� ������� �ʴٸ�
                    if (!hitEnemies.Contains(colliders[i]))
                    {
                        target = colliders[i].transform;
                        break;
                    }
                }
            }
        }
        else // ���� ���� �׾��ٸ�
        {
            // ���� ������ Ÿ�� ����
            target = colliders[0].transform;

            for (int i = 0; i < colliders.Length; i++)
            {
                if (!hitEnemies.Contains(colliders[i]))
                {
                    target = colliders[i].transform;
                    break;
                }
            }
        }
    }

    void BulletDmgFormula()
    {
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
        GameObject tempDamageText = PoolManager.instance.GetPool(PoolObejectType.damageText);
        tempDamageText.transform.position = this.transform.position;

        TextMeshPro tempDamageTextMesh = tempDamageText.GetComponent<TextMeshPro>();
        tempDamageTextMesh.text = dmg.ToString("F2");
        tempDamageTextMesh.color = isCritBullet ? Color.red : Color.white;
    }

    void SortArrayByNearest()
    {      
        Transform nearestTarget = colliders[0].transform;
        float nearestDist = Vector3.Distance(nearestTarget.position, transform.position);
        float checkDist;

        for (int i = 1; i < colliders.Length; i++)
        {
            // üũ�� �Ÿ� ����
            checkDist = Vector3.Distance(colliders[i].transform.position, transform.position);

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
        Collider2D temp = colliders[0];
        colliders[0] = colliders[swapIndex];
        colliders[swapIndex] = temp;
    }
}
