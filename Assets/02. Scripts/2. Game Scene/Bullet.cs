using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : PoolObject
{
    [SerializeField]
    TextMeshPro damageText;
    Player player;
    public Transform target;

    [Header("# Info")]
    [SerializeField]
    float speed;

    bool isCritBullet = false;
    bool isBouncBullet = false;


    int bounceCount;

    // 총알의 기본 대미지
    float bulletDmg;
    // 총알의 최종 대미지
    float finalDmg;

    [Header("# Detect")]
    // 이전 프레임 위치 저장
    Vector3 prePos;
    
    Collider2D enemy;

    // 탐지된 적들
    Collider2D[] detectedEnemies;

    [SerializeField]
    /// <summary>
    /// 이미 맞은 적들
    /// </summary>
    List<Collider2D> hitEnemies = new List<Collider2D>();


    private void Awake()
    {
        player = GameManager.instance.player;
    }

    private void OnEnable()
    {
        speed = 5;

        isBouncBullet = GameManager.instance.IsChanceTrue(player.BounceChance);
        // 매개변수로 크확을 넣었으므로 -> 크리티컬인가?
        isCritBullet = GameManager.instance.IsChanceTrue(player.CritChance);

        bulletDmg = player.Damage;

        bounceCount = player.BounceCount;

        // 맞은 적 초기화 해주기
        hitEnemies.Clear();
    }

    // 감지된 적의 인덱스
    int detectedIndex;

    // Update is called once per frame
    void Update()
    {
        // 날아가는 도중에 타겟이 사라진다면
        if (!target.gameObject.activeSelf)
            ReturnPool();

        //if (!target.gameObject.activeSelf)
        //{
        //    // 바운스 총알이 아니라면 없어지기
        //    if (!isBouncBullet) ReturnPool();
        //    else // 바운스 총알이라면 다음 타겟 찾아가기
        //    {
        //        detectedEnemies = Physics2D.OverlapCircleAll(transform.position,
        //                            player.BounceRange / 10, player.enemyMask);
        //        target = detectedEnemies[0].transform;
        //    }
        //}

            transform.up = target.position - transform.position;

        transform.Translate(Vector3.up * speed * Time.deltaTime);

        Vector3 posChange = prePos - transform.position;

        // 이전 프레임과 현재 프레임 사이의 레이에 맞는 물체 찾기
        RaycastHit2D[] hitObjs = 
            Physics2D.RaycastAll(transform.position, posChange, Vector2.Distance(prePos, transform.position));

        // 맞은 물체 중 적이 있다면
        if (Array.Find(hitObjs, x => x.transform.tag == "Enemy"))
        {
            // 적의 collider2d를 빼와서 저장
            enemy = Array.Find(hitObjs, x => x.transform.tag == "Enemy").transform.GetComponent<Collider2D>();
            EnemyHit();
        }
    }

    private void LateUpdate()
    {
        prePos = transform.position;
    }

    void EnemyHit()
    {
        IHit hitObj = enemy.GetComponent<IHit>();

        BulletDmgFormula();

        // hitObj의 Hit함수 실행
        hitObj.Hit(finalDmg);

        // 이미 맞은 적에 자신 추가
        hitEnemies.Add(enemy);

        // 플레이어 흡혈 발동
        player.LifeSteal(finalDmg);

        // 부딪힌 적의 액티브가 꺼져있지 않다면 -> 살아있다면
        if (enemy.transform.gameObject.activeSelf == true) SpawnUpText(finalDmg);

        // 바운스 총알이고, 바운스 횟수가 남아있다면
        if (isBouncBullet && bounceCount > 0)
        {
            Bounce(enemy);
        }
        else
        {
            ReturnPool();
        }
    }

    void Bounce(Collider2D collision)   
    {
        // 총알 속도 올려주기
        speed = 10;

        bounceCount--;

        // 바운스범위 내의 적 모두 찾기
        detectedEnemies = Physics2D.OverlapCircleAll(transform.position, 
            player.BounceRange / 10, player.enemyMask);

        // 타겟이 없다면 리턴풀 후 얼리
        if (detectedEnemies.Length == 0)
        {
            ReturnPool();
            return;
        }

        // 가까운 순으로 정렬
        SortArrayByNearest();

        // 맞은 적이 죽지 않았다면
        if (detectedEnemies[0] == collision)
        {
            // 범위 내에 죽지 않은 적이 처음 맞은 적밖에 없다면
            if (detectedEnemies.Length == 1)
            {
                ReturnPool();
            }
            else
            {
                // 타겟을 우선 정해주고
                target = detectedEnemies[1].transform;

                // 그 타겟이 맞은 에너미인지 검사
                for (int i = 1; i < detectedEnemies.Length; i++)
                {
                    // 이미 맞았던 적의 리스트에 들어있지 않다면
                    if (!hitEnemies.Contains(detectedEnemies[i]))
                    {
                        target = detectedEnemies[i].transform;
                        break;
                    }
                }
            }
        }
        else // 맞은 적이 죽었다면
        {
            // 다음 적으로 타겟 설정
            target = detectedEnemies[0].transform;

            for (int i = 0; i < detectedEnemies.Length; i++)
            {
                if (!hitEnemies.Contains(detectedEnemies[i]))
                {
                    target = detectedEnemies[i].transform;
                    detectedIndex = i;
                    break;
                }
            }
        }
    }

    void BulletDmgFormula()
    {
        // 플레이어가 존재할 때만 계산
        if (player == null)
            return;

        // 총알이 사라질 때 총알의 위치 = 적이 죽은 위치
        // 플레이어와의 거리 -> 데미지/미터 적용
        float dist = Vector3.Distance(transform.position, player.transform.position) * 10;

        // 거리 데미지 적용
        float distDmg = bulletDmg * Mathf.Pow(player.DmgPerMeter, dist);
        // 크리 데미지 적용
        finalDmg = isCritBullet ? distDmg * player.CritFactor : distDmg;
    }

    void SpawnUpText(float dmg)
    {
        GameObject tempDamageText = PoolManager.instance.GetPool(PoolObejectType.DamageText);
        tempDamageText.transform.position = this.transform.position;

        TextMeshPro tempDamageTextMesh = tempDamageText.GetComponent<TextMeshPro>();
        tempDamageTextMesh.text = dmg.ToString("F2");
        tempDamageTextMesh.color = isCritBullet ? Color.red : Color.white;
    }

    void SortArrayByNearest()
    {      
        Transform nearestTarget = detectedEnemies[0].transform;
        float nearestDist = Vector3.Distance(nearestTarget.position, transform.position);
        float checkDist;

        for (int i = 1; i < detectedEnemies.Length; i++)
        {
            // 체크할 거리 설정
            checkDist = Vector3.Distance(detectedEnemies[i].transform.position, transform.position);

            // 체크 거리가 더 짧다면
            if (checkDist < nearestDist)
            {
                // 배열의 첫번째와 자신을 바꿔줌
                Swap(i);
            }
        }
    }

    void Swap(int swapIndex)
    {
        Collider2D temp = detectedEnemies[0];
        detectedEnemies[0] = detectedEnemies[swapIndex];
        detectedEnemies[swapIndex] = temp;
    }
}
