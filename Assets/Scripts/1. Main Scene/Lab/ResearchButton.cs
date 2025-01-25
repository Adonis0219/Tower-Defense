using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

class DisplayTime
{
    public int d;
    public int h;
    public int m;
    public int s;
}

public struct CurResearchData
{
    public string NameNLevelStr;
    public string UpInfoStr;
    public string ReqTimeStr;
    public string CostStr;
}

public class ResearchButton : MonoBehaviour
{
    public ResearchData data;

    int level;
    public int Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;

            curResearchData.NameNLevelStr = data.researchName + "  Lv." + (Level + 1);
            curResearchData.UpInfoStr = UpInfoStrSet();
            curResearchData.ReqTimeStr = LabManager.instance.DisplayTime(data.reqTimes[Level]).ToString();
            curResearchData.CostStr = data.costs[Level] + "<sprite=12>";
        }
    }

    TextMeshProUGUI textNameNLevel;
    TextMeshProUGUI textUpInfo;
    TextMeshProUGUI textReqTime;
    TextMeshProUGUI textCost;

    public CurResearchData curResearchData;

    string upInfoStr = "";

    private void Awake()
    {
        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();

        textNameNLevel = texts[0];
        textUpInfo = texts[1];
        textReqTime = texts[2];
        textCost = texts[3];

        Level = Level;
    }

    private void Update()
    {
        // �ƽ������� �����ϰų�, ����� �����ϸ� ��ư ��Ȱ��ȭ


        textNameNLevel.text = curResearchData.NameNLevelStr;
        textUpInfo.text = curResearchData.UpInfoStr;
        textReqTime.text = curResearchData.ReqTimeStr;
        textCost.text = curResearchData.CostStr;
    }

    public void OnBtClk()
    {
        LabManager.instance.researchListPN.SetActive(false);
        LabManager.instance.checkPN.SetActive(true);

        TextMeshProUGUI[] texts = LabManager.instance.checkPN.transform.GetChild(0).GetComponentsInChildren<TextMeshProUGUI>();

        texts[0].text = curResearchData.NameNLevelStr;
        texts[1].text = curResearchData.UpInfoStr;
        // ���׷��̵� ����
        texts[2].text = data.researchDesc.ToString();
        // �ִ� ����
        texts[3].text = "Lv. " + Level + " / " + data.maxLevel;
        texts[4].text = curResearchData.ReqTimeStr;
        texts[5].text = curResearchData.CostStr;
    }

    string UpInfoStrSet()
    {
        switch (data.researchType)
        {
            case ResearchType.Main:
                switch (data.researchID)
                {
                    case 0:
                        upInfoStr = "x" + (data.oriValue + Level * data.increaseValue).ToString("F1") + " >> " + "x" + (data.oriValue + (Level + 1) * data.increaseValue).ToString("F1");
                        break;
                    case 1:
                        upInfoStr = (data.oriValue + Level * data.increaseValue) + " >> " + (data.oriValue + (Level + 1) * data.increaseValue);
                        break;
                    case 3:
                        upInfoStr =(data.oriValue + Level * data.increaseValue).ToString("F2") + "% >> " + (data.oriValue + (Level + 1) * data.increaseValue).ToString("F2") + "%";
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

    /// <summary>
    /// üũ�ǳڿ��� ������ ������ ��
    /// </summary>
    public void OnChkExitClk()
    {
        LabManager.instance.researchListPN.SetActive(true);
        LabManager.instance.checkPN.SetActive(false);
    }

    /// <summary>
    /// üũ�ǳڿ��� �����ϱ� ������ ��
    /// </summary>
    public void OnChkResearchClk()
    {
        // üũ �ǳ��� ���ȴٴ� ���� ������ ����ϴٴ� ������ �����Ƿ�
        PlayDataManager.Instance.playData.haveCoin -= data.costs[Level];        

        // üũâ �ݱ�
        LabManager.instance.checkPN.SetActive(false);

        LabButton clickedBt = LabManager.instance.labs[LabManager.instance.clickedIndex];

        // Ŭ���� ������ ĭ ���������� �ٲ��ֱ�
        clickedBt.transform.GetChild(1).gameObject.SetActive(false);
        clickedBt.transform.GetChild(2).gameObject.SetActive(true);

        // �������� ������ ��� �Ѱ��ֱ�
        clickedBt.nameNLevelText.text = curResearchData.NameNLevelStr;
        clickedBt.upInfoText.text = curResearchData.UpInfoStr;
        clickedBt.requireTime = data.reqTimes[Level];
        clickedBt.remainTime = data.reqTimes[Level];

        clickedBt.StartCoroutine(clickedBt.WaitReqTime(data.researchType, data.researchID));
    }
}
