using System.Collections;
using System.Collections.Generic;
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

    [Header("# Upgrade")]
    [SerializeField]
    public TextMeshProUGUI nameNLevelText;
    [SerializeField]
    public TextMeshProUGUI upInfoText;
    [SerializeField]
    public TextMeshProUGUI remainTimeText;

    [SerializeField]
    Slider mySlider;

    [Header("# Unlock")]
    [SerializeField]
    TextMeshProUGUI unlockText;

    [SerializeField]
    bool isEmpty = true;
    [SerializeField]
    bool isOpen = false;

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

    public int labIndex;

    [HideInInspector]       // 소요시간 변화 X
    public float requireTime;
    [HideInInspector]       // 남은시간 -> 시간이 지날 수록 줄어듦
    public float remainTime;

    private void Start()
    {
        labNameText.text = "Lab " + (labIndex + 1);

        // 마지막 연구실이 아니고 && 잠긴 연구실이라면
        if (!isOpen)
        {
            unlockText.text = (labIndex + 1) + "번째 실험실잠금해제\n" + LabManager.instance.labOpenCost[labIndex] + "<sprite=0>";
        }
    }

    private void Update()
    {
        // 업그레이드 중이고, 남은 시간이 0보다 작다면
        if (!transform.GetChild(2).gameObject.activeSelf && remainTime < 0)
        {
            remainTime = 0;
            return;
        }

        remainTime -= Time.deltaTime;

        remainTimeText.text = LabManager.instance.DisplayTime(remainTime);

        mySlider.value = 1 - (remainTime / requireTime);
    }

    void LabBtSet()
    {
        if (isOpen)
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
        LabManager.instance.clickedIndex = labIndex;
    }


    public IEnumerator WaitReqTime(ResearchType type, int researchID)
    {
        IsEmpty = false;

        // 해당 버튼 연구중
        PlayDataManager.Instance.playData.isResearching[(int)type, researchID] = true;

        //Print.Array2D(PlayDataManager.Instance.playData.isResearching);

        yield return new WaitForSeconds(requireTime);

        ResearchComplete(type, researchID);
    }

    /// <summary>
    /// 연구가 완료 됐을 때 실행할 함수
    /// </summary>
    void ResearchComplete(ResearchType type, int researchID)
    {
        // 해당 버튼 연구중 해제
        PlayDataManager.Instance.playData.isResearching[(int)type, researchID] = false;
        // PDM에 해당 업그레이드 레벨 올려주기
        PlayDataManager.Instance.playData.labResearchLevels[(int)type, researchID]++;

        // 빈 연구실로 만들어주기
        IsEmpty = true;

        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(false);

        Print.Array2D(PlayDataManager.Instance.playData.labResearchLevels);
    }
}
