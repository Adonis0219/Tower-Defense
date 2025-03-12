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
            Debug.Log("코루틴 실행중");

            yield return new WaitForSeconds(-1f);
        }
    }

    public void OnStopClk()
    {
        StopCoroutine(coru);
    }
}
