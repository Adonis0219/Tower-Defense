using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
public enum UtilUpgradeType
{
    캐시보너스,
    캐시웨이브,
    코인킬보너스,
    코인웨이브,
    무료공격업,
    무료방어업,
    무료유틸업,
    이자웨이브,
    Length
}

public class UtilUpgradeButton : UpgradeButton, ISetUpType
{
    [SerializeField]
    public UtilUpgradeType myUpType;

    private void Update()
    {
        bt.interactable = GameManager.instance.CurDollar < upCost ? false : true;

        costText.text = "$" + upCost;

        switch (myUpType)
        {
            case UtilUpgradeType.캐시보너스:
                curValueText.text = "x" + GameManager.instance.DollarBonusFactor.ToString("F2");
                break;
            case UtilUpgradeType.캐시웨이브:
                curValueText.text = GameManager.instance.DollarWaveBonus.ToString();
                break;
            case UtilUpgradeType.코인킬보너스:
                curValueText.text = "x" + GameManager.instance.CoinKillBonus.ToString("F2");
                break;
            case UtilUpgradeType.코인웨이브:
                curValueText.text = GameManager.instance.CoinWaveBonus.ToString();
                break;
            case UtilUpgradeType.무료공격업:
                curValueText.text = GameManager.instance.AtkFreeUpChance.ToString("F2") + "%";
                break;
            case UtilUpgradeType.무료방어업:
                curValueText.text = GameManager.instance.DefFreeUpChance.ToString("F2") + "%";
                break;
            case UtilUpgradeType.무료유틸업:
                curValueText.text = GameManager.instance.UtilFreeUpChance.ToString("F2") + "%";
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
        GameManager.instance.CurDollar -= upCost;

        GameManager.instance.utilDollarLevels[(int)myUpType]++;

        upCost = Mathf.RoundToInt(upCost * upFactor);
    }

    public void SetUpType(int upType)
    {
        myUpType = (UtilUpgradeType)upType;
    }
}
