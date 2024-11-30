using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]  // 직렬화 가능하도록 해주는 헤더
public class PoolObjectData
{
    public int initCount;   // 초기에 생성할 개수
    public GameObject original; // 생성할 오브젝트의 원본
}

// 생성할 풀 오브젝트의 타입 열거형
public enum PoolObejectType
{
    bullet, normal, speed, range, dollarText, coinText, damageText
}

public class PoolManager : MonoBehaviour
{
    // 여러 곳에서 사용하기 위해 싱글톤
    public static PoolManager instance;

    // poolObjectData 클래스를 사용하기 위해 변수 선언
    public PoolObjectData[] poolObjDatas;

    Queue<GameObject>[] objectQ;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        for (int i = 0; i < poolObjDatas.Length; i++)
        {
            // 새로운 empty를 만들어 그것을 오리지널의 이름으로 한다
            GameObject parent = new GameObject();
            parent.transform.SetParent(transform);
            parent.name = poolObjDatas[i].original.name;
        }

        Init();
    }


    // 오브젝트 풀 초기세팅 함수
    void Init()
    {
        // 큐의 배열 초기화, 길이 = 오브젝트 데이터의 길이
        objectQ = new Queue<GameObject>[poolObjDatas.Length];

        // 오브젝트큐 배열에 PoolDatas의 길이만큼 새로운 큐 할당
        for (int i = 0; i < poolObjDatas.Length; i++)
        {
            objectQ[i] = new Queue<GameObject>();

            // 큐배열의 i번째 큐에 PoolDatas의 초기생성 개수만큼 생성하여 큐에 넣어주기 
            for (int j = 0; j < poolObjDatas[i].initCount; j++)
            {
                objectQ[i].Enqueue(CreateObject(i));
            }
        }
    }

    GameObject CreateObject(int index)
    {
        // 만든 오브젝트
        // PoolDatas 배열의 index 번째의 원본을 복제하여 자신의 자식으로 설정한 후 반환한다
        GameObject createObject = Instantiate(poolObjDatas[index].original, transform.GetChild(index));

        return createObject;
    }

    // 가져오는 것이니 GameObject를 반환
    public GameObject GetPool(PoolObejectType type)
    {
        int i = (int)type;

        // 반환할 오브젝트
        GameObject returnObject;

        // 큐에 반환할 오브젝트가 없다면 
        if (objectQ[i].Count == 0)
        {
            // 새로 만들어서 반환해준다
            returnObject = CreateObject(i);
        }
        // 큐에 ROB가 있다면
        else
        {
            // type의 큐에서 빼서 반환해주기
            returnObject = objectQ[i].Dequeue();
        }

        // 뺀 오브젝트 활성화 시켜주기
        returnObject.gameObject.SetActive(true);
        return returnObject;
    }

    /// <summary>
    /// Pool에 넣어주는 함수
    /// </summary>
    /// <param name="setObject">넣어줄 오브젝트</param>
    /// <param name="type">넣어줄 Pool의 타입</param>
    public void SetPool(GameObject setObject, PoolObejectType type)
    {
        int i = (int)type;

        // 넣어줄 것이니 오브젝트를 꺼주기
        setObject.gameObject.SetActive(false);
        // 큐에 셋 오브젝트 넣어주기
        objectQ[i].Enqueue(setObject);
    }
}
