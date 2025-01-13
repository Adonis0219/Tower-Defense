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

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Delete());

        isBouncBullet = IsChanceTrue(player.bounceChance);
        // 매개변수로 크확을 넣었으므로 -> 크리티컬인가?
        isCritBullet = IsChanceTrue(player.CritChance);
        bulletDmg = player.Damage;

        bounceCount = player.bounceCount;
    }

    [SerializeField]
    float lineSize = 1000f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        Debug.DrawRay(transform.position, transform.up, Color.yellow, lineSize);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, lineSize);

        if (!hit)
        { 
            PoolManager.instance.SetPool(gameObject, PoolObejectType.bullet);
        }
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
        yield return new WaitForSeconds(2f);

        PoolManager.instance.SetPool(gameObject, PoolObejectType.bullet);
    }

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

        SpawnUpText(finalDmg);

        Transform nearestTarget;

        // 적이 살아 있을 때 논리 추가
        
        // 바운스 총알이고
        if (isBouncBullet)
        {
            if (bounceCount > 0)
            {
                bounceCount--;

                // 바운스범위 내의 적 모두 찾기
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, player.bounceRange, player.enemyMask);

                for (int i = 0; i < colliders.Length; i++)
                {
                    Debug.Log(colliders[i]);
                }

                if (colliders.Length > 1)
                {
                    // 가장 가까운 타겟
                    nearestTarget = colliders[0].transform;
                    // 가장 가까운 타겟과의 거리
                    float nearestDist = Vector3.Distance(colliders[0].transform.position, this.transform.position);

                    for (int j = 1; j < colliders.Length; j++)
                    {
                        // 가장 가까운 타겟과의 거리보다 j번째와의 거리가 더 짧으면
                        if (Vector3.Distance(colliders[j].transform.position, this.transform.position) < nearestDist)
                        {
                            nearestDist = Vector3.Distance(colliders[j].transform.position, this.transform.position);
                            // 가장 가까운 타겟은 i이다
                            nearestTarget = colliders[j].transform;
                        }
                    }

                    transform.up = nearestTarget.position - this.transform.position;
                }
                else
                {
                    PoolManager.instance.SetPool(gameObject, PoolObejectType.bullet);
                    isBouncBullet = false;
                }
                colliders = new Collider2D[0];
            }
            else
            {
                isBouncBullet = false;
                PoolManager.instance.SetPool(gameObject, PoolObejectType.bullet);
            }            
        }
        else
        {
            isBouncBullet= false;
            PoolManager.instance.SetPool(gameObject, PoolObejectType.bullet);
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

    void FindNearestTarget(Collider2D[] colliders)
    {

    }
}
