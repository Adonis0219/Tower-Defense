using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Print
{
    public static void Array2D(int[,] arr)
    {
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
}

public class PlayData
{
    // 플레이어가 가진 코인수
    public int haveCoin = 0;

    public int haveDia = 0;

    public int dia = 0;
    public int bestWave = 0;
    public int achive = 0;
    
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
    public int[] openResearchBtCounts = new int[(int)ResearchType.Length - 1];
    public int[,] labResearchLevels = new int[(int)ResearchType.Length - 1, 9];
    public bool[,] isResearching = new bool[(int)ResearchType.Length - 1, 9];
}


// 잠금해제 조건들
public class UnlockConditions
{
   public const int BEST_WAVE = 2;
   public const float TOTAL_EARN_COIN = 10;
}

// 어떤 씬에서든 PlayData 참조 가능하도록
public class PlayDataManager : MonoBehaviour
{
    static PlayDataManager instance = null;

    public static PlayDataManager Instance
    {
        get 
        {
            //
            if (instance == null)
            {
                // PlayData를 가진 GameObject 검색 후 instance에 초기화
                instance = FindObjectOfType<PlayDataManager>();

                if (instance == null)
                {
                    GameObject temp = new GameObject();
                    temp.name = "PlayData";
                    instance = temp.AddComponent<PlayDataManager>();

                    // 파괴 불가 오브젝트로 만들기
                    // 생성자에서 호출 불가
                    DontDestroyOnLoad(temp);
                }

            }

            return instance; 
        }
    }

    public PlayData playData;

    const string SAVE_DATA_KEY = "SaveData";

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

                if (playData.bestWave >= UnlockConditions.BEST_WAVE)
                    playData.achive |= 1 << (int)Achive.UnlockWorkShop;
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

            if (playData.totalEarnCoin >= UnlockConditions.TOTAL_EARN_COIN)
                playData.achive |= 1 << (int)Achive.UnlockLabs;
        }
    }

    public int MainDia
    {
        get { return playData.haveDia; }
        set
        {
            playData.haveDia = value;
            onChangedDia?.Invoke(MainDia);
        }
    }

    private void Awake()
    {
        if( instance == null)
        {
            instance = this;
            // 파괴 불가 오브젝트로 만들기
            // 생성자에서 호출 불가
            DontDestroyOnLoad(this.gameObject);
        }
        // 항상 하나만 존재하게 하기 위해
        // 이미 만들어진 instance가 존재
        else
        {
            Destroy(this.gameObject);
            return;
        }

        // 새로운 플레이데이터가 생성될 때 데이터 로드 해주기
        LoadData();
    }


    ////// MainScene의 PlayDataManager GOBJ가 활성화 됐을 때(메인 씬 시작하자마자), 
    ////// GameScene의 GameManager가 활성화 됐을 때(게임씬으로 넘어가자마자)

    // 제이선데이터로된 정보들을 playData에 초기화 시켜준다.
    public void LoadData()
    {
        string loadJD = PlayerPrefs.GetString(SAVE_DATA_KEY, "");
        playData = JsonUtility.FromJson<PlayData>(loadJD);

        if (playData == null)
        {
            playData = new PlayData();
        }
    }

    public void SaveData(float coin, float dia)
    {
       //MainCoin = coin;
        MainCoin = 99999;
        MainDia = 10000;


        string saveJD = JsonUtility.ToJson(playData);
        PlayerPrefs.SetString(SAVE_DATA_KEY, saveJD);
    }
}
