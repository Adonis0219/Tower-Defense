using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
public enum UtilUpgradeType
{
    ĳ�ú��ʽ�,
    ĳ�ÿ��̺�,
    ����ų���ʽ�,
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
            case UtilUpgradeType.����ų���ʽ�:
                curValueText.text = "x" + GameManager.instance.CoinKillBonus.ToString("F2");
                break;
            case UtilUpgradeType.���ο��̺�:
                curValueText.text = GameManager.instance.CoinWaveBonus.ToString();
                break;
            case UtilUpgradeType.������ݾ�:
                curValueText.text = GameManager.instance.AtkFreeUpChance.ToString("F2") + "%";
                break;
            case UtilUpgradeType.�������:
                curValueText.text = GameManager.instance.DefFreeUpChance.ToString("F2") + "%";
                break;
            case UtilUpgradeType.������ƿ��:
                curValueText.text = GameManager.instance.UtilFreeUpChance.ToString("F2") + "%";
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
