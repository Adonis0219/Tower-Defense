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
    public List<LabButton> labs;        // 연구실 버튼 목록
    [SerializeField]
    LabButton oriLabBt;

    [HideInInspector]
    public int clickedIndex;    // 클릭한 인덱스 -> 연구확인 패널과 연동 시 사용
    public int[] labOpenCost = new int[4] { 100,  400, 1400, 3000 };

    [SerializeField]
    public GameObject completeCountUI;

    [Header("## Research List")]
    [SerializeField]
    public GameObject researchListPN;   // 연구사항 목록 판넬
    [SerializeField]
    public GameObject checkPN;          // 연구 확인 판넬

    [SerializeField]
    GameObject[] listPanels;

    [SerializeField]
    GameObject[] listPanelArrows;

    [Header("## Check List")]
    [SerializeField]
    ResearchButton oriResBt;

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
            // 연구칸 생성 후 lab 목록에 넣기, 인덱스 설정
            LabButton temp = Instantiate(oriLabBt, labListContent);
            labs.Add(temp);
            temp.labIndex = i;
            temp.IsOpen = true;

            // 연구실 인덱스에 지금 연구중인 데이터가 없다면 연구실 = 빔
            if (PlayDataManager.Instance.playData.isResearchingData[i] == null) temp.IsEmpty = true;
            else
            {
                // 연구칸의 연구 데이터는 PDM에서 가져오기
                temp.MyData = PlayDataManager.Instance.playData.isResearchingData[i];
                temp.IsEmpty = false;
            }
        }

        // 실험실 5개가 모두 열려 있으면 추가 생성 X
        if (labs.Count == 5)
            return;

        // 연구칸 생성 후 lab 목록에 넣기
        LabButton unlockTemp = Instantiate(oriLabBt, labListContent);
        labs.Add(unlockTemp);
        // 인덱스 및 각종 상태 초기화
        unlockTemp.labIndex = PlayDataManager.Instance.playData.openLabCount;
        unlockTemp.IsEmpty = true;
        unlockTemp.IsOpen = false;
    }

    /// <summary>
    /// 연구판넬 초기 세팅
    /// </summary>
    /// <param name="type"></param>
    void ResearchInitSet(ResearchType type)
    {
        List<ResearchData> datas = new List<ResearchData>();
        Transform createContent = listPanels[(int)type].transform;

        switch (type)
        {
            case ResearchType.Main:
                datas = mainDatas;
                break;
            case ResearchType.Attak:
                datas = atkDatas;
                break;
            case ResearchType.Defense:
                datas = defDatas;
                break;
            case ResearchType.Utility:
                datas = utilDatas;
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
    public void ListNameBtClk(int type)
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click);

        listPanels[type].SetActive(listPanels[type].activeSelf ? false : true);
        listPanelArrows[type].SetActive(listPanels[type].activeSelf ? false : true);
    }

    /// <summary>
    /// x 버튼 눌렀을 떄
    /// </summary>
    /// <param name="go">꺼줄 게임오브젝트</param>
    public void OnCloseBtClk(GameObject go)
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.NoClk);

        go.SetActive(false);
    }

    /// <summary>
    /// 체크 판넬에서 나가기 버튼 눌렀을 때
    /// </summary>
    public void OnChkExitClk()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.NoClk);
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
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.OkClk);

        // 연구 전 연구 레벨 가져오기
        researchLevel = PlayDataManager.Instance.playData.labResearchLevels[(int)myData.researchType, myData.researchID];

        // 체크 판넬이 열렸다는 것은 코인이 충분하다는 전제가 있으므로 if 사용x
        PlayDataManager.Instance.MainCoin -= SaledCost(myData.costs[researchLevel]);

        // 체크판넬 닫기
        checkPN.SetActive(false);

        // 클릭한 버튼을 할당
        LabButton clickedBt = labs[clickedIndex];

        // 지금 연구중인 데이터를 자기 자신으로 넣어줌
        PlayDataManager.Instance.playData.isResearchingData[clickedIndex] = myData;
        // 남은 시간 넘겨주기
        PlayDataManager.Instance.playData.labRemainTime[clickedIndex] = ReqTime(myData.reqTimes[researchLevel]);
        PlayDataManager.Instance.playData.fixedLabRemainTime[clickedIndex] = ReqTime(myData.reqTimes[researchLevel]);

        PlayDataManager.Instance.playData.startTimes[clickedIndex] = DateTime.Now;

        clickedBt.MyData = myData;
        // 연구 단계 넘겨주기
        clickedBt.curResearchLevel = researchLevel;

        // 클릭했을 때 시간을 연구 시작 시간으로 넘겨줌
        // 코루틴 실행이 아닌 정보전달
        clickedBt.IsEmpty = false;
    }

    public int SaledCost(int cost)
    {
        return Mathf.FloorToInt(cost * 
            (1 - .003f * PlayDataManager.Instance.playData.
            labResearchLevels[(int)ResearchType.Main, (int)MainRschType.연구실할인]));
    }

    public float ReqTime(float reqTime)
    {
        return reqTime / 
            (1 + .02f * PlayDataManager.Instance.playData.
            labResearchLevels[(int)ResearchType.Main, (int)MainRschType.연구실속도]);       
    }

    public string UpInfoStrSet(ResearchData data)
    {
        string upInfoStr = "";
        researchLevel = PlayDataManager.Instance.playData.labResearchLevels[(int)data.researchType, data.researchID];

        float cur = data.oriValue + researchLevel * data.increaseValue;
        float next = data.oriValue + (researchLevel + 1) * data.increaseValue;

        switch (data.researchType)
        {
            case ResearchType.Main:
                switch (data.researchID)
                {
                    case 0:
                        upInfoStr = "x" + cur.ToString("F1") + "  >>  " + "x" + next.ToString("F1");
                        break;
                    case 1:
                        upInfoStr = cur + "  >>  " + next;
                        break;
                    case 2: case 3: case 4: case 5: case 6:
                            upInfoStr = cur.ToString("F2") + "%  >>  " + next.ToString("F2") + "%";
                            break;
                    case 7:
                        if (researchLevel == 0) upInfoStr = "1  >>  5";
                        else if (researchLevel == 1) upInfoStr = "5  >>  10";
                        else if (researchLevel == 2) upInfoStr = "10  >>  100";
                        break;
                    default:
                        break;
                        }
                break;
            // 나중에 Research Data가 더 쌓이면 따로 설정해주기 위해
            //case ResearchType.Attak:
            //    break;
            //case ResearchType.Defense:
            //    break;
            //case ResearchType.Utility:                
            //    break;
            default:
                upInfoStr = "x" + cur.ToString("F2") + "  >>  " + "x" + next.ToString("F2");
                break;
        }

        return upInfoStr;
    }

    public string DisplayTime(float reqTime)
    {
        // 초
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
