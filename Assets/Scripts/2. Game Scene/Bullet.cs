using System.Collections;
using TMPro;
using Unity.VisualScripting;
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

        isBouncBullet = IsChanceTrue(player.BounceChance);
        // �Ű������� ũȮ�� �־����Ƿ� -> ũ��Ƽ���ΰ�?
        isCritBullet = IsChanceTrue(player.CritChance);
        bulletDmg = player.Damage;

        bounceCount = player.BounceCount;
    }

    [SerializeField]
    float lineSize = 1000f;

    // Update is called once per frame
    void Update()
    {
        transform.up = target.position - transform.position;
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    /// <summary>
    /// �� Ȯ���� ���� �ߵ� �ƴ���
    /// </summary>
    /// <param name="chance">�ߵ� Ȯ��</param>
    /// <returns></returns>
    bool IsChanceTrue(float chance)
    {
        float randNum = Random.Range(0f, 100f);

        // �������� �������� chance���� �۴ٸ�
        // �� ��ȯ -> Ȯ�� ����
        if (chance >= randNum)
        {
            return true;
        }
        else return false;
    }

    // 2�� �ڿ� �ڽ��� ����
    IEnumerator Delete()
    {
        yield return new WaitForSeconds(4f);
        ReturnPool();
    }

    Collider2D[] colliders;

    private void OnTriggerEnter2D(Collider2D collision)
    {       
        // ���� ���濡�Լ� IHit�� �����´�
        IHit hitObj = collision.GetComponent<IHit>();

        if (hitObj == null)
            return;

        // �Ѿ��� ����� �� �Ѿ��� ��ġ = ���� ���� ��ġ 
        // �÷��̾���� �Ÿ� -> ������/���� ����
        float dist = Vector3.Distance(transform.position, player.transform.position) * 10;

        // �Ÿ� ������ ����
        float distDmg = bulletDmg * Mathf.Pow(player.DmgPerMeter, dist);
        // ũ�� ������ ����
        float finalDmg = isCritBullet ? distDmg * player.CritFactor : distDmg;

        // hitObj�� Hit�Լ� ����
        hitObj.Hit(finalDmg);

        player.LifeSteal(finalDmg);

        // �ε��� ���� ��Ƽ�갡 �������� �ʴٸ� -> ����ִٸ�
        if (collision.transform.gameObject.activeSelf == true) SpawnUpText(finalDmg);

        // �ٿ �Ѿ��̰�
        if (isBouncBullet && bounceCount > 0)
        {
                bounceCount--;

                // �ٿ���� ���� �� ��� ã��
                colliders = Physics2D.OverlapCircleAll(transform.position, player.BounceRange / 10, player.enemyMask);
                    
                // ����� ������ ����
                SortArrayByNearest();

                if (colliders.Length == 0)
                {
                    ReturnPool();
                    return;
                }

                if (colliders[0] == collision)
                {
                    if (colliders.Length == 1)
                    {
                        ReturnPool();
                    }
                    else
                    {
                        target = colliders[1].transform;
                    }
                }
                else
                {
                    target = colliders[0].transform;
                }          
        }
        else
        {
            ReturnPool();
        }
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
        if (colliders.Length == 0)
            return;

        Transform nearestTarget = colliders[0].transform;
        float nearestDist = Vector3.Distance(nearestTarget.position, transform.position);
        float checkDist;

        for (int i = 1; i < colliders.Length; i++)
        {
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
