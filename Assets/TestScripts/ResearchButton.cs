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

public class ResearchButton : MonoBehaviour
{
    public ResearchData data;
    public int level;

    TextMeshProUGUI textNameNLevel;
    TextMeshProUGUI textUpInfo;
    TextMeshProUGUI textReqTime;
    TextMeshProUGUI textCost;

    private void Awake()
    {
        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();

        textNameNLevel = texts[0];
        textUpInfo = texts[1];
        textReqTime = texts[2];
        textCost = texts[3];
    }

    private void Update()
    {
        // �ƽ������� �����ϰų�, ����� �����ϸ� ��ư ��Ȱ��ȭ

        textNameNLevel.text = "���� �ӵ�  Lv." + (level + 1);
        textUpInfo.text = "x" + data.values[level].ToString("F1") + " >> x" + data.values[level + 1].ToString("F1");
        textReqTime.text = LabManager.instance.DisplayTime(data.reqTimes[level]).ToString();
        textCost.text = data.costs[level] + "<sprite=12>";
    }

    public void OnBtClk()
    {
        LabManager.instance.researchListPN.SetActive(false);
        LabManager.instance.checkPN.SetActive(true);

        TextMeshProUGUI[] texts = LabManager.instance.checkPN.transform.GetChild(0).GetComponentsInChildren<TextMeshProUGUI>();

        texts[0].text = "���� �ӵ�  Lv." + (level + 1);
        texts[1].text = "x" + data.values[level].ToString("F1") + " >> x" + data.values[level + 1].ToString("F1");
        texts[2].text = data.researchDesc.ToString();
        texts[3].text = "Lv. " + level + " / " + data.maxLevel;
        texts[4].text = LabManager.instance.DisplayTime(data.reqTimes[level]).ToString();
        texts[5].text = textCost.text = data.costs[level] + "<sprite=12>";
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
        PlayDataManager.Instance.playData.haveCoin -= data.costs[level];        

        // üũâ �ݱ�
        LabManager.instance.checkPN.SetActive(false);

        LabButton clickedBt = LabManager.instance.labs[LabManager.instance.clickedIndex];

        // Ŭ���� ������ ĭ ���������� �ٲ��ֱ�
        clickedBt.transform.GetChild(1).gameObject.SetActive(false);
        clickedBt.transform.GetChild(2).gameObject.SetActive(true);

        // �������� ������ ��� �Ѱ��ֱ�
        clickedBt.nameNLevelText.text = "���� �ӵ�  Lv." + (level + 1);
        clickedBt.upInfoText.text = "x" + data.values[level].ToString("F1") + " >> x" + data.values[level + 1].ToString("F1");
        clickedBt.requireTime = data.reqTimes[level];
        clickedBt.remainTime = data.reqTimes[level];
    }
}
