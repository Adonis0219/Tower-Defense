using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockPanels;
    public GameObject[] unlockPanels;

    public enum Achive
    {
        UnlockWorkShop, UnlockCards, UnlockLabs
    }

    Achive[] achives;

    private void Awake()
    {
        // Enum.GetValue -> �־��� �������� ��� �����͸� �������� �Լ�
        achives = (Achive[])Enum.GetValues(typeof(Achive));

        if (!PlayerPrefs.HasKey("MyData")) Init();
    }

    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);

        foreach (Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
        // �ر��� �� �Ǿ����Ƿ� 0���� �ʱ�ȭ
        //PlayerPrefs.SetInt("UnlockWorkShop", 0);
        //PlayerPrefs.SetInt("UnlockCards", 0);
        //PlayerPrefs.SetInt("UnlockLabs", 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        UnlockPanel();
    }

    void UnlockPanel()
    {
        for (int i = 0; i < lockPanels.Length; i++)
        {
            string achiveName = achives[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;

            lockPanels[i].SetActive(!isUnlock);
            unlockPanels[i].SetActive(isUnlock);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Achive achive in achives)
        {
            CheckAchive(achive);
        }
    }

    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        switch (achive)
        {
            case Achive.UnlockWorkShop:
                // 10���̺� �̻� �޼� �� ���
                isAchive = PlayData.Instance.BestWave > 10;
                break;
            case Achive.UnlockCards:
                // ȹ�� ������ 100 �̻� �� ���
                //isAchive = GameManager.instance.earnCoin >= 100;
                break;
            case Achive.UnlockLabs:
                //isAchive =
                break;
            default:
                break;
        }
    }
}
