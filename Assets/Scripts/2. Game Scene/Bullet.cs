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
        StartCoroutine(Delete());

        isCritBullet = IsCritical(player.CritChance);
        bulletDamage = isCritBullet ? player.Damage * player.CritFactor : player.Damage;
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
            Debug.Log("noawd");
            //PoolManager.instance.SetPool(gameObject, PoolObejectType.bullet);
        }
    }

    bool IsCritical(float chance)
    {
        float randNum = Random.Range(0f, 100f);

        // �������� �������� chance���� �۴ٸ�
        // �� ��ȯ -> ũ��Ƽ���̴�
        if (chance > randNum)
        {
            return true;
        }
        else return false;
    }

    // 2�� �ڿ� �ڽ��� ����
    IEnumerator Delete()
    {
        yield return new WaitForSeconds(2f);

        PoolManager.instance.SetPool(gameObject, PoolObejectType.bullet);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� ���濡�Լ� IHit�� �����´�
        IHit hitObj = collision.GetComponent<IHit>();
        
        // �ǰ� �����ϸ�
        if (hitObj != null ) 
        {
            // hitObj�� Hit�Լ� ����
            hitObj.Hit(bulletDamage);
        }

        GameObject tempDamageText = PoolManager.instance.GetPool(PoolObejectType.damageText);
        tempDamageText.transform.position = this.transform.position;

        TextMeshPro tempDamageTextMesh = tempDamageText.GetComponent<TextMeshPro>();
        tempDamageTextMesh.text = bulletDamage.ToString();
        tempDamageTextMesh.color = isCritBullet ? Color.red : Color.white;

        // ���� �̱���
        PoolManager.instance.SetPool(gameObject, PoolObejectType.bullet);
    }
}
