using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 구독자B : MonoBehaviour
{
    
    public 유튜버 유;

    void Start()
    {
        유.알람 += 댓글달기;
        //유.알람 -= 댓글달기;
    }

    void 댓글달기(int a)
    {
        Debug.Log("B는 댓글달기~");
        Debug.Log(a);
    }
}
