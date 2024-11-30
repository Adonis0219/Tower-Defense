using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayData
{
    // �÷��̾ ���� ���μ�
    public int haveCoin = 0;

    public int dia = 0;
    public int bestWave = 0;
    public int achive = 0;
    // ����ȭ���� �ǳ� ������ ���� �� ������� ���� ��
    public float totalEarnCoin = 0;
    // ���� ���׷��̵� Ÿ���� ������ŭ �迭 
    public int[] atkCoinLevels = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
}


// � �������� PlayData ���� �����ϵ���
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
                // PlayData�� ���� GameObject �˻� �� instance�� �ʱ�ȭ
                instance = FindObjectOfType<PlayDataManager>();

                if (instance == null)
                {
                    GameObject temp = new GameObject();
                    temp.name = "PlayData";
                    instance = temp.AddComponent<PlayDataManager>();

                    // �ı� �Ұ� ������Ʈ�� �����
                    // �����ڿ��� ȣ�� �Ұ�
                    DontDestroyOnLoad(temp);
                }

            }

            return instance; 
        }
        //set { Instance = value; }
    }

    public PlayData playData;

    const string SAVE_DATA_KEY = "SaveData";

    public int BestWave
    {
        get { return playData.bestWave; }
        set
        {
            // ���� ���� ������ �ְ� ���̺� ���� ���ٸ�
            if( playData.bestWave < value)
                // �ְ� ���̺긦 ���� ������ ����
                playData.bestWave = value ;
        }
    }

    public int MainCoin
    {
        get { return playData.haveCoin; }
        set 
        { 
            playData.haveCoin = value;
            onChangedCoin?.Invoke(MainCoin);
        }
    }

    public Action<int> onChangedCoin;

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
        playData = JsonUtility.FromJson<PlayData>(loadJD);

        if (playData == null)
        {
            playData = new PlayData();
        }
    }

    public void SaveData(int coin)
    {
        MainCoin = coin;

        string saveJD = JsonUtility.ToJson(playData);
        PlayerPrefs.SetString(SAVE_DATA_KEY, saveJD);
    }
}
