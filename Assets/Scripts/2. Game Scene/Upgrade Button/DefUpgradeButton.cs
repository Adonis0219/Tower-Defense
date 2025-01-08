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
    반사데미지, 흡혈,
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

        costText.text = "$" + upCost;

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
            default:
                break;
        }

    }

    public void OnUpBtClk()
    {
        GameManager.instance.CurDollar -= upCost;

        GameManager.instance.defDollarLevels[(int)myUpType] ++;

        // 업그레이드 버튼이 체력타입일 경우 현재 체력 올려주기
        if (myUpType == DefUpgradeType.체력)
            GameManager.instance.player.CurrentHp += 5;

        upCost = Mathf.RoundToInt(upCost * upFactor);
    }

    public void SetUpType(int upType)
    {
        myUpType = (DefUpgradeType)upType;
    }
}
