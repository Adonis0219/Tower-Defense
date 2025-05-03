using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    private void Start()
    {
        float num = 123.456f;
        Debug.Log(num);
        Debug.Log((int)num);

        Debug.Log(ChangeNum((int)12.34));
    }

    string ChangeNum(double num)
    {
        string strNum = num.ToString();
        string retStr = "";

        char[] symbols = { 'K', 'M', 'B', 'T' };
        // ´ÜÀ§
        int unit = 0;

        while (strNum.Length > 6)
        {
            unit++;
            strNum = strNum.Substring(0, strNum.Length - 3);
        }

        if (strNum.Length > 3)
        {
            int newInt = int.Parse(strNum);

            retStr = (newInt / 1000f).ToString("0.00") + symbols[unit];

            return retStr;
        }
        else
        {
            return strNum;
        }
    }

}
