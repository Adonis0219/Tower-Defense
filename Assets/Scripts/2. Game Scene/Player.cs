using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHit
{
    [Header("# Debug")]
    [SerializeField]
    bool isShoot = false;

    [Header("# Player Status")]
    [Header("  # Def")]
    [SerializeField]
    float maxHp;

    public float MaxHp
    {
        get { return maxHp; }
        set 
        { 
            maxHp = value; 
        }
    }

    [SerializeField]
    float currentHp;

    public float CurrentHp
    {
        get { return currentHp; }
        set 
        { 
            currentHp = value;

            // ü��ȸ������ ���� �ִ�ü�º��� ����ü���� Ŀ���� �ٽ� �ʱ�ȭ
            if (CurrentHp > MaxHp)
                CurrentHp = MaxHp;

            // ����ü���� 0���Ϸ� �������� ���
            if (currentHp <= 0)
                Dead();
        }
    }

    public float regenHp;

    public float def;
    public float absDef;

    [Header("  # ATTACK")]
    [SerializeField]
    float damage;

    public float Damage
    {
        get
        {
            damage = 3 * (GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.������] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.������] + 1);
            return damage;
        }
    }

    [SerializeField]    // ũ��Ƽ�� Ȯ��
    public float critChance = 1;

    [SerializeField]    // ũ��Ƽ�� ����
    public float critFactor = 1.2f;

    [SerializeField]
    float atkSpd;

    public float AtkSpd
    {
        get
        {
            atkSpd = 1 + .05f * (GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.���ݼӵ�] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.���ݼӵ�]);
            return atkSpd;
        }
    }


    [Header("# Bullet")]
    [SerializeField]
    Transform range;

    [SerializeField]
    float radius;

    [SerializeField]
    LayerMask enemyMask;

    Transform nearestTarget;

    private void Awake()
    {
        CurrentHp = MaxHp;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isShoot)
            StartCoroutine(OnShoot());

        StartCoroutine(RegenHp());
    }

    // Update is called once per frame
    void Update()
    {
        // ���� ����
        range.localScale = Vector3.one * radius * 2;
    }

    void Shoot(Vector3 targetPos)
    {       
        Transform tempBullet = PoolManager.instance.GetPool(PoolObejectType.bullet).transform;
        tempBullet.SetParent(GameManager.instance.poolManager.GetChild(0));
        tempBullet.position = transform.position;
        tempBullet.up = targetPos - transform.position;

        //Transform tempBullet = Instantiate(oriBullet, transform.position, Quaternion.identity);
    }

    IEnumerator OnShoot()
    {
        while (true)
        {
            // ���� ���� collider ��� ��������
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, enemyMask);
            // �ݶ��̴��� �ִٸ� -> ���� �ִٸ�
            if (colliders.Length > 0 && Vector3.Distance(colliders[0].transform.position, this.transform.position) <= radius)
            {
                // ���� ����� Ÿ��
                nearestTarget = colliders[0].transform;
                // ���� ����� Ÿ�ٰ��� �Ÿ�
                float nearestDist = Vector3.Distance(colliders[0].transform.position, this.transform.position);

                for (int i = 1; i < colliders.Length; i++)
                {
                    // ���� ����� Ÿ�ٰ��� �Ÿ����� i��°���� �Ÿ��� �� ª����
                    if (Vector3.Distance(colliders[i].transform.position, this.transform.position) < nearestDist)
                    {
                        nearestDist = Vector3.Distance(colliders[i].transform.position, this.transform.position);
                        // ���� ����� Ÿ���� i�̴�
                        nearestTarget = colliders[i].transform;
                    }
                }

                // ù��° ������ �Ѿ� �߻�
                Shoot(nearestTarget.position);              

                yield return new WaitForSeconds(1/AtkSpd);
            }

            yield return null;
        }
    }

    IEnumerator RegenHp()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            // ������ ü�°� �ִ� ü���� ���� ���� ����
            if (CurrentHp != MaxHp)
                CurrentHp += regenHp;
        }
    }    

    public void Hit(float damage)
    {
        CurrentHp -= (damage * (1 - def / 100)) - absDef;
        //CurrentHp -= damage;
    }

    // �ǰݵ�����
    IEnumerator IHit.OnCollisionHit(float damage)
    {
        while (CurrentHp > 0)
        {
            CurrentHp -= (damage * (1 - def / 100)) - absDef;

            yield return new WaitForSeconds(2f);
        }
    }

    void Dead()
    {
        GameManager.instance.resultPanel.SetActive(true);
        Destroy(gameObject);
        Time.timeScale = 0;
    }
}
