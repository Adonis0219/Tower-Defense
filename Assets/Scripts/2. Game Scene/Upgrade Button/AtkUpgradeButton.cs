using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public enum AtkUpgradeType
{
    ������, ���ݼӵ�,
    ġ��ŸȮ��, ġ��Ÿ����,
    ����, �Ÿ��絥����,
    ��Ƽ��Ȯ��, ��Ƽ��ǥ��,
    �ٿ��Ȯ��, �ٿ��ǥ��, �ٿ������,
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
            case AtkUpgradeType.������:
                curValueText.text = GameManager.instance.player.Damage.ToString();                
                break;
            case AtkUpgradeType.���ݼӵ�:              
                curValueText.text = GameManager.instance.player.AtkSpd.ToString("F2");                
                break;
            case AtkUpgradeType.ġ��ŸȮ��:
                curValueText.text = GameManager.instance.player.CritChance.ToString("F2") + "%";
                break;
            case AtkUpgradeType.ġ��Ÿ����:
                curValueText.text = "x" + GameManager.instance.player.CritFactor.ToString("F2");
                break;
            case AtkUpgradeType.����:
                curValueText.text = GameManager.instance.player.Range.ToString("F2") + "m";
                break;
            case AtkUpgradeType.�Ÿ��絥����:
                curValueText.fontSize = 40;
                curValueText.text = "x" + GameManager.instance.player.DmgPerMeter.ToString("F4") + " / m";
                break;
            case AtkUpgradeType.��Ƽ��Ȯ��:
                curValueText.text = GameManager.instance.player.MultiChance.ToString("F2") + "%";
                break;
            case AtkUpgradeType.��Ƽ��ǥ��:
                curValueText.text = GameManager.instance.player.MultiCount.ToString();
                break;
            case AtkUpgradeType.�ٿ��Ȯ��:
                curValueText.text = GameManager.instance.player.BounceChance.ToString("F2") + "%";
                break;
            case AtkUpgradeType.�ٿ��ǥ��:
                curValueText.text = GameManager.instance.player.BounceCount.ToString();
                break;
            case AtkUpgradeType.�ٿ������:
                curValueText.text = GameManager.instance.player.BounceRange.ToString("F1") + "m";
                break;
            default:
                break;
        }
    }

    public void OnUpBtClk()
    {
        // ���� �޷��� ��븸ŭ �������ֱ�
        GameManager.instance.CurDollar -= SetCost(0);

        // �ش� ���׷��̵� ��ư�� �ش��ϴ� �޷� ���� �÷��ֱ�
        GameManager.instance.atkDollarLevels[(int)myUpType] += GameManager.instance.curMultis[0];

        // ���׷��̵� ��� �÷��ֱ�
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
