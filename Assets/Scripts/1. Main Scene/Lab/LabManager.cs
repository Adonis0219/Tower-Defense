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
    public List<LabButton> labs;        // ������ ��ư ���
    [SerializeField]
    LabButton oriLabBt;

    [HideInInspector]
    public int clickedIndex;    // Ŭ���� �ε��� -> ����Ȯ�� �гΰ� ���� �� ���
    public int[] labOpenCost = new int[4] { 100,  400, 1400, 3000 };

    [SerializeField]
    public GameObject completeCountUI;

    [Header("## Research List")]
    [SerializeField]
    public GameObject researchListPN;   // �������� ��� �ǳ�
    [SerializeField]
    public GameObject checkPN;          // ���� Ȯ�� �ǳ�

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
    /// ������ ȭ�� �ʱ� ����
    /// </summary>
    void LabInitSet()
    {
        // �����ٸ�ŭ �����ְ� ������ �־��ֱ�
        for (int i = 0; i < PlayDataManager.Instance.playData.openLabCount; i++)
        {
            LabButton temp = Instantiate(oriLabBt, labListContent);
            labs.Add(temp);
            temp.labIndex = i;
            temp.IsOpen = true;

            if (PlayDataManager.Instance.playData.isResearchingData[i] == null)
            {
                temp.IsEmpty = true;
            }
            else
            {
                temp.MyData = PlayDataManager.Instance.playData.isResearchingData[i];
                temp.IsEmpty = false;
            }
        }

        // ����� 5���� ��� ���� ������ �߰� ���� X
        if (labs.Count == 5)
            return;

        LabButton unlockTemp = Instantiate(oriLabBt, labListContent);
        labs.Add(unlockTemp);
        unlockTemp.labIndex = PlayDataManager.Instance.playData.openLabCount;
        unlockTemp.IsEmpty = true;
        unlockTemp.IsOpen = false;
    }

    /// <summary>
    /// �����ǳ� �ʱ� ����
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

        // datas�� Ÿ�Կ� ���� �� content�� datas�� ������ŭ ����ġ ��ư ����
        for (int i = 0; i < datas.Count; i++)
        {
            ResearchButton temp = Instantiate(oriResBt, createContent);
            temp.data = datas[i];
        }
    }

    /// <summary>
    /// �������� ������ �� 
    /// </summary>
    /// <param name="openIndex">������ ������ ��ȣ</param>
    public void OpenLab(int openIndex)
    {
        // ������ �ε����� �ڽ�Ʈ�� ���̰� ���ٸ�
        if (openIndex == labOpenCost.Length)
            return;

        LabButton unlockTemp = Instantiate(oriLabBt, labListContent);
        labs.Add(unlockTemp);
        unlockTemp.labIndex = PlayDataManager.Instance.playData.openLabCount;
        unlockTemp.IsOpen = false;
        unlockTemp.IsEmpty = true;
    }

    /// <summary>
    /// ���� ����Ʈ �̸��� ���� �� -> �� ����Ʈ �ǳڵ��� ���� ����
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
    /// x ��ư ������ ��
    /// </summary>
    /// <param name="go">���� ���ӿ�����Ʈ</param>
    public void OnCloseBtClk(GameObject go)
    {
        go.SetActive(false);
    }

    /// <summary>
    /// üũ �ǳڿ��� ������ ��ư ������ ��
    /// </summary>
    public void OnChkExitClk()
    {
        // üũ �ǳ� ���ֱ�
        checkPN.SetActive(false);
        // ���� ����Ʈ �ǳ� ���ֱ�
        researchListPN.SetActive(true);
    }

    public ResearchData myData;
    public int researchLevel;

    /// <summary>
    /// üũ �ǳڿ��� �������� ��ư�� ������ �� ������ �Լ�
    /// </summary>
    public void OnChkResearchClk()
    {
        // ���� �� ���� ���� ��������
        researchLevel = PlayDataManager.Instance.playData.labResearchLevels[(int)myData.researchType, myData.researchID];

        // üũ �ǳ��� ���ȴٴ� ���� ������ ����ϴٴ� ������ �����Ƿ� if ���x
        PlayDataManager.Instance.MainCoin -= SaledCost(myData.costs[researchLevel]);

        // üũ�ǳ� �ݱ�
        checkPN.SetActive(false);

        // Ŭ���� ��ư�� �Ҵ�
        LabButton clickedBt = labs[clickedIndex];

        // ���� �������� �����͸� �ڱ� �ڽ����� �־���
        PlayDataManager.Instance.playData.isResearchingData[clickedIndex] = myData;
        // ���� �ð� �Ѱ��ֱ�
        PlayDataManager.Instance.playData.labRemainTime[clickedIndex] = myData.reqTimes[researchLevel];
        PlayDataManager.Instance.playData.fixedLabRemainTime[clickedIndex] = myData.reqTimes[researchLevel];

        PlayDataManager.Instance.playData.startTimes[clickedIndex] = DateTime.Now;

        clickedBt.MyData = myData;
        // ���� �ܰ� �Ѱ��ֱ�
        clickedBt.curResearchLevel = researchLevel;

        // Ŭ������ �� �ð��� ���� ���� �ð����� �Ѱ���

        // �ڷ�ƾ ������ �ƴ� ��������
        clickedBt.IsEmpty = false;
    }

    public int SaledCost(int cost)
    {
        return Mathf.FloorToInt(cost * (1 - .003f * PlayDataManager.Instance.playData.labResearchLevels[(int)ResearchType.Main, (int)MainRschType.����������])); ;
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

        // �ϼ��� �������� �ʴ´ٸ�
        if (dispTime.d == 0)
        {
            // �� �� �� ǥ��
            dispTimeStr = dispTime.h.ToString("D2") + "h "
            + dispTime.m.ToString("D2") + "m " + dispTime.s.ToString("D2") + "s";

            // �ϼ��� ����, �ð��� ���ٸ�
            if (dispTime.h == 0)
            {
                // �� �� ǥ��
                dispTimeStr = dispTime.m.ToString("D2") + "m " + dispTime.s.ToString("D2") + "s";
            }
        }
        // �ϼ��� �����Ѵٸ�
        else
        {
            // �� �� �б����� ǥ��
            dispTimeStr = dispTime.d.ToString() + "d " + dispTime.h.ToString("D2") + "h "
                + dispTime.m.ToString("D2") + "m";
        }

        return dispTimeStr;
    }
}
