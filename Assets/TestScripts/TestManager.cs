using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    [SerializeField]
    Transform grandFather;

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log(grandFather.GetChild(grandFather.childCount - 1).name);
        //if (grandFather.GetChild(grandFather.childCount - 1).childCount == 1)
        //{
        //    Debug.Log("자식 한 명");

        //    for (int i = 0; i < grandFather.GetChild(grandFather.childCount - 1).childCount; i++)
        //    {
        //        Debug.Log(grandFather.GetChild(grandFather.childCount - 1).GetChild(i).name);
        //    }
        //}
        //else
        //{
        //    Debug.Log("자식 두 명");

        //    for (int i = 0; i < grandFather.GetChild(grandFather.childCount - 1).childCount; i++)
        //    {
        //        Debug.Log(grandFather.GetChild(grandFather.childCount - 1).GetChild(i).name);
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DestroyImmediate(grandFather.GetChild(grandFather.childCount - 1).gameObject);
            Debug.Log(grandFather.GetChild(grandFather.childCount - 1).name);
        }
    }
}
