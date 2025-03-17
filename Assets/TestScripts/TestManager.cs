using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TestManager : Singleton<TestManager>
{
    int[] test;

    private void Start()
    {
        int[] print = test = new int[3] { 1, 2, 3 };

        for (int i = 0; i < print.Length; i++)
        {
            Debug.Log(print[i]);
        }

        for (int j = 0; j < test.Length; j++)
        {
            Debug.Log(test[j]);
        }
    }
}
