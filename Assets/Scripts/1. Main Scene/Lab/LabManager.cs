using System;
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
            ResearchInitSet((ResearchType)i);
        }
    }

    /// <summary>
    /// 연구실 화면 초기 세팅
    /// </summary>
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
            temp.IsEmpty = PlayDataManager.Instance.playData.labRemainTimes[i] == -1 ? true : false;
            // PDM에 저장된 자신의 인덱스에 맞는 남은 시간 전달
            temp.remainTime = PlayDataManager.Instance.playData.labRemainTimes[i] 
                - (float)PlayDataManager.Instance.timeDif.TotalSeconds;
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

    /// <summary>
    /// 연구판넬 초기 세팅
    /// </summary>
    /// <param name="type"></param>
    void ResearchInitSet(ResearchType type)
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

        // datas의 타입에 따라 각 content에 datas의 개수만큼 리서치 버튼 생성
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

    /// <summary>
    /// 연구 리스트 이름을 누를 때 -> 각 리스트 판넬들을 껐다 켜줌
    /// </summary>
    /// <param name="type"></param>
    [VisibleEnum(typeof(ResearchType))]
    public void ListNameBtClk(int type)
    {
        ResearchType myResearchType = (ResearchType)type;

        listPanels[type].SetActive(listPanels[type].activeSelf ? false : true);
        listPanelArrows[type].SetActive(listPanels[type].activeSelf ? false : true);
    }

    /// <summary>
    /// x 버튼 눌렀을 떄
    /// </summary>
    /// <param name="go">꺼줄 게임오브젝트</param>
    public void OnCloseBtClk(GameObject go)
    {
        go.SetActive(false);
    }

    /// <summary>
    /// 체크 판넬에서 나가기 버튼 눌렀을 때
    /// </summary>
    public void OnChkExitClk()
    {
        // 체크 판넬 꺼주기
        checkPN.SetActive(false);
        // 연구 리스트 판넬 켜주기
        researchListPN.SetActive(true);
    }

    public ResearchData myData;
    public int researchLevel;

    /// <summary>
    /// 체크 판넬에서 연구시작 버튼을 눌렀을 때 실행할 함수
    /// </summary>
    public void OnChkResearchClk()
    {
        researchLevel = PlayDataManager.Instance.playData.labResearchLevels[(int)myData.researchType, myData.researchID];

        // 체크 판넬이 열렸다는 것은 코인이 충분하다는 전제가 있으므로 if 사용x
        PlayDataManager.Instance.playData.haveCoin -= myData.costs[researchLevel];

        // 체크판넬 닫기
        checkPN.SetActive(false);

        LabButton clickedBt = labs[clickedIndex];

        // 클릭한 연구실 연구중으로 바꿔주기
        clickedBt.transform.GetChild(1).gameObject.SetActive(false);
        clickedBt.transform.GetChild(2).gameObject.SetActive(true);

        clickedBt.nameNLevelText.text = myData.researchName + " Lv." + (researchLevel + 1);
        clickedBt.upInfoText.text = UpInfoStrSet(myData);
        clickedBt.remainTime = myData.reqTimes[researchLevel];
        clickedBt.requireTime = myData.reqTimes[researchLevel];

        clickedBt.StartCoroutine(clickedBt.WaitReqTime(myData.researchType, myData.researchID));
    }

    public string UpInfoStrSet(ResearchData data)
    {
        string upInfoStr = "";
        researchLevel = PlayDataManager.Instance.playData.labResearchLevels[(int)data.researchType, data.researchID];

        switch (data.researchType)
        {
            case ResearchType.Main:
                switch (data.researchID)
                {
                    case 0:
                        upInfoStr = "x" + (data.oriValue + researchLevel * data.increaseValue).ToString("F1") + " >> " + "x" + (data.oriValue + (researchLevel + 1) * data.increaseValue).ToString("F1");
                        break;
                    case 1:
                        upInfoStr = (data.oriValue + researchLevel * data.increaseValue) + " >> " + (data.oriValue + (researchLevel + 1) * data.increaseValue);
                        break;
                    case 3:
                        upInfoStr = (data.oriValue + researchLevel * data.increaseValue).ToString("F2") + "% >> " + (data.oriValue + (researchLevel + 1) * data.increaseValue).ToString("F2") + "%";
                        break;
                    default:
                        break;
                }
                break;
            case ResearchType.Attak:
                break;
            case ResearchType.Defense:
                break;
            case ResearchType.Utility:
                break;
            default:
                break;
        }

        return upInfoStr;
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
