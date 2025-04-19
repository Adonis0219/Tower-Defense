using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bullet : PoolObject
{
    [SerializeField]
    float speed;

    [SerializeField]
    TextMeshPro damageText;

    Player player;

    bool isCritBullet = false;
    bool isBouncBullet;
    int bounceCount;

    float bulletDmg;

    public Transform target;

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Delete());

        speed = 15;

        isBouncBullet = GameManager.instance.IsChanceTrue(player.BounceChance);
        // 매개변수로 크확을 넣었으므로 -> 크리티컬인가?
        isCritBullet = GameManager.instance.IsChanceTrue(player.CritChance);
        bulletDmg = player.Damage;

        bounceCount = player.BounceCount;
    }

    [SerializeField]
    //float lineSize = 1000f;

    // Update is called once per frame
    void Update()
    {
        transform.up = target.position - transform.position;
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    // 2초 뒤에 자신을 삭제
    IEnumerator Delete()
    {
        yield return new WaitForSeconds(4f);
        ReturnPool();
    }

    Collider2D[] colliders;
    List<Collider2D> hitEnemies = new List<Collider2D>();

    // 총알의 최종 대미지
    float finalDmg;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 맞은 상대방에게서 IHit을 가져온다
        IHit hitObj = collision.GetComponent<IHit>();

        if (hitObj == null)
            return;

        BulletDmgFormula();

        // hitObj의 Hit함수 실행
        hitObj.Hit(finalDmg);

        // 이미 맞은 적에 자신 추가
        hitEnemies.Add(collision);

        // 플레이어 흡혈 발동
        player.LifeSteal(finalDmg);

        // 부딪힌 적의 액티브가 꺼져있지 않다면 -> 살아있다면
        if (collision.transform.gameObject.activeSelf == true) SpawnUpText(finalDmg);

        // 바운스 총알이고, 바운스 횟수가 남아있다면
        if (isBouncBullet && bounceCount > 0)
        {
            Bounce(collision);
        }
        else
        {
            ReturnPool();
        }
    }

    void Bounce(Collider2D collision)   
    {
        bounceCount--;

        // 바운스범위 내의 적 모두 찾기
        colliders = Physics2D.OverlapCircleAll(transform.position, 
            player.BounceRange / 10, player.enemyMask);

        // 타겟이 없다면 리턴풀 후 얼리
        if (colliders.Length == 0)
        {
            ReturnPool();
            return;
        }

        // 가까운 순으로 정렬
        SortArrayByNearest();

        // 맞은 적이 죽지 않았다면
        if (colliders[0] == collision)
        {
            // 범위 내에 죽지 않은 적이 처음 맞은 적밖에 없다면
            if (colliders.Length == 1)
            {
                ReturnPool();
            }
            else
            {
                // 타겟을 우선 정해주고
                target = colliders[1].transform;

                // 그 타겟이 맞은 에너미인지 검사
                for (int i = 1; i < colliders.Length; i++)
                {
                    // 이미 맞았던 적의 리스트에 들어있지 않다면
                    if (!hitEnemies.Contains(colliders[i]))
                    {
                        target = colliders[i].transform;
                        break;
                    }
                }
            }
        }
        else // 맞은 적이 죽었다면
        {
            // 다음 적으로 타겟 설정
            target = colliders[0].transform;

            for (int i = 0; i < colliders.Length; i++)
            {
                if (!hitEnemies.Contains(colliders[i]))
                {
                    target = colliders[i].transform;
                    break;
                }
            }
        }
    }

    void BulletDmgFormula()
    {
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
        GameObject tempDamageText = PoolManager.instance.GetPool(PoolObejectType.damageText);
        tempDamageText.transform.position = this.transform.position;

        TextMeshPro tempDamageTextMesh = tempDamageText.GetComponent<TextMeshPro>();
        tempDamageTextMesh.text = dmg.ToString("F2");
        tempDamageTextMesh.color = isCritBullet ? Color.red : Color.white;
    }

    void SortArrayByNearest()
    {      
        Transform nearestTarget = colliders[0].transform;
        float nearestDist = Vector3.Distance(nearestTarget.position, transform.position);
        float checkDist;

        for (int i = 1; i < colliders.Length; i++)
        {
            // 체크할 거리 설정
            checkDist = Vector3.Distance(colliders[i].transform.position, transform.position);

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
        Collider2D temp = colliders[0];
        colliders[0] = colliders[swapIndex];
        colliders[swapIndex] = temp;
    }
}
