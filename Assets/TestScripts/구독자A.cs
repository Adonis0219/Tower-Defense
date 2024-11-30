using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 구독자A : MonoBehaviour
{
    void Start()
    {
        유튜버.Instance.알람 += 영상보기;
    }
 
    void 영상보기(int a)
    {
        Debug.Log("A는 영상봐~");
    }

    private void OnDestroy()
    {
        Debug.Log("파괴됨");
        유튜버.Instance.알람 -= 영상보기;
    }
}
