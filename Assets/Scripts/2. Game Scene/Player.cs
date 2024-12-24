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
            damage = 3 * (GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.데미지] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.데미지] + 1);
            return damage;
        }
        // 외부에선 설정할 수 없으므로 Set은 없다
    }

    [SerializeField]    // 크리티컬 확률
    float critChance;

    public float CritChance
    {
        get
        {
            critChance = GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.치명타확률] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.치명타확률] + 1;
            return critChance;
        }
    }

    [SerializeField]    // 크리티컬 배율
    float critFactor;

    public float CritFactor
    {
        get
        {
            critFactor = 1.2f + .1f * (GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.치명타데미지] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.치명타데미지]);
            return critFactor;
        }
    }

    [SerializeField]
    float atkSpd;

    public float AtkSpd
    {
        get
        {
            atkSpd = 1 + .05f * (GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.공격속도] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.공격속도]);
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
        // 범위 조정
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
            // 범위 내의 collider 모두 가져오기
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, enemyMask);
            // 콜라이더가 있다면 -> 적이 있다면
            if (colliders.Length > 0 && Vector3.Distance(colliders[0].transform.position, this.transform.position) <= radius)
            {
                // 가장 가까운 타겟
                nearestTarget = colliders[0].transform;
                // 가장 가까운 타겟과의 거리
                float nearestDist = Vector3.Distance(colliders[0].transform.position, this.transform.position);

                for (int i = 1; i < colliders.Length; i++)
                {
                    // 가장 가까운 타겟과의 거리보다 i번째와의 거리가 더 짧으면
                    if (Vector3.Distance(colliders[i].transform.position, this.transform.position) < nearestDist)
                    {
                        nearestDist = Vector3.Distance(colliders[i].transform.position, this.transform.position);
                        // 가장 가까운 타겟은 i이다
                        nearestTarget = colliders[i].transform;
                    }
                }

                // 첫번째 적에게 총알 발사
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

            // 현재의 체력과 최대 체력이 같지 않을 때만
            if (CurrentHp != MaxHp)
                CurrentHp += RegenHp;
        }
    }    

    public void Hit(float damage)
    {
        CurrentHp -= (damage * (1 - Def)) - AbsDef;
        //CurrentHp -= damage;
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
