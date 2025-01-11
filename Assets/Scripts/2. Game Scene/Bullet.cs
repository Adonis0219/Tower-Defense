using System.Collections;
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

    float bulletDmg;

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Delete());

        isCritBullet = IsCritical(player.CritChance);
        bulletDmg = player.Damage;
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

    bool IsCritical(float chance)
    {
        float randNum = Random.Range(0f, 100f);

        // 랜덤으로 돌린값이 chance보다 작다면
        // 참 반환 -> 크리티컬이다
        if (chance > randNum)
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

        GameObject tempDamageText = PoolManager.instance.GetPool(PoolObejectType.damageText);
        tempDamageText.transform.position = this.transform.position;

        TextMeshPro tempDamageTextMesh = tempDamageText.GetComponent<TextMeshPro>();
        tempDamageTextMesh.text = finalDmg.ToString("F2");
        tempDamageTextMesh.color = isCritBullet ? Color.red : Color.white;

        // 관통 미구현
        PoolManager.instance.SetPool(gameObject, PoolObejectType.bullet);
    }
}
