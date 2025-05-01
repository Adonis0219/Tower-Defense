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
            // ����ĭ ���� �� lab ��Ͽ� �ֱ�, �ε��� ����
            LabButton temp = Instantiate(oriLabBt, labListContent);
            labs.Add(temp);
            temp.labIndex = i;
            temp.IsOpen = true;

            // ������ �ε����� ���� �������� �����Ͱ� ���ٸ� ������ = ��
            if (PlayDataManager.Instance.playData.isResearchingData[i] == null) temp.IsEmpty = true;
            else
            {
                // ����ĭ�� ���� �����ʹ� PDM���� ��������
                temp.MyData = PlayDataManager.Instance.playData.isResearchingData[i];
                temp.IsEmpty = false;
            }
        }

        // ����� 5���� ��� ���� ������ �߰� ���� X
        if (labs.Count == 5)
            return;

        // ����ĭ ���� �� lab ��Ͽ� �ֱ�
        LabButton unlockTemp = Instantiate(oriLabBt, labListContent);
        labs.Add(unlockTemp);
        // �ε��� �� ���� ���� �ʱ�ȭ
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
    public void ListNameBtClk(int type)
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Click);

        listPanels[type].SetActive(listPanels[type].activeSelf ? false : true);
        listPanelArrows[type].SetActive(listPanels[type].activeSelf ? false : true);
    }

    /// <summary>
    /// x ��ư ������ ��
    /// </summary>
    /// <param name="go">���� ���ӿ�����Ʈ</param>
    public void OnCloseBtClk(GameObject go)
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.NoClk);

        go.SetActive(false);
    }

    /// <summary>
    /// üũ �ǳڿ��� ������ ��ư ������ ��
    /// </summary>
    public void OnChkExitClk()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.NoClk);
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
        AudioManager.instance.PlaySfx(AudioManager.Sfx.OkClk);

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
        PlayDataManager.Instance.playData.labRemainTime[clickedIndex] = ReqTime(myData.reqTimes[researchLevel]);
        PlayDataManager.Instance.playData.fixedLabRemainTime[clickedIndex] = ReqTime(myData.reqTimes[researchLevel]);

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
        return Mathf.FloorToInt(cost * 
            (1 - .003f * PlayDataManager.Instance.playData.
            labResearchLevels[(int)ResearchType.Main, (int)MainRschType.����������]));
    }

    public float ReqTime(float reqTime)
    {
        return reqTime / 
            (1 + .02f * PlayDataManager.Instance.playData.
            labResearchLevels[(int)ResearchType.Main, (int)MainRschType.�����Ǽӵ�]);       
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
            // ���߿� Research Data�� �� ���̸� ���� �������ֱ� ����
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
        // ��
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
