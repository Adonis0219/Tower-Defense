using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockPanels;
    public GameObject[] unlockPanels;

    public enum Achive
    {
        UnlockWorkShop, 
        UnlockCards,
        UnlockLabs,
        Length
    }

    private void Awake()
    {
        for (int i = 0; i < (int)Achive.Length; i++)
        {
            CheckAchive((Achive)i);
        }
    }

    /// <summary>
    /// ���� üũ���ִ� �Լ�
    /// </summary>
    /// <param name="achive">üũ�� ����</param>
    void CheckAchive(Achive achive)
    {
        bool winAchive = false;

        int achiveIndex = (int)achive;

        switch (achive)
        {
            case Achive.UnlockWorkShop:
                if (PlayDataManager.Instance.BestWave >= 10)
                    winAchive = true;
                break;
            case Achive.UnlockCards:
                if (PlayDataManager.Instance.playData.totalEarnCoin >= 100)
                    winAchive = true;
                break;
            case Achive.UnlockLabs:
                //isAchive =
                break;
            default:
                break;
        }
        if (winAchive)
        {
            ChangeAchive(achiveIndex);
        }
    }

    //�������� �ٲٴ� �Լ�
    //�Ű����� ������ΰ�?
    /// <summary>
    /// �޼����� �ٲ��ִ� �Լ�
    /// </summary>
    /// <param name="index">�ٲ��� ����(index)</param>
    void ChangeAchive(int index)
    {
        // index��° ������ �޼����ִٸ� return;
        if ((PlayDataManager.Instance.playData.achive >> index) % 2 == 1)
            return;

        PlayDataManager.Instance.playData.achive += 1 << index;

        lockPanels[index].SetActive(false);
        unlockPanels[index].SetActive(true);
    }
}
