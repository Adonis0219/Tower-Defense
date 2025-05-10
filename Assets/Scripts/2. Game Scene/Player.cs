using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour, IHit
{
    [Header("# Player Status")]
    [Header("  # ATTACK")]
    float damage;

    public float Damage
    {
        get
        {
            return PlayDataManager.Instance.DmgFormula(SceneType.Game);
        }
        // �ܺο��� ������ �ʿ䰡 �����Ƿ� Set�� ����
    }

    float critChance;

    public float CritChance
    {
        get
        {
            critChance = PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.ġ��ŸȮ��] 
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.ġ��ŸȮ��] + 1;
            return critChance;
        }
    }

    float critFactor;

    public float CritFactor
    {
        get
        {
            return PlayDataManager.Instance.CritFactorFormula(SceneType.Game);
        }
    }

    float atkSpd;

    public float AtkSpd
    {
        get
        {
           return PlayDataManager.Instance.AtkSpdFormula(SceneType.Game);
        }
    }

    [SerializeField]
    float range;

    /// <summary>
    /// ������ ���� ����
    /// </summary>
    public float Range
    {
        get 
        {
            return PlayDataManager.Instance.RangeFormula(SceneType.Game); 
        }
    }

    [SerializeField]
    float dmgPerMeter;

    public float DmgPerMeter
    {
        get
        {
            dmgPerMeter = 1 + .008f * (PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.�Ÿ��絥����] 
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.�Ÿ��絥����]);
            return dmgPerMeter;
        }
    }

    [SerializeField]
    float multiChance;

    public float MultiChance
    {
        get
        {
            multiChance = .5f * (PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.��Ƽ��Ȯ��] 
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.��Ƽ��Ȯ��]);
            return multiChance;
        }
    }

    [SerializeField]
    int multiCount;

    public int MultiCount
    {
        get
        {
            multiCount = 2 + PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.��Ƽ��ǥ��] 
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.��Ƽ��ǥ��];
            return multiCount;
        }
    }

    [SerializeField]
    float bounceChance;

    public float BounceChance
    {
        get
        {
            bounceChance = .5f * (PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.�ٿ��Ȯ��] 
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.�ٿ��Ȯ��]);
            return bounceChance;
        }
    }

    [SerializeField]
    int bounceCount;

    public int BounceCount
    {
        get
        {
            bounceCount = 2 + PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.�ٿ��ǥ��] 
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.�ٿ��ǥ��];
            return bounceCount;
        }
    }

    [SerializeField]
    float bounceRange;

    /// <summary>
    /// ������ ����
    /// </summary>
    public float BounceRange
    {
        get
        {
            bounceRange = 10 + .1f * (PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.�ٿ������] 
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.�ٿ������]);
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
            return PlayDataManager.Instance.HpFormula(SceneType.Game); 
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
            return PlayDataManager.Instance.HpRegenFormula(SceneType.Game); 
        }
    }

    [SerializeField]
    float def;

    public float Def
    {
        get
        {
            def = .5f * (GameManager.instance.defDollarLevels[(int)DefUpgradeType.����] 
                + PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.����]);
            return def;
        }
    }

    [SerializeField]
    float absDef;
    public float AbsDef
    {
        get
        {
            return PlayDataManager.Instance.AbsDefFormula(SceneType.Game);
        }
    }

    [SerializeField]
    float thronsPer;
    /// <summary>
    /// ���� ����� �ۼ�Ʈ
    /// </summary>
    public float ThronsPer
    {
        get
        {
            thronsPer = GameManager.instance.defDollarLevels[(int)DefUpgradeType.���ô����] 
                + PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.���ô����];
            return thronsPer;
        }
    }

    [SerializeField]
    float lifeStealPer;

    /// <summary>
    /// ���� �ۼ�Ʈ
    /// </summary>
    public float LifeStealPer
    {
        get
        {
            lifeStealPer = .05f * (GameManager.instance.defDollarLevels[(int)DefUpgradeType.����] 
                + PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.����]);
            return lifeStealPer;
        }
    }

    [SerializeField]
    float knockbackChance;

    public float KnockbackChance
    {
        get
        {
            knockbackChance = GameManager.instance.defDollarLevels[(int)DefUpgradeType.�˹�Ȯ��] 
                + PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.�˹�Ȯ��];
            return knockbackChance;
        }
    }

    [SerializeField]
    float knockbackForce;

    public float KnockbackForce
    {
        get
        {
            knockbackForce = .5f + .15f * (GameManager.instance.defDollarLevels[(int)DefUpgradeType.�˹鰭��]
                + PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.�˹鰭��]);
            return knockbackForce;
        }
    }  

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

    IEnumerator OnShoot()
    {
        while (true)
        {
            // ���� ���� collider ��� ��������
            Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, Range / 10, enemyMask);

            // �÷��̾��� Ÿ�ٵ�
            List<Target> targets = new List<Target>();

            // ���� ���� �ݶ��̴��� Target ����Ʈ�� �־���
            foreach (var coll in colls)
            {
                // Target Ŭ������ ������ �־���
                // coll�� ���� �÷��̾� ������ �Ÿ�
                targets.Add(new Target(coll, Vector3.Distance(transform.position, coll.transform.position)));
            }

            // ���ٽ��� �̿��� targets ����Ʈ�� �Ÿ��� ����� ������ ���� �� sortedTarget�� �ʱ�ȭ
            List<Target> sortedTarget = targets.OrderBy(x => x.distance).ToList();

            // ���� ������
            if (sortedTarget.Count > 0)
            {
                // ��Ƽ�� Ȯ���� �����Ͽ� �߻���� ������
                int shootCount = GameManager.instance.IsChanceTrue(MultiChance) ? MultiCount : 1;

                // �߻���� �������� ������ ������ �߻�� ����
                if (shootCount > sortedTarget.Count) shootCount = sortedTarget.Count;
               
                // ����� ������ �߻��(��Ƽ�� ��)��ŭ �߻�
                for (int i = 0; i < shootCount; i++)
                {
                    Shoot(sortedTarget[i].collider.transform);
                }

                yield return new WaitForSeconds(1 / AtkSpd);
            }

            yield return null;
        }
    }

    /// <summary>
    /// �Ѿ� �߻� �Լ�
    /// </summary>
    /// <param name="target">Ÿ��</param>
    void Shoot(Transform target)
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Fire);


        // �Ѿ� ����
        Transform tempBullet = PoolManager.instance.GetPool(PoolObejectType.Bullet).transform;
        // �θ� ����
        tempBullet.SetParent(GameManager.instance.poolManager.GetChild(0));
        // �ʱ� ��ġ ����
        tempBullet.position = transform.position;

        tempBullet.up = target.position - transform.position;

        // �ٶ� ������ ���� Target ����
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
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.PlayerHit);
        CurrentHp -= (damage * (1 - Def)) - AbsDef;
    }

    void Dead()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.PlayerDie);
        Time.timeScale = 0;
        GameManager.instance.ResultPanelSetActive(true);
        Destroy(gameObject);
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