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
        // obj1���� ���ο� Dictionary�� �ʱ�ȭ �Ѵ�
        object obj1 = new Dictionary<int, int>();
        // a�� obj1�� �ʱ�ȭ�Ѵ�
        // ���� obj1�� null�̶�� ���ο� List�� �Ҵ�����
        object a = obj1 ?? new List<int>();
        Debug.Log(a);       // Dictionary ��µ�
        // obj1�� null�� �ƴϹǷ� ������ ���� �ƴ� Dictionary�� ����Ѵ�

        // obj2�� �ƹ��͵� �ʱ�ȭ ���� �ʾҴ�.
        object obj2 = null;
        // b�� obj2�� �ʱ�ȭ �ϴµ� obj2�� null�̹Ƿ� ������ ���� List�� ���� �Ҵ����ش�
        object b = obj2 ?? new List<int>();
        // ����Ʈ�� ���
        Debug.Log(b);

        //////// ?. ?? ���� ���
        List<int> aa = new List<int>();

        aa.Add(5);
        aa.Add(4);

        Debug.Log(ReturnCount(aa));

        List<int> bb = null;

        Debug.Log(ReturnCount(bb));
    }

    int ReturnCount<T>(List<T> list)
    {
        return list?.Count ?? -1;   // -1�� null���� �ǹ�
    }
}
