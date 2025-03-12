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

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockPanels;
    public GameObject[] unlockPanels;


    private void Awake()
    {
        for (int i = 0; i < (int)Achive.Length; i++)
        {
            // ���� ������ �޼��ƴٸ� �ǳ� ����
            if ((PlayDataManager.Instance.playData.achive >> i) % 2 == 1)
            //if ((PlayDataManager.Instance.playData.achive & (1 << i)) == 1)
                OpenPanel(i);
        }
    }


    void OpenPanel(int index)
    {
        lockPanels[index].SetActive(false);
        unlockPanels[index].SetActive(true);
    }
}
