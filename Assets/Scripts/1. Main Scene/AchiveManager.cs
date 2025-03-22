using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public enum Achive
{
    UnlockWorkShop, 
    UnlockLabs,
    UnlockCards,
    Length
}

// 잠금해제 조건들
public class UnlockConditions
{
    public const int BEST_WAVE = 0;
    public const float TOTAL_EARN_COIN = 0;
    public const int BEST_WAVE_CARD = 0;
}

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockPanels;
    public GameObject[] unlockPanels;


    private void Awake()
    {
        for (int i = 0; i < (int)Achive.Length; i++)
        {
            // 만약 업적이 달성됐다면 판넬 해제
            if ((PlayDataManager.Instance.playData.achive >> i) % 2 == 1)
                OpenPanel(i);
        }
    }


    void OpenPanel(int index)
    {
        lockPanels[index].SetActive(false);
        unlockPanels[index].SetActive(true);
    }
}
