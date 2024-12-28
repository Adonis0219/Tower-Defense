using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 추가사항(집에서)
public class MyNumber
{
    public int 지수;
    public float 숫자;
}

public class ParseNumber 
{
    public static string Parse(int num)
    {
        int 지수 = 7;
        float 숫자 = 5.88435f;

        string 단위 = "";

        if (6 <= 지수)
        {
            지수 -= 6;

            for (int i = 0; i < 지수; i++)
            {
                숫자 *= 10;
            }
            단위 = "M";
        }
        else if (3 <= 지수)
        {
            지수 -= 3;

            for (int i = 0; i < 지수; i++)
            {
                숫자 *= 10;
            }
            단위 = "K";
        }

        return 숫자.ToString("F2") + 단위;
    }
}
