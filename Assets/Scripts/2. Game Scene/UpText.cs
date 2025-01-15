using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpText : PoolObject
{
    [SerializeField]    // �ö󰡴� �ӵ�
    float upSpeed;

    [SerializeField]    // �����Ǵ� ������ �ɸ��� �ð�
    float deleteTime;

    // �⺻ ���� ����
    Color textColor;

    TextMeshPro textMeshPro;

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        textColor = textMeshPro.color;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(DisAppearUpText());
    }

    IEnumerator DisAppearUpText()
    {
        float a = 1;

        // �ٲ��� ����
        Color c = textColor;

        while (a > 0)
        {
            if (poolType != PoolObejectType.damageText)
            {
                // ���� �ö�
                transform.position += Vector3.up * upSpeed * Time.deltaTime;
            }

            c.a = a;

            a -= Time.deltaTime / deleteTime;

            textMeshPro.color = c;

            yield return null;  
        }

        ReturnPool();
    }
}
