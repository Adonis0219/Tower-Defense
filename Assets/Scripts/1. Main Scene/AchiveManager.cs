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
    /// 업적 체크해주는 함수
    /// </summary>
    /// <param name="achive">체크할 업적</param>
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

    //업적값을 바꾸는 함수
    //매개변수 어떤업적인가?
    /// <summary>
    /// 달성으로 바꿔주는 함수
    /// </summary>
    /// <param name="index">바꿔줄 업적(index)</param>
    void ChangeAchive(int index)
    {
        // index번째 업적이 달성돼있다면 return;
        if ((PlayDataManager.Instance.playData.achive >> index) % 2 == 1)
            return;

        PlayDataManager.Instance.playData.achive += 1 << index;

        lockPanels[index].SetActive(false);
        unlockPanels[index].SetActive(true);
    }
}
