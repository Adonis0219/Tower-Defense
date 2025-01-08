using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;
public enum DefUpgradeType
{
    ü��, ü��ȸ��,
    ����, ������,
    �ݻ絥����, ����,
    �˹�Ȯ��, �˹鰭��,
    ����ӵ�, ����,
    Length
}   


public class DefUpgradeButton : UpgradeButton, ISetUpType
{
    [SerializeField]
    public DefUpgradeType myUpType;

    private void Update()
    {
        bt.interactable = GameManager.instance.CurDollar < upCost ? false : true;

        costText.text = "$" + upCost;

        switch (myUpType)
        {
            case DefUpgradeType.ü��:
                curValueText.text = GameManager.instance.player.MaxHp.ToString();
                break;
            case DefUpgradeType.ü��ȸ��:
                curValueText.text = GameManager.instance.player.RegenHp.ToString("F2") + "/sec";
                break;
            case DefUpgradeType.����:
                curValueText.text = GameManager.instance.player.Def.ToString("F2") + "%";
                break;
            case DefUpgradeType.������:
                curValueText.text = GameManager.instance.player.AbsDef.ToString("F2");
                break;
            default:
                break;
        }

    }

    public void OnUpBtClk()
    {
        GameManager.instance.CurDollar -= upCost;

        GameManager.instance.defDollarLevels[(int)myUpType] ++;

        // ���׷��̵� ��ư�� ü��Ÿ���� ��� ���� ü�� �÷��ֱ�
        if (myUpType == DefUpgradeType.ü��)
            GameManager.instance.player.CurrentHp += 5;

        upCost = Mathf.RoundToInt(upCost * upFactor);
    }

    public void SetUpType(int upType)
    {
        myUpType = (DefUpgradeType)upType;
    }
}
