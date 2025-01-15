using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpText : PoolObject
{
    [SerializeField]    // 올라가는 속도
    float upSpeed;

    [SerializeField]    // 삭제되는 데까지 걸리는 시간
    float deleteTime;

    // 기본 글자 색상
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

        // 바꿔줄 색상
        Color c = textColor;

        while (a > 0)
        {
            if (poolType != PoolObejectType.damageText)
            {
                // 점점 올라감
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
