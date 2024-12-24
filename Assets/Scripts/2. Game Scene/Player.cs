using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHit
{
    [Header("# Debug")]
    [SerializeField]
    bool isShoot = false;

    [Header("# Player Status")]
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
        // �ܺο��� ������ �� �����Ƿ� Set�� ����
    }

    [SerializeField]    // ũ��Ƽ�� Ȯ��
    float critChance;

    public float CritChance
    {
        get
        {
            critChance = GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.ġ��ŸȮ��] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.ġ��ŸȮ��] + 1;
            return critChance;
        }
    }

    [SerializeField]    // ũ��Ƽ�� ����
    float critFactor;

    public float CritFactor
    {
        get
        {
            critFactor = 1.2f + .1f * (GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.ġ��Ÿ������] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.ġ��Ÿ������]);
            return critFactor;
        }
    }

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


    [Header("  # Def")]
    [SerializeField]
    float maxHp;

    public float MaxHp
    {
        get 
        { 
            maxHp = 5 * (1 + GameManager.instance.defDollarLevels[(int)DefUpgradeType.ü��] + GameManager.instance.defCoinLevels[(int)DefUpgradeType.ü��]);
            return maxHp; 
        }
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

    [SerializeField]
    float regenHp;

    public float RegenHp
    {
        get
        {
            regenHp = .04f * (GameManager.instance.defDollarLevels[(int)DefUpgradeType.ü��ȸ��] + GameManager.instance.defCoinLevels[(int)DefUpgradeType.ü��ȸ��]);
            return regenHp; 
        }
    }

    [SerializeField]
    float def;

    public float Def
    {
        get
        {
            def = .5f * (GameManager.instance.defDollarLevels[(int)DefUpgradeType.����] + GameManager.instance.defCoinLevels[(int)DefUpgradeType.����]);
            return def;
        }
    }

    float absDef;
    public float AbsDef
    {
        get
        {
            absDef = .5f * (GameManager.instance.defDollarLevels[(int)DefUpgradeType.������] + GameManager.instance.defCoinLevels[(int)DefUpgradeType.������]);
            return absDef; 
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

    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentHp = MaxHp;

        if (isShoot)
            StartCoroutine(OnShoot());

        StartCoroutine(OnRegenHp());
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

    IEnumerator OnRegenHp()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            // ������ ü�°� �ִ� ü���� ���� ���� ����
            if (CurrentHp != MaxHp)
                CurrentHp += RegenHp;
        }
    }    

    public void Hit(float damage)
    {
        CurrentHp -= (damage * (1 - Def)) - AbsDef;
        //CurrentHp -= damage;
    }

    // �ǰݵ�����
    IEnumerator IHit.OnCollisionHit(float damage)
    {
        while (CurrentHp > 0)
        {
            CurrentHp -= (damage * (1 - Def / 100)) - AbsDef;

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
