using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �߰�����(������)
public class MyNumber
{
    public int ����;
    public float ����;
}

public class ParseNumber 
{
    public static string Parse(int num)
    {
        int ���� = 7;
        float ���� = 5.88435f;

        string ���� = "";

        if (6 <= ����)
        {
            ���� -= 6;

            for (int i = 0; i < ����; i++)
            {
                ���� *= 10;
            }
            ���� = "M";
        }
        else if (3 <= ����)
        {
            ���� -= 3;

            for (int i = 0; i < ����; i++)
            {
                ���� *= 10;
            }
            ���� = "K";
        }

        return ����.ToString("F2") + ����;
    }
}
