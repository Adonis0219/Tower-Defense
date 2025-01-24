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
        // 맥스레벨에 도달하거나, 비용이 부족하면 버튼 비활성화

        textNameNLevel.text = "게임 속도  Lv." + (level + 1);
        textUpInfo.text = "x" + data.values[level].ToString("F1") + " >> x" + data.values[level + 1].ToString("F1");
        textReqTime.text = LabManager.instance.DisplayTime(data.reqTimes[level]).ToString();
        textCost.text = data.costs[level] + "<sprite=12>";
    }

    public void OnBtClk()
    {
        LabManager.instance.researchListPN.SetActive(false);
        LabManager.instance.checkPN.SetActive(true);

        TextMeshProUGUI[] texts = LabManager.instance.checkPN.transform.GetChild(0).GetComponentsInChildren<TextMeshProUGUI>();

        texts[0].text = "게임 속도  Lv." + (level + 1);
        texts[1].text = "x" + data.values[level].ToString("F1") + " >> x" + data.values[level + 1].ToString("F1");
        texts[2].text = data.researchDesc.ToString();
        texts[3].text = "Lv. " + level + " / " + data.maxLevel;
        texts[4].text = LabManager.instance.DisplayTime(data.reqTimes[level]).ToString();
        texts[5].text = textCost.text = data.costs[level] + "<sprite=12>";
    }

    /// <summary>
    /// 체크판넬에서 나가기 눌렀을 때
    /// </summary>
    public void OnChkExitClk()
    {
        LabManager.instance.researchListPN.SetActive(true);
        LabManager.instance.checkPN.SetActive(false);
    }

    /// <summary>
    /// 체크판넬에서 연구하기 눌렀을 때
    /// </summary>
    public void OnChkResearchClk()
    {
        // 체크 판넬이 열렸다는 것은 코인이 충분하다는 전제가 있으므로
        PlayDataManager.Instance.playData.haveCoin -= data.costs[level];        

        // 체크창 닫기
        LabManager.instance.checkPN.SetActive(false);

        LabButton clickedBt = LabManager.instance.labs[LabManager.instance.clickedIndex];

        // 클릭한 연구실 칸 연구중으로 바꿔주기
        clickedBt.transform.GetChild(1).gameObject.SetActive(false);
        clickedBt.transform.GetChild(2).gameObject.SetActive(true);

        // 연구중인 데이터 모두 넘겨주기
        clickedBt.nameNLevelText.text = "게임 속도  Lv." + (level + 1);
        clickedBt.upInfoText.text = "x" + data.values[level].ToString("F1") + " >> x" + data.values[level + 1].ToString("F1");
        clickedBt.requireTime = data.reqTimes[level];
        clickedBt.remainTime = data.reqTimes[level];
    }
}
