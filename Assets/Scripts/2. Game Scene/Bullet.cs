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

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Delete());

        isBouncBullet = IsChanceTrue(player.bounceChance);
        // �Ű������� ũȮ�� �־����Ƿ� -> ũ��Ƽ���ΰ�?
        isCritBullet = IsChanceTrue(player.CritChance);
        bulletDmg = player.Damage;

        bounceCount = player.bounceCount;
    }

    [SerializeField]
    float lineSize = 1000f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        Debug.DrawRay(transform.position, transform.up, Color.yellow, lineSize);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, lineSize);

        if (!hit)
        { 
            PoolManager.instance.SetPool(gameObject, PoolObejectType.bullet);
        }
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
        yield return new WaitForSeconds(2f);

        PoolManager.instance.SetPool(gameObject, PoolObejectType.bullet);
    }

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

        SpawnUpText(finalDmg);

        Transform nearestTarget;

        // ���� ��� ���� �� �� �߰�
        
        // �ٿ �Ѿ��̰�
        if (isBouncBullet)
        {
            if (bounceCount > 0)
            {
                bounceCount--;

                // �ٿ���� ���� �� ��� ã��
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, player.bounceRange, player.enemyMask);

                for (int i = 0; i < colliders.Length; i++)
                {
                    Debug.Log(colliders[i]);
                }

                if (colliders.Length > 1)
                {
                    // ���� ����� Ÿ��
                    nearestTarget = colliders[0].transform;
                    // ���� ����� Ÿ�ٰ��� �Ÿ�
                    float nearestDist = Vector3.Distance(colliders[0].transform.position, this.transform.position);

                    for (int j = 1; j < colliders.Length; j++)
                    {
                        // ���� ����� Ÿ�ٰ��� �Ÿ����� j��°���� �Ÿ��� �� ª����
                        if (Vector3.Distance(colliders[j].transform.position, this.transform.position) < nearestDist)
                        {
                            nearestDist = Vector3.Distance(colliders[j].transform.position, this.transform.position);
                            // ���� ����� Ÿ���� i�̴�
                            nearestTarget = colliders[j].transform;
                        }
                    }

                    transform.up = nearestTarget.position - this.transform.position;
                }
                else
                {
                    PoolManager.instance.SetPool(gameObject, PoolObejectType.bullet);
                    isBouncBullet = false;
                }
                colliders = new Collider2D[0];
            }
            else
            {
                isBouncBullet = false;
                PoolManager.instance.SetPool(gameObject, PoolObejectType.bullet);
            }            
        }
        else
        {
            isBouncBullet= false;
            PoolManager.instance.SetPool(gameObject, PoolObejectType.bullet);
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

    void FindNearestTarget(Collider2D[] colliders)
    {

    }
}
