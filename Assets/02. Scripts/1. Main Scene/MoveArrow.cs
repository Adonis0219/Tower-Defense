using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveArrow : MonoBehaviour
{
    [SerializeField]
    float moveSpd;

    [SerializeField]
    float range;

    Vector3 curPos;

    // Start is called before the first frame update
    void Start()
    {
        curPos = transform.position;
    }

    float t;

    // Update is called once per frame
    void Update()
    {
        Vector3 v = curPos;
        v.y += range * Mathf.Sin(Time.time * moveSpd);
        transform.position = v;
    }
}
