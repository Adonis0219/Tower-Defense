using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum TTTT
{
    a, b, c, d, e, f
}

public class T : MonoBehaviour
{

    [SerializeField]
    int num = 50;

    private void Start()
    {
        StartCoroutine(WFS());
        StartCoroutine(WFSRT());
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                Time.timeScale = i + 1;
            }
        }
    }

    IEnumerator WFS()
    {
        while (true)
        {

            yield return new WaitForSeconds(5);

            Debug.Log("wfs");
        }
    }

    IEnumerator WFSRT()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(5);
            Debug.Log("wfsrt");

        }
    }
}
