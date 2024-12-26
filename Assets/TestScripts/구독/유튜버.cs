using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ��Ʃ�� : MonoBehaviour
{
    static ��Ʃ�� instance = null;

    public static ��Ʃ�� Instance
    {
        get
        {
            //
            if (instance == null)
            {
                // PlayData�� ���� GameObject �˻� �� instance�� �ʱ�ȭ
                instance = FindObjectOfType<��Ʃ��>();

                if (instance == null)
                {
                    GameObject temp = new GameObject();
                    temp.name = "��Ʃ��";
                    instance = temp.AddComponent<��Ʃ��>();

                    // �ı� �Ұ� ������Ʈ�� �����
                    // �����ڿ��� ȣ�� �Ұ�
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
            // �ı� �Ұ� ������Ʈ�� �����
            // �����ڿ��� ȣ�� �Ұ�
            DontDestroyOnLoad(this.gameObject);
        }
        // �׻� �ϳ��� �����ϰ� �ϱ� ����
        // �̹� ������� instance�� ����
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    public Action<int> �˶�;

    public int ���󰳼�;
    
    public void �����󸸵�()
    {
        Debug.Log("����");
        //�����ڵ鿡�� �˶�������!
        //�˶�(���󰳼�);
        �˶�?.Invoke(���󰳼�);
        //setting?.Invoke();
    }

}