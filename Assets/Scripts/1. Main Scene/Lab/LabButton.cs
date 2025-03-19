using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTime
{
    public int d;
    public int h;
    public int m;
    public int s;
}

public class LabButton : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI labNameText;

    public int labIndex;

    [Header("# Unlock")]
    [SerializeField]
    TextMeshProUGUI unlockText;

    [SerializeField]
    bool isEmpty = true;
    [SerializeField]
    bool isOpen = false;

    [Header("# Upgrade")]
    [SerializeField]
    public TextMeshProUGUI nameNLevelText;
    [SerializeField]
    public TextMeshProUGUI upInfoText;
    [SerializeField]
    public TextMeshProUGUI remainTimeText;

    [SerializeField]
    Slider mySlider;

    /// <summary>
    /// 경과된 시간
    /// </summary>
    public TimeSpan elapsedTime;

    [HideInInspector]       // 남은시간 -> 시간이 지날 수록 줄어듦
    public float remainTime;

    ResearchData myData;

    public int curResearchLevel;

    /// <summary>
    /// 비어있는가? -> false = 연구중
    /// </summary>
    public bool IsEmpty
    {
        get { return isEmpty; }
        set
        {
            isEmpty = value;
            LabBtSet();
        }
    }

    public bool IsOpen
    {
        get { return isOpen; }
        set
        {
            isOpen = value;
            LabBtSet();
        }
    }


    public ResearchData MyData
    {
        get { return myData; }

        set
        {
            myData = value;

            // myData가 비어있지 않다면 -> 연구 중이라면
            // 게임 재접속 시 흐른 시간만큼 연구 시간 줄여주기
            if (myData != null)
            {
                SetUpgradeBt();
                // 연구 시작, 연구 재개
                Researching();
            }
        }
    }

    void SetUpgradeBt()
    {
        // 클릭한 연구실 바꿔주기
        // 빈 연구실 꺼주기
        transform.GetChild(1).gameObject.SetActive(false);
        // 연구중 켜주기
        transform.GetChild(2).gameObject.SetActive(true);

        nameNLevelText.text = myData.researchName + "Lv." + (curResearchLevel + 1);
        upInfoText.text = LabManager.instance.UpInfoStrSet(myData);
        remainTime = myData.reqTimes[curResearchLevel];
    }

    private void Awake()
    {
        MyData = PlayDataManager.Instance.playData.isResearchingData[labIndex];
    }

    private void Start()
    {
        labNameText.text = "Lab " + (labIndex + 1);

        if (IsEmpty)
        {
            this.enabled = false;

            // 잠긴 연구실이면
            if (!IsOpen)
            {
                unlockText.text = (labIndex + 1) + "번째 실험실잠금해제\n" + LabManager.instance.labOpenCost[labIndex] + "<sprite=0>";
            }
        }
    }


    private void Update()
    {
        // 연구중이 아니라면 얼리리턴
        // 남은 시간이 0보다 작다면 리턴 || remainTime < 0
        if (IsEmpty)
        {
            return;
        }

        // 시간 갭 = 현재시간 - 시작 시간
        ////////////////elapsedTime = DateTime.Now - PlayDataManager.Instance.playData.startTimes[labIndex];

        //////////////remainTime = myData.reqTimes[curResearchLevel] - (float)elapsedTime.TotalSeconds;

        remainTimeText.text = LabManager.instance.DisplayTime(PlayDataManager.Instance.playData.labRemainTime[labIndex]);
        ////////////remainTimeText.text = LabManager.instance.DisplayTime(remainTime);

        Debug.Log(labIndex + "번째 연구실 남은 시간 : " + LabManager.instance.DisplayTime(remainTime));

        mySlider.value = 1 - (PlayDataManager.Instance.playData.labRemainTime[labIndex] / myData.reqTimes[curResearchLevel]);
        /////////////mySlider.value = 1 - (remainTime / myData.reqTimes[curResearchLevel]);

        ///////////////if (remainTime < 0)
        if (PlayDataManager.Instance.playData.labRemainTime[labIndex] < 0)
        {
            // 연구 완료 판넬 띄워주기
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(4).gameObject.SetActive(true);

            // 연구 완료 개수 올려주기
            //////////////LabManager.instance.LabCompleteCount++;
        }
    }

    /// <summary>
    /// 랩버튼 UI 설정
    /// </summary>
    void LabBtSet()
    {
        if (IsOpen)
        {
            // 빈 연구실
            transform.GetChild(1).gameObject.SetActive(IsEmpty ? true : false);
            // 업그레이드중
            transform.GetChild(2).gameObject.SetActive(IsEmpty ? false : true);
            // 잠김
            transform.GetChild(3).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 빈 연구실을 클릭 했을 때 실행할 함수
    /// </summary>
    public void EmptyLabClick()
    {
        // 비어있지 않다면 -> 연구중이라면
        if (!isEmpty)
            return;

        // 잠긴 연구실이면
        if (!isOpen)
        {
            // 비용 감소
            PlayDataManager.Instance.MainDia -= LabManager.instance.labOpenCost[PlayDataManager.Instance.playData.openLabCount];
            
            PlayDataManager.Instance.playData.openLabCount++;

            // 열어주고
            LabManager.instance.OpenLab(PlayDataManager.Instance.playData.openLabCount);

            // 열림으로 바꾸기
            IsOpen = true;
            return;
        }

        LabManager.instance.researchListPN.SetActive(true);
        // 랩 매니저에게 클릭한 인덱스 번호 넘겨주기
        LabManager.instance.clickedIndex = labIndex;
    }

    /// <summary>
    ///  스킵 버튼과 연구완료 버튼 누를 시 실행할 함수
    /// </summary>
    public void RschCompleteBtClk()
    {
        ResearchComplete();
    }

    void Researching()
    {
        // 해당 버튼 연구중
        PlayDataManager.Instance.playData.isResearching[(int)MyData.researchType, MyData.researchID] = true;

        this.enabled = true;        
    }

    /// <summary>
    /// 연구가 완료 됐을 때 실행할 함수
    /// </summary>
    void ResearchComplete()
    {
        // 연구완료 버튼 꺼주기
        transform.GetChild(4).gameObject.SetActive(false);
        // 연구완료 개수 줄여주기
        /////////////////LabManager.instance.LabCompleteCount--;
        PlayDataManager.Instance.LabCompleteCount--;


        // PDM에 해당 업그레이드 레벨 올려주기
        PlayDataManager.Instance.playData.labResearchLevels[(int)MyData.researchType, MyData.researchID]++;
        // 현재 연구중 아님
        PlayDataManager.Instance.playData.isResearching[(int)MyData.researchType, MyData.researchID] = false;
        // 연구중인 연구데이터 지워주기
        PlayDataManager.Instance.playData.isResearchingData[labIndex] = null;

        // 빈 연구실로 만들어주기
        IsEmpty = true;

        ///////////////// 이거 왜 한 거지..?
        this.enabled = false;

        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(false);

        Print.Array2D(PlayDataManager.Instance.playData.labResearchLevels);
        Print.Array2D(PlayDataManager.Instance.playData.isResearching);

        Debug.Log("연구 끝남");
    }
}
