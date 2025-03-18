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
    ���ô����, ����,
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

        SetMultiText(1);

        costText.text = "$" + SetCost(1);

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
            case DefUpgradeType.���ô����:
                curValueText.text = GameManager.instance.player.ThronsPer.ToString("F2") + "%";
                break;
            case DefUpgradeType.����:
                curValueText.text = GameManager.instance.player.LifeStealPer.ToString("F2") + "%";
                break;
            case DefUpgradeType.�˹�Ȯ��:
                curValueText.text = GameManager.instance.player.KnockbackChance.ToString("F2") + "%";
                break;
            case DefUpgradeType.�˹鰭��:
                curValueText.text = GameManager.instance.player.KnockbackForce.ToString("F2");
                break;
            default:
                break;
        }
    }

    public void OnUpBtClk()
    {
        GameManager.instance.CurDollar -= SetCost(1);

        GameManager.instance.defDollarLevels[(int)myUpType] += GameManager.instance.curMultis[1];

        for (int i = 0; i < GameManager.instance.curMultis[1]; i++)
        {
            // ���׷��̵� ��ư�� ü��Ÿ���� ��� ���� ü�� �÷��ֱ�
            if (myUpType == DefUpgradeType.ü��)
                GameManager.instance.player.CurrentHp += 5 * (1 + .03f * PlayDataManager.Instance.playData.labResearchLevels[(int)ResearchType.Defense, (int)DefRschType.ü��]);

            upCost = Mathf.RoundToInt(upCost * upFactor);
        }
    }

    public void SetUpType(int upType)
    {
        myUpType = (DefUpgradeType)upType;
    }
}
