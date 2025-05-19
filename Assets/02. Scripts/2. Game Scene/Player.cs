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

    [SerializeField]
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

    [SerializeField]
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

    [SerializeField]
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

    [SerializeField]
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

    [SerializeField]
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

    [SerializeField]
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

    [SerializeField]
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
            return PlayDataManager.Instance.HpRegenFormula(SceneType.Game); 
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

    [SerializeField]
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

            // 플레이어의 타겟들
            List<Target> targets = new List<Target>();

            // 범위 내의 콜라이더를 Target 리스트에 넣어줌
            foreach (var coll in colls)
            {
                // Target 클래스를 생성후 넣어줌
                // coll와 적과 플레이어 사이의 거리
                targets.Add(new Target(coll, Vector3.Distance(transform.position, coll.transform.position)));
            }

            // 람다식을 이용해 targets 리스트를 거리가 가까운 순으로 정렬 후 sortedTarget에 초기화
            List<Target> sortedTarget = targets.OrderBy(x => x.distance).ToList();

            // 적이 있으면
            if (sortedTarget.Count > 0)
            {
                // 멀티샷 확률을 적용하여 발사수를 정해줌
                int shootCount = GameManager.instance.IsChanceTrue(MultiChance) ? MultiCount : 1;

                // 발사수가 범위내의 적보다 많으면 발사수 조정
                if (shootCount > sortedTarget.Count) shootCount = sortedTarget.Count;
               
                // 가까운 순으로 발사수(멀티샷 수)만큼 발사
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
    /// 총알 발사 함수
    /// </summary>
    /// <param name="target">타겟</param>
    void Shoot(Transform target)
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Fire);


        // 총알 생성
        Transform tempBullet = PoolManager.instance.GetPool(PoolObejectType.Bullet).transform;
        // 부모 설정
        tempBullet.SetParent(GameManager.instance.poolManager.GetChild(0));
        // 초기 위치 설정
        tempBullet.position = transform.position;

        tempBullet.up = target.position - transform.position;

        // 바라볼 방향을 위해 Target 설정
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