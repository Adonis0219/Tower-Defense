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

        if (hitObj == null)
            return;

        // �Ѿ��� ����� �� �Ѿ��� ��ġ = ���� ���� ��ġ 
        // �÷��̾���� �Ÿ� -> ������/���� ����
        float dist = Vector3.Distance(transform.position, player.transform.position) * 10;

        // �Ÿ� ������ ����
        float distDmg = bulletDmg * Mathf.Pow(player.DmgPerMeter, dist);
        // ũ�� ������ ����
        float finalDmg = isCritBullet ? distDmg * player.CritFactor : distDmg;

        // hitObj�� Hit�Լ� ����
        hitObj.Hit(finalDmg);

        GameObject tempDamageText = PoolManager.instance.GetPool(PoolObejectType.damageText);
        tempDamageText.transform.position = this.transform.position;

        TextMeshPro tempDamageTextMesh = tempDamageText.GetComponent<TextMeshPro>();
        tempDamageTextMesh.text = finalDmg.ToString("F2");
        tempDamageTextMesh.color = isCritBullet ? Color.red : Color.white;

        // ���� �̱���
        PoolManager.instance.SetPool(gameObject, PoolObejectType.bullet);
    }
}
