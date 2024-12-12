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
