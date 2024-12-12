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

    float bulletDamage;

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    // Start is called before the first frame update
    void Start()
    {
        isCritBullet = IsCritical(player.CritChance);
        StartCoroutine(Delete());
        bulletDamage = isCritBullet ? player.Damage * player.CritFactor : player.Damage;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
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
        
        // 피격 가능하면
        if (hitObj != null ) 
        {
            // hitObj의 Hit함수 실행
            hitObj.Hit(bulletDamage);
        }

        GameObject tempDamageText = PoolManager.instance.GetPool(PoolObejectType.damageText);
        tempDamageText.transform.position = this.transform.position;

        TextMeshPro tempDamageTextMesh = tempDamageText.GetComponent<TextMeshPro>();
        tempDamageTextMesh.text = bulletDamage.ToString();
        tempDamageTextMesh.color = isCritBullet ? Color.red : Color.white;

        // 관통 미구현
        PoolManager.instance.SetPool(gameObject, PoolObejectType.bullet);
    }
}
