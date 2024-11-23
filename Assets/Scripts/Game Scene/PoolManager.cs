using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]  // ����ȭ �����ϵ��� ���ִ� ���
public class PoolObjectData
{
    public int initCount;   // �ʱ⿡ ������ ����
    public GameObject original; // ������ ������Ʈ�� ����
}

// ������ Ǯ ������Ʈ�� Ÿ�� ������
public enum PoolObejectType
{
    bullet, normal, speed, range, dollarText, coinText, damageText
}

public class PoolManager : MonoBehaviour
{
    // ���� ������ ����ϱ� ���� �̱���
    public static PoolManager instance;

    // poolObjectData Ŭ������ ����ϱ� ���� ���� ����
    public PoolObjectData[] poolObjDatas;

    Queue<GameObject>[] objectQ;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        for (int i = 0; i < poolObjDatas.Length; i++)
        {
            // ���ο� empty�� ����� �װ��� ���������� �̸����� �Ѵ�
            GameObject parent = new GameObject();
            parent.transform.SetParent(transform);
            parent.name = poolObjDatas[i].original.name;
        }

        Init();
    }


    // ������Ʈ Ǯ �ʱ⼼�� �Լ�
    void Init()
    {
        // ť�� �迭 �ʱ�ȭ, ���� = ������Ʈ �������� ����
        objectQ = new Queue<GameObject>[poolObjDatas.Length];

        // ������Ʈť �迭�� PoolDatas�� ���̸�ŭ ���ο� ť �Ҵ�
        for (int i = 0; i < poolObjDatas.Length; i++)
        {
            objectQ[i] = new Queue<GameObject>();

            // ť�迭�� i��° ť�� PoolDatas�� �ʱ���� ������ŭ �����Ͽ� ť�� �־��ֱ� 
            for (int j = 0; j < poolObjDatas[i].initCount; j++)
            {
                objectQ[i].Enqueue(CreateObject(i));
            }
        }
    }

    GameObject CreateObject(int index)
    {
        // ���� ������Ʈ
        // PoolDatas �迭�� index ��°�� ������ �����Ͽ� �ڽ��� �ڽ����� ������ �� ��ȯ�Ѵ�
        GameObject createObject = Instantiate(poolObjDatas[index].original, transform.GetChild(index));

        return createObject;
    }

    // �������� ���̴� GameObject�� ��ȯ
    public GameObject GetPool(PoolObejectType type)
    {
        int i = (int)type;

        // ��ȯ�� ������Ʈ
        GameObject returnObject;

        // ť�� ��ȯ�� ������Ʈ�� ���ٸ� 
        if (objectQ[i].Count == 0)
        {
            // ���� ���� ��ȯ���ش�
            returnObject = CreateObject(i);
        }
        // ť�� ROB�� �ִٸ�
        else
        {
            // type�� ť���� ���� ��ȯ���ֱ�
            returnObject = objectQ[i].Dequeue();
        }

        // �� ������Ʈ Ȱ��ȭ �����ֱ�
        returnObject.gameObject.SetActive(true);
        return returnObject;
    }

    /// <summary>
    /// Pool�� �־��ִ� �Լ�
    /// </summary>
    /// <param name="setObject">�־��� ������Ʈ</param>
    /// <param name="type">�־��� Pool�� Ÿ��</param>
    public void SetPool(GameObject setObject, PoolObejectType type)
    {
        int i = (int)type;

        // �־��� ���̴� ������Ʈ�� ���ֱ�
        setObject.gameObject.SetActive(false);
        // ť�� �� ������Ʈ �־��ֱ�
        objectQ[i].Enqueue(setObject);
    }
}
