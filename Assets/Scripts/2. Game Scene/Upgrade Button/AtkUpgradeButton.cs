using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public enum AtkUpgradeType
{
    데미지, 공격속도,
    치명타확률, 치명타배율,
    범위, 거리당데미지,
    멀티샷확률, 멀티샷표적,
    바운스샷확률, 바운스샷표적, 바운스샷범위,
    Length
}   

public class AtkUpgradeButton : UpgradeButton
{

    [SerializeField]
    public AtkUpgradeType myUpType;

    private void Update()
    {
        //bt.interactable = GameManager.instance.CurDollar < SetCost(0) ? false : true;
        bt.interactable = GameManager.instance.CurDollar < upCost ? false : true;

        SetMultiText(0);

        //costText.text = "$" + SetCost(0);
        costText.text = "$" + upCost;

        switch (myUpType)
        {
            case AtkUpgradeType.데미지:
                curValueText.text = GameManager.instance.player.Damage.ToString();                
                break;
            case AtkUpgradeType.공격속도:              
                curValueText.text = GameManager.instance.player.AtkSpd.ToString("F2");                
                break;
            case AtkUpgradeType.치명타확률:
                curValueText.text = GameManager.instance.player.CritChance.ToString("F2") + "%";
                break;
            case AtkUpgradeType.치명타배율:
                curValueText.text = "x" + GameManager.instance.player.CritFactor.ToString("F2");
                break;
            case AtkUpgradeType.범위:
                curValueText.text = GameManager.instance.player.Range.ToString("F2") + "m";
                break;
            case AtkUpgradeType.거리당데미지:
                curValueText.fontSize = 40;
                curValueText.text = "x" + GameManager.instance.player.DmgPerMeter.ToString("F4") + " / m";
                break;
            case AtkUpgradeType.멀티샷확률:
                curValueText.text = GameManager.instance.player.MultiChance.ToString("F2") + "%";
                break;
            case AtkUpgradeType.멀티샷표적:
                curValueText.text = GameManager.instance.player.MultiCount.ToString();
                break;
            case AtkUpgradeType.바운스샷확률:
                curValueText.text = GameManager.instance.player.BounceChance.ToString("F2") + "%";
                break;
            case AtkUpgradeType.바운스샷표적:
                curValueText.text = GameManager.instance.player.BounceCount.ToString();
                break;
            case AtkUpgradeType.바운스샷범위:
                curValueText.text = GameManager.instance.player.BounceRange.ToString("F1") + "m";
                break;
            default:
                break;
        }
    }

    public void OnUpBtClk()
    {
        // 현재 달러를 비용만큼 차감해주기
        GameManager.instance.CurDollar -= SetCost(0);

        // 해당 업그레이드 버튼에 해당하는 달러 레벨 올려주기
        GameManager.instance.atkDollarLevels[(int)myUpType] += GameManager.instance.curMultis[0];

        // 업그레이드 비용 올려주기
        for (int i = 0; i < GameManager.instance.curMultis[0]; i++)
        {
            upCost = Mathf.RoundToInt(upCost * upFactor);
        }
    }

    public override void SetUpType(int upType)
    {
        myUpType = (AtkUpgradeType)upType;
    }
}
