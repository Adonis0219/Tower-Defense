using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour, IHit
{
    [Header("# Debug")]

    //bool isShoot = false;

    [Header("# Player Status")]
    [Header("  # ATTACK")]
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

    float critChance;

    public float CritChance
    {
        get
        {
            critChance = GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.ġ��ŸȮ��] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.ġ��ŸȮ��] + 1;
            return critChance;
        }
    }

    float critFactor;

    public float CritFactor
    {
        get
        {
            critFactor = 1.2f + .1f * (GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.ġ��Ÿ������] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.ġ��Ÿ������]);
            return critFactor;
        }
    }

    float atkSpd;

    public float AtkSpd
    {
        get
        {
            atkSpd = 1 + .05f * (GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.���ݼӵ�] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.���ݼӵ�]);
            return atkSpd;
        }
    }

    float range;

    /// <summary>
    /// ������ ���� ����
    /// </summary>
    public float Range
    {
        get 
        {
            range = 20 + .5f * (GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.����] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.����]);
            return range; 
        }
    }

    float dmgPerMeter;

    public float DmgPerMeter
    {
        get
        {
            dmgPerMeter = 1 + .008f * (GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.�Ÿ��絥����] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.�Ÿ��絥����]);
            return dmgPerMeter;
        }
    }

    float multiChance;

    public float MultiChance
    {
        get
        {
            multiChance = .5f * (GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.��Ƽ��Ȯ��] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.��Ƽ��Ȯ��]);
            return multiChance;
        }
    }

    int multiCount;

    public int MultiCount
    {
        get
        {
            multiCount = 2 + GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.��Ƽ��ǥ��] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.��Ƽ��ǥ��];
            return multiCount;
        }
    }

    float bounceChance;

    public float BounceChance
    {
        get
        {
            bounceChance = .5f * (GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.�ٿ��Ȯ��] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.�ٿ��Ȯ��]);
            return bounceChance;
        }
    }

    int bounceCount;

    public int BounceCount
    {
        get
        {
            bounceCount = 2 + GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.�ٿ��ǥ��] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.�ٿ��ǥ��];
            return bounceCount;
        }
    }

    float bounceRange;

    /// <summary>
    /// ������ ����
    /// </summary>
    public float BounceRange
    {
        get
        {
            bounceRange = 10 + .1f * (GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.�ٿ������] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.�ٿ������]);
            return bounceRange;
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

    float thronsPer;
    /// <summary>
    /// ���� ����� �ۼ�Ʈ
    /// </summary>
    public float ThronsPer
    {
        get
        {
            thronsPer = GameManager.instance.defDollarLevels[(int)DefUpgradeType.���ô����] + GameManager.instance.defCoinLevels[(int)DefUpgradeType.���ô����];
            return thronsPer;
        }
    }

    float lifeStealPer;

    /// <summary>
    /// ���� �ۼ�Ʈ
    /// </summary>
    public float LifeStealPer
    {
        get
        {
            lifeStealPer = .05f * (GameManager.instance.defDollarLevels[(int)DefUpgradeType.����] + GameManager.instance.defCoinLevels[(int)DefUpgradeType.����]);
            return lifeStealPer;
        }
    }

    [Header("  # Util")]
   // [SerializeField]
   

    [Header("# Bullet")]
    [SerializeField]
    Transform rangeObject;

    [SerializeField]
    public LayerMask enemyMask;

    Transform nearestTarget;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHp = MaxHp;

        StartCoroutine(OnShoot());
        StartCoroutine(OnRegenHp());
    }

    // Update is called once per frame
    void Update()
    {
        // ���� ����
        rangeObject.localScale = Vector3.one * Range / 5;        
    }

    bool IsMultishot(float chance)
    {
        float randNum = Random.Range(0f, 100f);

        // �������� �������� chance���� �۴ٸ�
        // �� ��ȯ -> ũ��Ƽ���̴�
        if (chance >= randNum)
        {
            return true;
        }
        else return false;
    }

    IEnumerator OnShoot()
    {
        while (true)
        {
            // ���� ���� collider ��� ��������
            Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, Range / 10, enemyMask);

            List<Target> targets = new List<Target>();

            foreach (var coll in colls)
            {
                targets.Add(new Target(coll, Vector3.Distance(transform.position, coll.transform.position)));
            }

            List<Target> sortedTraget = targets.OrderBy(x => x.distance).ToList();


            if (sortedTraget.Count > 0)
            {
                //int shootCount = IsMultishot(MultiChance) ? MultiCount : 1;
                int shootCount = IsMultishot(100) ? MultiCount : 1;

                if (shootCount > sortedTraget.Count) shootCount = sortedTraget.Count;
               
                // ����� ������ �߻��(��Ƽ�� ��)��ŭ �߻�
                for (int i = 0; i < shootCount; i++)
                {
                    Shoot(sortedTraget[i].collider.transform);
                }

                yield return new WaitForSeconds(1 / AtkSpd);
            }

            yield return null;
        }
    }

    void Shoot(Transform target)
    {
        Transform tempBullet = PoolManager.instance.GetPool(PoolObejectType.bullet).transform;
        tempBullet.SetParent(GameManager.instance.poolManager.GetChild(0));
        tempBullet.position = transform.position;
        
        tempBullet.GetComponent<Bullet>().target = target;
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
    }

    void Dead()
    {
        GameManager.instance.resultPanel.SetActive(true);
        Destroy(gameObject);
        Time.timeScale = 0;
    }

    public void LifeSteal(float dmg)
    {
        CurrentHp += dmg * (LifeStealPer * .01f);
    }
}

public class Target
{
    public Collider2D collider;
    public float distance;

    public Target(Collider2D collider, float distance)
    {
        this.collider = collider;
        this.distance = distance;
    } 
}