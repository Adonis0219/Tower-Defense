using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MDefUpgradeButton : MUpgradeButton, ISetUpType
{
    [SerializeField]
    public DefUpgradeType myUpType;
   
    private void Update()
    {
        bt.interactable = PlayDataManager.Instance.MainCoin > upCost ? true : false;

        costText.text = "<sprite=12>" + upCost;

        switch (myUpType)
        {
            case DefUpgradeType.체력:
                // Main화면으로 시작했을 때 게임매니저에 접근할 수 없으므로 계산식으로 넣어주기
                curValueText.text = (5 * (PlayDataManager.Instance.playData.defCoinLevels[(int)myUpType]+1)).ToString();
                break;
            case DefUpgradeType.체력회복:
                curValueText.text = (.04f * PlayDataManager.Instance.playData.defCoinLevels[(int)myUpType]).ToString("F2") + "/sec";
                break;
            case DefUpgradeType.방어력:
                curValueText.text = (.5f * PlayDataManager.Instance.playData.defCoinLevels[(int)myUpType]).ToString("F2") + "%";
                break;
            case DefUpgradeType.절대방어:
                curValueText.text = (1 * PlayDataManager.Instance.playData.defCoinLevels[(int)myUpType]).ToString("F2");
                break;
            case DefUpgradeType.가시대미지:
                curValueText.text = (PlayDataManager.Instance.playData.defCoinLevels[(int)myUpType]).ToString("F2") + "%";
                break;
            case DefUpgradeType.흡혈:
                curValueText.text = (.05f * (PlayDataManager.Instance.playData.defCoinLevels[(int)myUpType])).ToString("F2") + "%";
                break;
            case DefUpgradeType.넉백확률:
                curValueText.text = (PlayDataManager.Instance.playData.defCoinLevels[(int)myUpType]).ToString("F2") + "%";
                break;
            case DefUpgradeType.넉백강도:
                curValueText.text = (.5f + .15f * (PlayDataManager.Instance.playData.defCoinLevels[(int)myUpType])).ToString("F2");
                break;
            default:
                break;
        }
    }

    public void OnUpBtClk()
    {
        PlayDataManager.Instance.MainCoin -= upCost;

        PlayDataManager.Instance.playData.defCoinLevels[(int)myUpType]++;

        upCost = Mathf.RoundToInt(upCost * upFactor);
    }

    public void SetUpType(int upType)
    {
        myUpType = (DefUpgradeType)upType;
    }
}
