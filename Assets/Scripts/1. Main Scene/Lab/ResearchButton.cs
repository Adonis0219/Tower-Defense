using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            level = PlayDataManager.Instance.playData.labResearchLevels[(int)data.researchType, data.researchID];
            return level;
        }

        set
        {
            level = value;

            curResearchData.NameNLevelStr = data.researchName + "  Lv." + (Level + 1);
            curResearchData.UpInfoStr = LabManager.instance.UpInfoStrSet(data);
            curResearchData.ReqTimeStr = LabManager.instance.DisplayTime(data.reqTimes[Level]).ToString();
            curResearchData.CostStr = data.costs[Level] + "<sprite=12>";
        }
    }

    TextMeshProUGUI textNameNLevel;
    TextMeshProUGUI textUpInfo;
    TextMeshProUGUI textReqTime;
    TextMeshProUGUI textCost;

    public CurResearchData curResearchData;

    Button bt;

    private void Awake()
    {
        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        bt = GetComponent<Button>();

        textNameNLevel = texts[0];
        textUpInfo = texts[1];
        textReqTime = texts[2];
        textCost = texts[3];
    }

    private void OnEnable()
    {
        // 레벨 동기화를 위해 활성화 될 때마다 Level 초기화 시켜주기
        Level = Level;
    }

    private void Update()
    {
        // 맥스레벨이거나 연구중이거나, 비용이 부족하다면
        if (Level == data.maxLevel || PlayDataManager.Instance.playData.isResearching[(int)data.researchType, data.researchID] 
            || PlayDataManager.Instance.playData.haveCoin < data.costs[Level])            
            bt.interactable = false;       
        else 
            bt.interactable = true;

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

        // 체크 판넬 글자 바꿔주기
        texts[0].text = curResearchData.NameNLevelStr;
        texts[1].text = curResearchData.UpInfoStr;
        // 업그레이드 설명
        texts[2].text = data.researchDesc.ToString();
        // 최대 레벨
        texts[3].text = "Lv. " + Level + " / " + data.maxLevel;
        texts[4].text = curResearchData.ReqTimeStr;
        texts[5].text = curResearchData.CostStr;

        // 나의 데이터 넘겨주기
        LabManager.instance.myData = data;
    }
}
