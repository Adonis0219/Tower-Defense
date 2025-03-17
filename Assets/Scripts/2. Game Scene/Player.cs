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
        // 외부에선 설정할 필요가 없으므로 Set은 없다
    }

    float critChance;

    public float CritChance
    {
        get
        {
            critChance = PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.치명타확률] 
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.치명타확률] + 1;
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

    float range;

    /// <summary>
    /// 반지름 기준 범위
    /// </summary>
    public float Range
    {
        get 
        {
            return PlayDataManager.Instance.RangeFormula(SceneType.Game); 
        }
    }

    float dmgPerMeter;

    public float DmgPerMeter
    {
        get
        {
            dmgPerMeter = 1 + .008f * (PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.거리당데미지] 
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.거리당데미지]);
            return dmgPerMeter;
        }
    }

    float multiChance;

    public float MultiChance
    {
        get
        {
            multiChance = .5f * (PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.멀티샷확률] 
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.멀티샷확률]);
            return multiChance;
        }
    }

    int multiCount;

    public int MultiCount
    {
        get
        {
            multiCount = 2 + PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.멀티샷표적] 
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.멀티샷표적];
            return multiCount;
        }
    }

    float bounceChance;

    public float BounceChance
    {
        get
        {
            bounceChance = .5f * (PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.바운스샷확률] 
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.바운스샷확률]);
            return bounceChance;
        }
    }

    int bounceCount;

    public int BounceCount
    {
        get
        {
            bounceCount = 2 + PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.바운스샷표적] 
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.바운스샷표적];
            return bounceCount;
        }
    }

    float bounceRange;

    /// <summary>
    /// 반지름 기준
    /// </summary>
    public float BounceRange
    {
        get
        {
            bounceRange = 10 + .1f * (PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.바운스샷범위] 
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.바운스샷범위]);
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
            maxHp = 5 * (1 + GameManager.instance.defDollarLevels[(int)DefUpgradeType.체력] 
                + PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.체력]);
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

            // 체력회복으로 인해 최대체력보다 현재체력이 커지면 다시 초기화
            if (CurrentHp > MaxHp)
                CurrentHp = MaxHp;

            // 현재체력이 0이하로 내려가면 사망
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
            regenHp = .04f * (GameManager.instance.defDollarLevels[(int)DefUpgradeType.체력회복] 
                + PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.체력회복]);
            return regenHp; 
        }
    }

    [SerializeField]
    float def;

    public float Def
    {
        get
        {
            def = .5f * (GameManager.instance.defDollarLevels[(int)DefUpgradeType.방어력] 
                + PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.방어력]);
            return def;
        }
    }

    float absDef;
    public float AbsDef
    {
        get
        {
            absDef = .5f * (GameManager.instance.defDollarLevels[(int)DefUpgradeType.절대방어] 
                + PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.절대방어]);
            return absDef; 
        }
    }

    float thronsPer;
    /// <summary>
    /// 가시 대미지 퍼센트
    /// </summary>
    public float ThronsPer
    {
        get
        {
            thronsPer = GameManager.instance.defDollarLevels[(int)DefUpgradeType.가시대미지] 
                + PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.가시대미지];
            return thronsPer;
        }
    }

    float lifeStealPer;

    /// <summary>
    /// 흡혈 퍼센트
    /// </summary>
    public float LifeStealPer
    {
        get
        {
            lifeStealPer = .05f * (GameManager.instance.defDollarLevels[(int)DefUpgradeType.흡혈] 
                + PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.흡혈]);
            return lifeStealPer;
        }
    }


    [SerializeField]
    float knockbackChance;

    public float KnockbackChance
    {
        get
        {
            knockbackChance = GameManager.instance.defDollarLevels[(int)DefUpgradeType.넉백확률] 
                + PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.넉백확률];
            return knockbackChance;
        }
    }

    [SerializeField]
    float knockbackForce;

    public float KnockbackForce
    {
        get
        {
            knockbackForce = .5f + .15f * (GameManager.instance.defDollarLevels[(int)DefUpgradeType.넉백강도]
                + PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.넉백강도]);
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
        // 범위 조정
        rangeObject.localScale = Vector3.one * Range / 5;        
    }

    IEnumerator OnShoot()
    {
        while (true)
        {
            // 범위 내의 collider 모두 가져오기
            Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, Range / 10, enemyMask);

            List<Target> targets = new List<Target>();

            foreach (var coll in colls)
            {
                targets.Add(new Target(coll, Vector3.Distance(transform.position, coll.transform.position)));
            }

            List<Target> sortedTraget = targets.OrderBy(x => x.distance).ToList();


            if (sortedTraget.Count > 0)
            {
                int shootCount = GameManager.instance.IsChanceTrue(MultiChance) ? MultiCount : 1;

                if (shootCount > sortedTraget.Count) shootCount = sortedTraget.Count;
               
                // 가까운 순으로 발사수(멀티샷 수)만큼 발사
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

            // 현재의 체력과 최대 체력이 같지 않을 때만
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
        GameManager.instance.ResultPanelSetActive(true);
        Time.timeScale = 0;
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