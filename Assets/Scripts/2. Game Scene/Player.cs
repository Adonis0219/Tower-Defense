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

            // 체력회복으로 인해 최대체력보다 현재체력이 커지면 다시 초기화
            if (CurrentHp > MaxHp)
                CurrentHp = MaxHp;

            // 현재체력이 0이하로 내려가면 사망
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
            damage = 3 * (GameManager.instance.atkCoinLevels[(int)AtkUpgradeType.데미지] + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.데미지] + 1);
            return damage;
        }
    }

    [SerializeField]    // 크리티컬 확률
    public float critChance = 1;

    [SerializeField]    // 크리티컬 배율
    public float critFactor = 1.2f;

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

    IEnumerator RegenHp()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            // 현재의 체력과 최대 체력이 같지 않을 때만
            if (CurrentHp != MaxHp)
                CurrentHp += regenHp;
        }
    }    

    public void Hit(float damage)
    {
        CurrentHp -= (damage * (1 - def / 100)) - absDef;
        //CurrentHp -= damage;
    }

    // 피격데미지
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
