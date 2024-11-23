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

// � �������� PlayData ���� �����ϵ���
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
                // PlayData�� ���� GameObject �˻� �� instance�� �ʱ�ȭ
                instance = FindObjectOfType<PlayData>();

                if (instance == null)
                {
                    GameObject temp = new GameObject();
                    temp.name = "PlayData";
                    instance = temp.AddComponent<PlayData>();

                    // �ı� �Ұ� ������Ʈ�� �����
                    // �����ڿ��� ȣ�� �Ұ�
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
            // �ı� �Ұ� ������Ʈ�� �����
            // �����ڿ��� ȣ�� �Ұ�
            DontDestroyOnLoad(this.gameObject);
        }
        // �׻� �ϳ��� �����ϰ� �ϱ� ����
        // �̹� ������� instance�� ����
        else
        {
            Destroy(this.gameObject);
            return;
        }

        // ���ο� �÷��̵����Ͱ� ������ �� ������ �ε� ���ֱ�
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
