using System.Collections;
using TMPro;
using Unity.VisualScripting;
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

        isBouncBullet = IsChanceTrue(player.BounceChance);
        // 매개변수로 크확을 넣었으므로 -> 크리티컬인가?
        isCritBullet = IsChanceTrue(player.CritChance);
        bulletDmg = player.Damage;

        bounceCount = player.BounceCount;
    }

    [SerializeField]
    float lineSize = 1000f;

    // Update is called once per frame
    void Update()
    {
        transform.up = target.position - transform.position;
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    /// <summary>
    /// 각 확률에 따라 발동 됐는지
    /// </summary>
    /// <param name="chance">발동 확률</param>
    /// <returns></returns>
    bool IsChanceTrue(float chance)
    {
        float randNum = Random.Range(0f, 100f);

        // 랜덤으로 돌린값이 chance보다 작다면
        // 참 반환 -> 확률 터짐
        if (chance >= randNum)
        {
            return true;
        }
        else return false;
    }

    // 2초 뒤에 자신을 삭제
    IEnumerator Delete()
    {
        yield return new WaitForSeconds(4f);
        ReturnPool();
    }

    Collider2D[] colliders;

    private void OnTriggerEnter2D(Collider2D collision)
    {       
        // 맞은 상대방에게서 IHit을 가져온다
        IHit hitObj = collision.GetComponent<IHit>();

        if (hitObj == null)
            return;

        // 총알이 사라질 때 총알의 위치 = 적이 죽은 위치 
        // 플레이어와의 거리 -> 데미지/미터 적용
        float dist = Vector3.Distance(transform.position, player.transform.position) * 10;

        // 거리 데미지 적용
        float distDmg = bulletDmg * Mathf.Pow(player.DmgPerMeter, dist);
        // 크리 데미지 적용
        float finalDmg = isCritBullet ? distDmg * player.CritFactor : distDmg;

        // hitObj의 Hit함수 실행
        hitObj.Hit(finalDmg);

        player.LifeSteal(finalDmg);

        // 부딪힌 적의 액티브가 꺼져있지 않다면 -> 살아있다면
        if (collision.transform.gameObject.activeSelf == true) SpawnUpText(finalDmg);

        // 바운스 총알이고
        if (isBouncBullet && bounceCount > 0)
        {
                bounceCount--;

                // 바운스범위 내의 적 모두 찾기
                colliders = Physics2D.OverlapCircleAll(transform.position, player.BounceRange / 10, player.enemyMask);
                    
                // 가까운 순으로 정렬
                SortArrayByNearest();

                if (colliders.Length == 0)
                {
                    ReturnPool();
                    return;
                }

                if (colliders[0] == collision)
                {
                    if (colliders.Length == 1)
                    {
                        ReturnPool();
                    }
                    else
                    {
                        target = colliders[1].transform;
                    }
                }
                else
                {
                    target = colliders[0].transform;
                }          
        }
        else
        {
            ReturnPool();
        }
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
        if (colliders.Length == 0)
            return;

        Transform nearestTarget = colliders[0].transform;
        float nearestDist = Vector3.Distance(nearestTarget.position, transform.position);
        float checkDist;

        for (int i = 1; i < colliders.Length; i++)
        {
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
