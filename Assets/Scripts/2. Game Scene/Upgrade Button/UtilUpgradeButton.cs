using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
public enum UtilUpgradeType
{
    �޷����̺�,
    �޷����ʽ�,
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
            case UtilUpgradeType.�޷����̺�:
                curValueText.text = GameManager.instance.waveBonusDollar.ToString();
                break;
            case UtilUpgradeType.�޷����ʽ�:
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
            case UtilUpgradeType.�޷����̺�:
                GameManager.instance.waveBonusDollar += 4;
                // ���׷��̵� ��� .1�辿 �÷��ֱ�
                upCost = Mathf.RoundToInt(upCost * upFactor);
                break;
            case UtilUpgradeType.�޷����ʽ�:

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
