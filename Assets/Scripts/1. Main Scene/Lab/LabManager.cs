using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LabManager : MonoBehaviour
{
    public static LabManager instance;

    [Header("# Lab")]
    [SerializeField]
    public List<LabButton> labs;

    public int clickedIndex;

    [Header("# Research List")]
    [SerializeField]
    public GameObject researchListPN;
    [SerializeField]
    public GameObject checkPN;

    [SerializeField]
    GameObject[] listPanels;

    [SerializeField]
    GameObject[] listPanelArrows;

    private void Awake()
    {
        instance = this;
    }

    [VisibleEnum(typeof(ResearchType))]
    public void ListNameBtClk(int type)
    {
        ResearchType myResearchType = (ResearchType)type;

        listPanels[type].SetActive(listPanels[type].activeSelf ? false : true);
        listPanelArrows[type].SetActive(listPanels[type].activeSelf ? false : true);

    }

    public void OnCloseBtClk(GameObject go)
    {
        go.SetActive(false);
    }

    public string DisplayTime(float reqTime)
    {
        DisplayTime dispTime = new DisplayTime();
        string dispTimeStr = "";

        dispTime.d = Mathf.FloorToInt(reqTime / 86400);
        dispTime.h = Mathf.FloorToInt((reqTime - dispTime.d * 86400) / 3600);
        dispTime.m = Mathf.FloorToInt(reqTime / 60 % 60);
        dispTime.s = (int)(reqTime % 60);

        // 일수가 존재하지 않는다면
        if (dispTime.d == 0)
        {
            // 시 분 초 표기
            dispTimeStr = dispTime.h.ToString("D2") + "h "
            + dispTime.m.ToString("D2") + "m " + dispTime.s.ToString("D2") + "s";

            // 일수가 없고, 시간도 없다면
            if (dispTime.h == 0)
            {
                // 분 초 표기
                dispTimeStr = dispTime.m.ToString("D2") + "m " + dispTime.s.ToString("D2") + "s";
            }
        }
        // 일수가 존재한다면
        else
        {
            // 일 시 분까지만 표기
            dispTimeStr = dispTime.d.ToString() + "d " + dispTime.h.ToString("D2") + "h "
                + dispTime.m.ToString("D2") + "m";
        }

        return dispTimeStr;
    }
}
