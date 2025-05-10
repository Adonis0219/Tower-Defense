using Newtonsoft.Json;
using System;
using TMPro;
using UnityEngine;
public class PlayData
{
    public bool isAdmin = false;

    // 플레이어가 가진 코인수
    public int haveCoin = 0;

    public int mainDia = 0;

    // 업적 비트마스크 활용
    public int achive = 0;
    // 이미 튜토리얼을 완료 했는가
    public int alreadyTuto = 0;

    public int bestWave = 0;
    
    // 메인화면의 판넬 해제를 위한 총 벌어들인 코인 수
    public float totalEarnCoin = 0;
    
    // 공격 업그레이드 타입의 개수만큼 배열 
    public int[] atkCoinLevels = new int[(int)AtkUpgradeType.Length];
    public int[] defCoinLevels = new int[(int)DefUpgradeType.Length];
    public int[] utilCoinLevels = new int[(int)UtilUpgradeType.Length];

    /// <summary>
    /// 총 만들어야 할 버튼의 개수
    /// </summary>
    public int[] totalCreatCounts = new int[3] { 4, 2, 0 };
    /// <summary>
    /// 열어야 할 Unlock버튼의 인덱스
    /// </summary>
    public int[] lineOpenCounts = new int[3];


    [Header("# Lab")]
    public int openLabCount = 1;
    /// <summary>
    /// 열어줄 연구 버튼의 개수 (Length는 반복문 전용이기 때문에 -1)
    /// </summary>
    public int[] openResearchBtCounts = new int[(int)ResearchType.Length];
    public int[,] labResearchLevels = new int[(int)ResearchType.Length, 9];
    public bool[,] isResearching = new bool[(int)ResearchType.Length, 9];

    // 지금 연구중인 데이터
    public ResearchData[] isResearchingData = new ResearchData[5];
    public DateTime[] startTimes = new DateTime[5];

    // 연구실 남은 시간
    public float[] labRemainTime = new float[5];
    public float[] fixedLabRemainTime = new float[5];

    public int labCompleteCount = 0;

    [Header("# Card")]
    public int curSlotCount = 1;
    public int maxSlotCount = 20;
    public int[] slotOpneCost = { 50, 100, 200, 300, 400, 500, 600, 750, 1000, 1200, 1400, 1600, 1800, 2000, 2300, 2600, 3000, 3500, 4000};

    public int[] cardLvs;
    // 현재 적용중인 카드 데이터들 (최대 슬롯은 변하지 않으므로 그냥 20)
    public int[] activedCardIDs = new int[20] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};    // -1은 장착 안 됨
}

// 어떤 씬에서든 PlayData 참조 가능하도록
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
            // 들어온 값이 현재의 최고 웨이브 보다 높다면
            if (playData.bestWave < value)
            {
                // 최고 웨이브를 들어온 값으로 설정
                playData.bestWave = value ;

                // 작업장 열어주기
                if (playData.bestWave >= UnlockConditions.BEST_WAVE_WORKSHOP)
                    playData.achive |= 1 << (int)Achive.UnlockWorkShop;

                // 카드 열어주기
                if (playData.bestWave >= UnlockConditions.BEST_WAVE_CARD)
                    playData.achive |= 1 << (int)Achive.UnlockCards;

                // 연구실 열어주기
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
        // 프레임 고정
        Application.targetFrameRate = 60;

        // 새로운 플레이데이터가 생성될 때 데이터 로드 해주기
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
    /// 슬롯에 있는 카드들 중에 CheckID 카드가 있는지
    /// </summary>
    /// <param name="checkID">확인할 카드의 ID</param>
    /// <returns>T 있다 F 없다</returns>
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
            // 시간 갭 = 현재시간 - 시작 시간
            elapsedTime = DateTime.Now - playData.startTimes[labIndex];

            playData.labRemainTime[labIndex] = playData.fixedLabRemainTime[labIndex] 
                - (float)elapsedTime.TotalSeconds;

            if (playData.labRemainTime[labIndex] < 0)
            {
                LabCompleteCount++;
            }
        }
    }

    ////// MainScene의 PlayDataManager GOBJ가 활성화 됐을 때(메인 씬 시작하자마자), 
    ////// GameScene의 GameManager가 활성화 됐을 때(게임씬으로 넘어가자마자)

    // 제이선데이터로된 정보들을 playData에 초기화 시켜준다.
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
            printStr += $"{i}번째 실험실 : " + arr[i] + "\n";
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
        // 단위
        int unit = 0;

        while (strNum.Length > 6)
        {
            unit++;
            strNum = strNum.Substring(0, strNum.Length - 3);
        }

        if (strNum.Length > 3)
        {
            int newInt = int.Parse(strNum);

            // ToString("0.00") 남는 숫자 자동 반올림
            retStr = (newInt / 1000f).ToString("0.000") + symbols[unit];

            return retStr;
        }
        else
        {
            return strNum;
        }
    }
}