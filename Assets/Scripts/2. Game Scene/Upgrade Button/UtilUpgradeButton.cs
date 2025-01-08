using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
public enum UtilUpgradeType
{
    달러웨이브,
    달러보너스,
    Length
}

public class UtilUpgradeButton : UpgradeButton, ISetUpType
{
    [SerializeField]
    public UtilUpgradeType myUpType;

    private void Update()
    {
        costText.text = "$" + upCost;

        switch (myUpType)
        {
            case UtilUpgradeType.달러웨이브:
                curValueText.text = GameManager.instance.waveBonusDollar.ToString();
                break;
            case UtilUpgradeType.달러보너스:
                curValueText.text = GameManager.instance.dollarBonusFactor.ToString();
                break;
            default:
                break;
        }

    }

    public void OnUpBtClk()
    {
        GameManager.instance.CurDollar -= upCost;

        switch (myUpType)
        {
            case UtilUpgradeType.달러웨이브:
                GameManager.instance.waveBonusDollar += 4;
                // 업그레이드 비용 .1배씩 올려주기
                upCost = Mathf.RoundToInt(upCost * upFactor);
                break;
            case UtilUpgradeType.달러보너스:

                GameManager.instance.dollarBonusFactor += .5f;
                upCost = Mathf.RoundToInt(upCost * upFactor);
                break;
            default:
                break;
        }
    }

    public void SetUpType(int upType)
    {
        myUpType = (UtilUpgradeType)upType;
    }
}
