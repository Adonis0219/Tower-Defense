using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TestManager : Singleton<TestManager>
{
    Coroutine coru;

    private void Start()
    {
        coru = StartCoroutine(TT());
    }

    public IEnumerator TT()
    {
        while (true)
        {
            Debug.Log("�ڷ�ƾ ������");

            yield return new WaitForSeconds(-1f);
        }
    }

    public void OnStopClk()
    {
        StopCoroutine(coru);
    }
}
