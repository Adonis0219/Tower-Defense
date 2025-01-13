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
            damage = 3 * (GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.데미지] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.데미지] + 1);
            return damage;
        }
        // 외부에선 설정할 수 없으므로 Set은 없다
    }

    float critChance;

    public float CritChance
    {
        get
        {
            critChance = GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.치명타확률] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.치명타확률] + 1;
            return critChance;
        }
    }

    float critFactor;

    public float CritFactor
    {
        get
        {
            critFactor = 1.2f + .1f * (GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.치명타데미지] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.치명타데미지]);
            return critFactor;
        }
    }

    float atkSpd;

    public float AtkSpd
    {
        get
        {
            atkSpd = 1 + .05f * (GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.공격속도] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.공격속도]);
            return atkSpd;
        }
    }

    float range;

    public float Range
    {
        get 
        {
            range = 20 + .5f * (GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.범위] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.범위]);
            return range; 
        }
    }

    float dmgPerMeter;

    public float DmgPerMeter
    {
        get
        {
            dmgPerMeter = 1 + .008f * (GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.거리당데미지] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.거리당데미지]);
            return dmgPerMeter;
        }
    }

    float multiChance;

    public float MultiChance
    {
        get
        {
            multiChance = .5f * (GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.멀티샷확률] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.멀티샷확률]);
            return multiChance;
        }
    }

    int multiCount;

    public int MultiCount
    {
        get
        {
            multiCount = 2 + GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.멀티샷표적] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.멀티샷표적];
            return multiCount;
        }
    }

    public float bounceChance = 100;
    public int bounceCount = 2;
    public float bounceRange = 1;
    
    [Header("  # Def")]
    [SerializeField]
    float maxHp;

    public float MaxHp
    {
        get 
        { 
            maxHp = 5 * (1 + GameManager.instance.defDollarLevels[(int)DefUpgradeType.체력] + GameManager.instance.defCoinLevels[(int)DefUpgradeType.체력]);
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
            regenHp = .04f * (GameManager.instance.defDollarLevels[(int)DefUpgradeType.체력회복] + GameManager.instance.defCoinLevels[(int)DefUpgradeType.체력회복]);
            return regenHp; 
        }
    }

    [SerializeField]
    float def;

    public float Def
    {
        get
        {
            def = .5f * (GameManager.instance.defDollarLevels[(int)DefUpgradeType.방어력] + GameManager.instance.defCoinLevels[(int)DefUpgradeType.방어력]);
            return def;
        }
    }

    float absDef;
    public float AbsDef
    {
        get
        {
            absDef = .5f * (GameManager.instance.defDollarLevels[(int)DefUpgradeType.절대방어] + GameManager.instance.defCoinLevels[(int)DefUpgradeType.절대방어]);
            return absDef; 
        }
    }

    [Header("  # Def")]
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
        // 범위 조정
        rangeObject.localScale = Vector3.one * Range / 5;        
    }

    bool IsMultishot(float chance)
    {
        float randNum = Random.Range(0f, 100f);

        // 랜덤으로 돌린값이 chance보다 작다면
        // 참 반환 -> 크리티컬이다
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
                int shootCount = IsMultishot(MultiChance) ? MultiCount : 1;

                if (shootCount > sortedTraget.Count) shootCount = sortedTraget.Count;
               
                // 가까운 순으로 발사수(멀티샷 수)만큼 발사
                for (int i = 0; i < shootCount; i++)
                {
                    Shoot(sortedTraget[i].collider.transform.position);
                }

                targets.Clear();
                sortedTraget.Clear();

                yield return new WaitForSeconds(1 / AtkSpd);
            }

            yield return null;
        }
    }

    void Shoot(Vector3 targetPos)
    {
        Transform tempBullet = PoolManager.instance.GetPool(PoolObejectType.bullet).transform;
        tempBullet.SetParent(GameManager.instance.poolManager.GetChild(0));
        tempBullet.position = transform.position;
        tempBullet.up = targetPos - transform.position;
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

    // 피격데미지
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