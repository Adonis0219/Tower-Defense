using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

class T
{
    public List<int> nums = new List<int>();
}

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //// ?.
        T test1 = new T();
        Debug.Log(test1?.nums);

        T test2 = null;
        Debug.Log(test2?.nums);


        ///// ??
        // obj1에는 새로운 Dictionary를 초기화 한다
        object obj1 = new Dictionary<int, int>();
        // a에 obj1을 초기화한다
        // 만약 obj1이 null이라면 새로운 List를 할당하자
        object a = obj1 ?? new List<int>();
        Debug.Log(a);       // Dictionary 출력됨
        // obj1은 null이 아니므로 오른쪽 값이 아닌 Dictionary를 출력한다

        // obj2는 아무것도 초기화 하지 않았다.
        object obj2 = null;
        // b에 obj2를 초기화 하는데 obj2는 null이므로 오른쪽 값인 List를 새로 할당해준다
        object b = obj2 ?? new List<int>();
        // 리스트가 출력
        Debug.Log(b);

        //////// ?. ?? 복합 사용
        List<int> aa = new List<int>();

        aa.Add(5);
        aa.Add(4);

        Debug.Log(ReturnCount(aa));

        List<int> bb = null;

        Debug.Log(ReturnCount(bb));
    }

    int ReturnCount<T>(List<T> list)
    {
        return list?.Count ?? -1;   // -1은 null임을 의미
    }
}
