using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
public enum UtilUpgradeType
{
    �޷����ʽ�,
    �޷����̺�,
    ����ų���ʽ�,
    ���ο��̺�,
    ������ݾ�,
    �������,
    ������ƿ��,
    ���ڿ��̺�,
    Length
}

public class UtilUpgradeButton : UpgradeButton
{
    [SerializeField]
    public UtilUpgradeType myUpType;

    private void Update()
    {
        bt.interactable = GameManager.instance.CurDollar < SetCost(2) ? false : true;

        SetMultiText(2);

        costText.text = "$" + Change.Num(SetCost(2));

        switch (myUpType)
        {
            case UtilUpgradeType.�޷����ʽ�:
                curValueText.text = "x" + GameManager.instance.DollarBonusFactor.ToString("F2");
                break;
            case UtilUpgradeType.�޷����̺�:
                curValueText.text = GameManager.instance.DollarWaveBonus.ToString("F1");
                break;
            case UtilUpgradeType.����ų���ʽ�:
                curValueText.text = "x" + GameManager.instance.CoinKillBonus.ToString("F2");
                break;
            case UtilUpgradeType.���ο��̺�:
                curValueText.text = GameManager.instance.CoinWaveBonus.ToString("F1");
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

    public override void OnUpBtClk()
    {
        GameManager.instance.CurDollar -= SetCost(2);

        GameManager.instance.utilDollarLevels[(int)myUpType] += GameManager.instance.curMultis[2];

        for (int i = 0; i < GameManager.instance.curMultis[2]; i++)
        {
            upCost = Mathf.RoundToInt(upCost * upFactor);
        }

        base.OnUpBtClk();
    }

    public override void SetUpType(int upType)
    {
        myUpType = (UtilUpgradeType)upType;
    }
}
