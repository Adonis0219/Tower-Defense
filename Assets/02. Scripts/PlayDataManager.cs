using Newtonsoft.Json;
using System;
using TMPro;
using UnityEngine;
public class PlayData
{
    public bool isAdmin = false;

    // �÷��̾ ���� ���μ�
    public int haveCoin = 0;

    public int mainDia = 0;

    // ���� ��Ʈ����ũ Ȱ��
    public int achive = 0;
    // �̹� Ʃ�丮���� �Ϸ� �ߴ°�
    public int alreadyTuto = 0;

    public int bestWave = 0;
    
    // ����ȭ���� �ǳ� ������ ���� �� ������� ���� ��
    public float totalEarnCoin = 0;
    
    // ���� ���׷��̵� Ÿ���� ������ŭ �迭 
    public int[] atkCoinLevels = new int[(int)AtkUpgradeType.Length];
    public int[] defCoinLevels = new int[(int)DefUpgradeType.Length];
    public int[] utilCoinLevels = new int[(int)UtilUpgradeType.Length];

    /// <summary>
    /// �� ������ �� ��ư�� ����
    /// </summary>
    public int[] totalCreatCounts = new int[3] { 4, 2, 0 };
    /// <summary>
    /// ����� �� Unlock��ư�� �ε���
    /// </summary>
    public int[] lineOpenCounts = new int[3];


    [Header("# Lab")]
    public int openLabCount = 1;
    /// <summary>
    /// ������ ���� ��ư�� ���� (Length�� �ݺ��� �����̱� ������ -1)
    /// </summary>
    public int[] openResearchBtCounts = new int[(int)ResearchType.Length];
    public int[,] labResearchLevels = new int[(int)ResearchType.Length, 9];
    public bool[,] isResearching = new bool[(int)ResearchType.Length, 9];

    // ���� �������� ������
    public ResearchData[] isResearchingData = new ResearchData[5];
    public DateTime[] startTimes = new DateTime[5];

    // ������ ���� �ð�
    public float[] labRemainTime = new float[5];
    public float[] fixedLabRemainTime = new float[5];

    public int labCompleteCount = 0;

    [Header("# Card")]
    public int curSlotCount = 1;
    public int maxSlotCount = 20;
    public int[] slotOpneCost = { 50, 100, 200, 300, 400, 500, 600, 750, 1000, 1200, 1400, 1600, 1800, 2000, 2300, 2600, 3000, 3500, 4000};

    public int[] cardLvs;
    // ���� �������� ī�� �����͵� (�ִ� ������ ������ �����Ƿ� �׳� 20)
    public int[] activedCardIDs = new int[20] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};    // -1�� ���� �� ��
}

// � �������� PlayData ���� �����ϵ���
public partial class PlayDataManager : Singleton<PlayDataManager>
{
    public PlayData playData;

    const string SAVE_DATA_KEY = "SaveData";
    const string LAB_DATA_KEY = "LabData";
    public int BestWave
    {
        get { return playData.bestWave; }
        set
        {
            // ���� ���� ������ �ְ� ���̺� ���� ���ٸ�
            if (playData.bestWave < value)
            {
                // �ְ� ���̺긦 ���� ������ ����
                playData.bestWave = value ;

                // �۾��� �����ֱ�
                if (playData.bestWave >= UnlockConditions.BEST_WAVE_WORKSHOP)
                    playData.achive |= 1 << (int)Achive.UnlockWorkShop;

                // ī�� �����ֱ�
                if (playData.bestWave >= UnlockConditions.BEST_WAVE_CARD)
                    playData.achive |= 1 << (int)Achive.UnlockCards;

                // ������ �����ֱ�
                if (playData.bestWave >= UnlockConditions.BEST_WAVE_LAB)
                    playData.achive |= 1 << (int)Achive.UnlockLabs;
            }
        }
    }

    public Action<int> onChangedCoin;
    public Action<int> onChangedDia;

    public int MainCoin
    {
        get { return playData.haveCoin; }
        set 
        { 
            playData.haveCoin = value;
            onChangedCoin?.Invoke(MainCoin);
        }
    }
   
    public float TotalEarnCoin
    {
        get { return playData.totalEarnCoin; }
        set
        {
            playData.totalEarnCoin = value;
        }
    }

    public int MainDia
    {
        get { return playData.mainDia; }
        set
        {
            playData.mainDia = value;
            onChangedDia?.Invoke(MainDia);
        }
    }

    public int LabCompleteCount
    {
        get
        {
            return playData.labCompleteCount;
        }
        
        set
        {
            playData.labCompleteCount = value;

            LabManager.instance.completeCountUI.SetActive(playData.labCompleteCount == 0 ? false : true);

            LabManager.instance.completeCountUI.GetComponentInChildren<TextMeshProUGUI>().text
                = playData.labCompleteCount.ToString();
        }
    }
    
    private void Awake()
    {
        // ������ ����
        Application.targetFrameRate = 60;

        // ���ο� �÷��̵����Ͱ� ������ �� ������ �ε� ���ֱ�
        LoadData();
    }

    public TimeSpan elapsedTime;

    private void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            LabReduceTime(i);
        }
    }

    /// <summary>
    /// ���Կ� �ִ� ī��� �߿� CheckID ī�尡 �ִ���
    /// </summary>
    /// <param name="checkID">Ȯ���� ī���� ID</param>
    /// <returns>T �ִ� F ����</returns>
    public bool CheckCard(CardID id)
    {
        int checkId = (int)id;

        for (int i = 0; i < 20; i++)
        {
            if (PlayDataManager.Instance.playData.activedCardIDs[i] == checkId)
            {
                return true;
            }
        }

        return false;
    }

    void LabReduceTime(int labIndex)
    {
        if (playData.labRemainTime[labIndex] > 0)
        {
            // �ð� �� = ����ð� - ���� �ð�
            elapsedTime = DateTime.Now - playData.startTimes[labIndex];

            playData.labRemainTime[labIndex] = playData.fixedLabRemainTime[labIndex] 
                - (float)elapsedTime.TotalSeconds;

            if (playData.labRemainTime[labIndex] < 0)
            {
                LabCompleteCount++;
            }
        }
    }

    ////// MainScene�� PlayDataManager GOBJ�� Ȱ��ȭ ���� ��(���� �� �������ڸ���), 
    ////// GameScene�� GameManager�� Ȱ��ȭ ���� ��(���Ӿ����� �Ѿ�ڸ���)

    // ���̼������ͷε� �������� playData�� �ʱ�ȭ �����ش�.
    public void LoadData()
    {
        string loadJD = PlayerPrefs.GetString(SAVE_DATA_KEY, "");
        playData = JsonConvert.DeserializeObject<PlayData>(loadJD);

        if (playData == null)
        {
            playData = new PlayData();
        }
    }

    public void SaveData(float coin, float dia)
    {
       //MainCoin = coin;
        MainCoin = (int)coin;
        MainDia = (int)dia;

        string saveJD = JsonConvert.SerializeObject(playData); 
        PlayerPrefs.SetString(SAVE_DATA_KEY, saveJD);
    }
}

public class Print
{
    public static void Array(int[] arr)
    {
#if !UNITY_EDITOR
    return;
#endif
        string printStr = "";

        for (int i = 0; i < arr.Length; i++)
        {
            printStr += arr[i] + " ";
        }

        Debug.Log(printStr);
    }

    public static void Array(CardData[] arr)
    {
#if !UNITY_EDITOR
    return;
#endif
        string printStr = "";

        for (int i = 0; i < arr.Length; i++)
        {
            printStr += arr[i] + " ";
        }

        Debug.Log(printStr);
    }

    public static void Array(RaycastHit2D[] arr)
    {
#if !UNITY_EDITOR
    return;
#endif
        string printStr = "";

        for (int i = 0; i < arr.Length; i++)
        {
            printStr += arr[i].transform.name + " ";
        }

        Debug.Log(printStr);
    }

    public static void Array2D(int[,] arr)
    {
#if !UNITY_EDITOR
    return;
#endif
        string printStr = "";

        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < arr.GetLength(1); j++)
            {
                printStr += arr[i, j] + " ";
            }
            printStr += "\n";
        }

        Debug.Log(printStr);
    }

    public static void Array2D(bool[,] arr)
    {
#if !UNITY_EDITOR
    return;
#endif
        string printStr = "";

        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < arr.GetLength(1); j++)
            {
                printStr += arr[i, j] + " ";
            }
            printStr += "\n";
        }

        Debug.Log(printStr);
    }

    public static void ResearchArray(ResearchData[] arr)
    {
#if !UNITY_EDITOR
    return;
#endif
        string printStr = "";

        for (int i = 0; i < arr.Length; i++)
        {
            printStr += $"{i}��° ����� : " + arr[i] + "\n";
        }

        Debug.Log(printStr);
    }
}

public class Change
{
    public static string Num(double num)
    {
        string strNum = num.ToString();
        string retStr = "";

        char[] symbols = { 'K', 'M', 'B', 'T' };
        // ����
        int unit = 0;

        while (strNum.Length > 6)
        {
            unit++;
            strNum = strNum.Substring(0, strNum.Length - 3);
        }

        if (strNum.Length > 3)
        {
            int newInt = int.Parse(strNum);

            // ToString("0.00") ���� ���� �ڵ� �ݿø�
            retStr = (newInt / 1000f).ToString("0.000") + symbols[unit];

            return retStr;
        }
        else
        {
            return strNum;
        }
    }
}