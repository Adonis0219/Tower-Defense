using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 구독자C : MonoBehaviour
{
    void Start()
    {
        유튜버.Instance.알람 += 좋아요누르기;
    }

    void 좋아요누르기(int a)
    {
        Debug.Log("C는 좋아요누르기~");
    }
}
