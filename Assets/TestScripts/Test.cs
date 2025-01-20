using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Test : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    float kbForce;  // �˹� �Ŀ�

    bool isKB = false;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isKB)
            return;

        transform.Translate(Vector3.up * Time.deltaTime * speed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(KnockBack());
        }
    }
    IEnumerator KnockBack()
    {
        isKB = true;

        rigid.AddForce(Vector2.down * kbForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(.1f);  // �ϳ��� ���� ������ ������

        rigid.velocity = Vector2.zero;
        isKB = false;
    }
}
