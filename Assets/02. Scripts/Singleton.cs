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
                // 씬 내에서 T 타입을 가진 객체를 찾아서 초기화
                instance = (T)FindObjectOfType(typeof(T));

                // 그래도 없다면
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
            // 파괴 불가 오브젝트로 만들기
            // 부모가 존재하고, 최상위가 존재한다면
            if (transform.parent != null && transform.root != null)
            {
                DontDestroyOnLoad(this.transform.root.gameObject);
            }
            else
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
        // 항상 하나만 존재하게 하기 위해 이미 만들어진 instance가 존재
        else
        {
            Destroy(this.gameObject);
            return;
        }        
    }
}
