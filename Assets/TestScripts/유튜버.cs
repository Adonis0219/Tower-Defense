using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class 유튜버 : MonoBehaviour
{
    static 유튜버 instance = null;

    public static 유튜버 Instance
    {
        get
        {
            //
            if (instance == null)
            {
                // PlayData를 가진 GameObject 검색 후 instance에 초기화
                instance = FindObjectOfType<유튜버>();

                if (instance == null)
                {
                    GameObject temp = new GameObject();
                    temp.name = "유튜버";
                    instance = temp.AddComponent<유튜버>();

                    // 파괴 불가 오브젝트로 만들기
                    // 생성자에서 호출 불가
                    DontDestroyOnLoad(temp);
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // 파괴 불가 오브젝트로 만들기
            // 생성자에서 호출 불가
            DontDestroyOnLoad(this.gameObject);
        }
        // 항상 하나만 존재하게 하기 위해
        // 이미 만들어진 instance가 존재
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    public Action<int> 알람;

    public int 영상개수;
    
    public void 새영상만듦()
    {
        Debug.Log("만듦");
        //구독자들에게 알람보내기!
        //알람(영상개수);
        알람?.Invoke(영상개수);
        //setting?.Invoke();
    }

}