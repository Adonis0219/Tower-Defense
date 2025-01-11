using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
public enum UtilUpgradeType
{
    ĳ�ú��ʽ�,
    ĳ�ÿ��̺�,
    ���κ��ʽ�,
    ���ο��̺�,
    ������ݾ�,
    �������,
    ������ƿ��,
    ���ڿ��̺�,
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
            case UtilUpgradeType.ĳ�ú��ʽ�:
                curValueText.text = "x" + GameManager.instance.DollarBonusFactor.ToString("F2");
                break;
            case UtilUpgradeType.ĳ�ÿ��̺�:
                curValueText.text = GameManager.instance.DollarWaveBonus.ToString();
                break;
            case UtilUpgradeType.���κ��ʽ�:
                break;
            case UtilUpgradeType.���ο��̺�:
                break;
            case UtilUpgradeType.������ݾ�:
                break;
            case UtilUpgradeType.�������:
                break;
            case UtilUpgradeType.������ƿ��:
                break;
            case UtilUpgradeType.���ڿ��̺�:
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
