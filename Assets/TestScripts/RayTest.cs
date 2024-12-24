using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTest : MonoBehaviour
{
    [SerializeField]
    float lineSize = 10f;

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.up, Color.yellow, lineSize);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.up, out hit, lineSize))
        {
            Debug.Log("닿음");
            // 닿은 물체의 이름을 출력
            Debug.Log(hit.collider.gameObject.name);
        }
    }
}
