using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // �� ������ T Ÿ���� ���� ��ü�� ã�Ƽ� �ʱ�ȭ
                instance = (T)FindObjectOfType(typeof(T));

                // �׷��� ���ٸ�
                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    instance = obj.GetComponent<T>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = (T)FindObjectOfType(typeof(T));
            // �ı� �Ұ� ������Ʈ�� �����
            // �θ� �����ϰ�, �ֻ����� �����Ѵٸ�
            if (transform.parent != null && transform.root != null)
            {
                DontDestroyOnLoad(this.transform.root.gameObject);
            }
            else
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
        // �׻� �ϳ��� �����ϰ� �ϱ� ���� �̹� ������� instance�� ����
        else
        {
            Destroy(this.gameObject);
            return;
        }        
    }
}
