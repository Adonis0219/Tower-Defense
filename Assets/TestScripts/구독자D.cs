using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 구독자D : MonoBehaviour
{
    
    public 유튜버 유;

    void Start()
    {
        유.알람 += 영상시청;
        //유.알람 -= 댓글달기;
    }

    void 영상시청(int a)
    {
        Debug.Log("D는 영상시청~");
        Debug.Log(a);
    }
}
