using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;
public enum DefUpgradeType
{
    체력, 체력회복,
    방어력, 절대방어,
    가시대미지, 흡혈,
    넉백확률, 넉백강도,
    오브속도, 오브,
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
            case DefUpgradeType.체력:
                curValueText.text = GameManager.instance.player.MaxHp.ToString();
                break;
            case DefUpgradeType.체력회복:
                curValueText.text = GameManager.instance.player.RegenHp.ToString("F2") + "/sec";
                break;
            case DefUpgradeType.방어력:
                curValueText.text = GameManager.instance.player.Def.ToString("F2") + "%";
                break;
            case DefUpgradeType.절대방어:
                curValueText.text = GameManager.instance.player.AbsDef.ToString("F2");
                break;
            case DefUpgradeType.가시대미지:
                curValueText.text = GameManager.instance.player.ThronsPer.ToString("F2") + "%";
                break;
            case DefUpgradeType.흡혈:
                curValueText.text = GameManager.instance.player.LifeStealPer.ToString("F2") + "%";
                break;
            case DefUpgradeType.넉백확률:
                curValueText.text = GameManager.instance.player.KnockbackChance.ToString("F2") + "%";
                break;
            case DefUpgradeType.넉백강도:
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
            // 업그레이드 버튼이 체력타입일 경우 현재 체력 올려주기
            if (myUpType == DefUpgradeType.체력)
                GameManager.instance.player.CurrentHp += 5 * (1 + .03f * PlayDataManager.Instance.playData.labResearchLevels[(int)ResearchType.Defense, (int)DefRschType.체력]);

            upCost = Mathf.RoundToInt(upCost * upFactor);
        }
    }

    public void SetUpType(int upType)
    {
        myUpType = (DefUpgradeType)upType;
    }
}
