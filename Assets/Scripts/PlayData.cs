using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsData
{
    public float coin = 0;
    public int dia = 0;
    public int bestWave;
    //public int[] atkCoinLevels;
}

// 어떤 씬에서든 PlayData 참조 가능하도록
public class PlayData : MonoBehaviour
{
    static PlayData instance = null;

    public static PlayData Instance
    {
        get 
        {
            //
            if (instance == null)
            {
                // PlayData를 가진 GameObject 검색 후 instance에 초기화
                instance = FindObjectOfType<PlayData>();

                if (instance == null)
                {
                    GameObject temp = new GameObject();
                    temp.name = "PlayData";
                    instance = temp.AddComponent<PlayData>();

                    // 파괴 불가 오브젝트로 만들기
                    // 생성자에서 호출 불가
                    DontDestroyOnLoad(temp);
                }

            }

            return instance; 
        }
        //set { Instance = value; }
    }

    public GoodsData goodsData;

    const string SAVE_DATA_KEY = "SaveData";

    public int BestWave
    {
        get { return goodsData.bestWave; }
        set
        {
            if( goodsData.bestWave < value)
                goodsData.bestWave = value;
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

    public void LoadData()
    {
        string loadJD = PlayerPrefs.GetString(SAVE_DATA_KEY, "");
        goodsData = JsonUtility.FromJson<GoodsData>(loadJD);

        if (goodsData == null)
        {
            goodsData = new GoodsData();
        }
    }

    public void SaveData(float coin)
    {
        goodsData.coin = coin;
        goodsData.bestWave = BestWave;

        string saveJD = JsonUtility.ToJson(goodsData);
        PlayerPrefs.SetString(SAVE_DATA_KEY, saveJD);
    }
}
