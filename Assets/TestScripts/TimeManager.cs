using System;
using Unity.VisualScripting;
using UnityEngine;

public class TimeData
{
    public string exitTime;
}

public class TimeManager : MonoBehaviour
{
    TimeData timeData;

    const string SAVE_DATA_KEY = "SaveData";

    DateTime accessTime;
    DateTime exitTime;
    TimeSpan timeDif;

    private void Awake()
    {
        LoadData();

        exitTime = Convert.ToDateTime(timeData.exitTime);
        accessTime = DateTime.Now;

        timeDif = accessTime - exitTime;

       // Debug.Log(DisplayTime((int)timeDif.TotalSeconds));
    }

    public string DisplayTime(float reqTime)
    {
        DisplayTime dispTime = new DisplayTime();
        string dispTimeStr = "";

        dispTime.d = Mathf.FloorToInt(reqTime / 86400);
        dispTime.h = Mathf.FloorToInt((reqTime - dispTime.d * 86400) / 3600);
        dispTime.m = Mathf.FloorToInt(reqTime / 60 % 60);
        dispTime.s = (int)(reqTime % 60);

        // �ϼ��� �������� �ʴ´ٸ�
        if (dispTime.d == 0)
        {
            // �� �� �� ǥ��
            dispTimeStr = dispTime.h.ToString("D2") + "h "
            + dispTime.m.ToString("D2") + "m " + dispTime.s.ToString("D2") + "s";

            // �ϼ��� ����, �ð��� ���ٸ�
            if (dispTime.h == 0)
            {
                // �� �� ǥ��
                dispTimeStr = dispTime.m.ToString("D2") + "m " + dispTime.s.ToString("D2") + "s";
            }
        }
        // �ϼ��� �����Ѵٸ�
        else
        {
            // �� �� �б����� ǥ��
            dispTimeStr = dispTime.d.ToString() + "d " + dispTime.h.ToString("D2") + "h "
                + dispTime.m.ToString("D2") + "m";
        }

        return dispTimeStr;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void LoadData()
    {
        string loadJD = PlayerPrefs.GetString(SAVE_DATA_KEY, "");
        timeData = JsonUtility.FromJson<TimeData>(loadJD);

        if (timeData == null)
        {
            timeData = new TimeData();
        }
    }

    public void SaveData()
    {
        string saveJD = JsonUtility.ToJson(timeData);
        PlayerPrefs.SetString(SAVE_DATA_KEY, saveJD);
    }

    private void OnApplicationQuit()
    {
        // ������ ���� �� �� ���� �ð� ����
        timeData.exitTime = DateTime.Now.ToString();

        SaveData();
    }
}
