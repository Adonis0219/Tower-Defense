using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LabManager : MonoBehaviour
{
    public static LabManager instance;

    [Header("# Lab")]
    [SerializeField]
    public Transform labListContent;
    
    [SerializeField]
    public List<LabButton> labs;

    [SerializeField]
    LabButton oriLabBt;

    [HideInInspector]
    public int clickedIndex;
    public int[] labOpenCost = new int[4] { 100, 400, 1400, 3000 };

    [Header("## Research List")]
    [SerializeField]
    public GameObject researchListPN;
    [SerializeField]
    public GameObject checkPN;

    [SerializeField]
    GameObject[] listPanels;

    [SerializeField]
    GameObject[] listPanelArrows;

    [Header("## Check List")]
    [SerializeField]
    ResearchButton oriResBt;

    [SerializeField]
    Transform mainContent;
    [SerializeField]
    Transform atkContent;
    [SerializeField]
    Transform defContent;
    [SerializeField]
    Transform utilContent;

    [Header("# Datas")]
    public List<ResearchData> mainDatas = new List<ResearchData>();
    public List<ResearchData> atkDatas = new List<ResearchData>();
    public List<ResearchData> defDatas = new List<ResearchData>();
    public List<ResearchData> utilDatas = new List<ResearchData>();

    private void Awake()
    {
        instance = this;

        LabInitSet();

        for (int i = 0; i < (int)ResearchType.Length; i++)
        {
            ChkInitSet((ResearchType)i);
        }
    }

    void LabInitSet()
    {
        // 열어줄만큼 열어주고 데이터 넣어주기
        for (int i = 0; i < PlayDataManager.Instance.playData.openLabCount; i++)
        {
            LabButton temp = Instantiate(oriLabBt, labListContent);
            labs.Add(temp);
            temp.labIndex = i;
            temp.IsOpen = true;
            // PDM에게 연구중인지를 알아내서 isEmpty 설정
            temp.IsEmpty = true;
        }

        // 실험실 5개가 모두 열려 있으면 추가 생성 X
        if (labs.Count == 5)
            return;

        LabButton unlockTemp = Instantiate(oriLabBt, labListContent);
        labs.Add(unlockTemp);
        unlockTemp.labIndex = PlayDataManager.Instance.playData.openLabCount;
        unlockTemp.IsOpen = false;
        unlockTemp.IsEmpty = true;
    }

    void ChkInitSet(ResearchType type)
    {
        List<ResearchData> datas = new List<ResearchData>();
        Transform createContent = null;

        switch (type)
        {
            case ResearchType.Main:
                datas = mainDatas;
                createContent = mainContent;
                break;
            case ResearchType.Attak:
                datas = atkDatas;
                createContent = atkContent;
                break;
            case ResearchType.Defense:
                datas = defDatas;
                createContent = defContent;
                break;
            case ResearchType.Utility:
                datas = utilDatas;
                createContent = utilContent;
                break;
            default:
                break;
        }


        for (int i = 0; i < datas.Count; i++)
        {
            ResearchButton temp = Instantiate(oriResBt, createContent);
            temp.data = datas[i];
        }
    }

    /// <summary>
    /// 연구실을 오픈할 때 
    /// </summary>
    /// <param name="openIndex">열어줄 연구실 번호</param>
    public void OpenLab(int openIndex)
    {
        // 열어줄 인덱스와 코스트의 길이가 같다면
        if (openIndex == labOpenCost.Length)
            return;

        LabButton unlockTemp = Instantiate(oriLabBt, labListContent);
        labs.Add(unlockTemp);
        unlockTemp.labIndex = PlayDataManager.Instance.playData.openLabCount;
        unlockTemp.IsOpen = false;
        unlockTemp.IsEmpty = true;
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
