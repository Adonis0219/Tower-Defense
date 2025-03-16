using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MUtilUpgradeButton : MUpgradeButton, ISetUpType
{
    [SerializeField]
    public UtilUpgradeType myUpType;

    private void Update()
    {
        bt.interactable = PlayDataManager.Instance.MainCoin > upCost ? true : false;

        costText.text = "<sprite=12>" + Mathf.FloorToInt(upCost * Sale(MainRschType.유틸할인));

        switch (myUpType)
        {
            case UtilUpgradeType.캐시보너스:
                curValueText.text = "x" + (1 + .01f * PlayDataManager.Instance.playData.utilCoinLevels[(int)myUpType]).ToString("F2");
                break;
            case UtilUpgradeType.캐시웨이브:
                curValueText.text = (4 * PlayDataManager.Instance.playData.utilCoinLevels[(int)myUpType]).ToString();
                break;
            case UtilUpgradeType.코인킬보너스:
                curValueText.text = "x" + (1 + .01f * PlayDataManager.Instance.playData.utilCoinLevels[(int)myUpType]).ToString("F2");
                break;
            case UtilUpgradeType.코인웨이브:
                curValueText.text = PlayDataManager.Instance.playData.utilCoinLevels[(int)myUpType].ToString();
                break;
            case UtilUpgradeType.무료공격업:
                curValueText.text = (.5f * PlayDataManager.Instance.playData.utilCoinLevels[(int)myUpType]).ToString("F2") + "%";
                break;
            case UtilUpgradeType.무료방어업:
                curValueText.text = (.5f * PlayDataManager.Instance.playData.utilCoinLevels[(int)myUpType]).ToString("F2") + "%";
                break;
            case UtilUpgradeType.무료유틸업:
                curValueText.text = (.5f * PlayDataManager.Instance.playData.utilCoinLevels[(int)myUpType]).ToString("F2") + "%";
                break;
            case UtilUpgradeType.이자웨이브:
                break;
            case UtilUpgradeType.Length:
                break;
            default:
                break;
        }
    }

    public void OnUpBtClk()
    {
        PlayDataManager.Instance.MainCoin -= Mathf.FloorToInt(upCost * Sale(MainRschType.유틸할인));

        PlayDataManager.Instance.playData.utilCoinLevels[(int)myUpType]++;

        upCost = Mathf.RoundToInt(upCost * upFactor);
    }

    public void SetUpType(int upType)
    {
        myUpType = (UtilUpgradeType)upType;
    }
}
