using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MAtkUpgradeButton : MUpgradeButton
{
    [SerializeField]
    public AtkUpgradeType myUpType;

    private void Update()
    {
        bt.interactable = PlayDataManager.Instance.MainCoin > upCost ? true : false;

        costText.text = "<sprite=12>" + Mathf.FloorToInt(upCost * Sale(MainRschType.��������));

        switch (myUpType)
        {
            case AtkUpgradeType.������:
                // Mainȭ������ �������� �� ���ӸŴ����� ������ �� �����Ƿ� �������� �־��ֱ�      
                curValueText.text = PlayDataManager.Instance.DmgFormula(SceneType.Main).ToString();
                break;
            case AtkUpgradeType.���ݼӵ�:
                curValueText.text = PlayDataManager.Instance.AtkSpdFormula(SceneType.Main).ToString("F2");
                break;
            case AtkUpgradeType.ġ��ŸȮ��:
                curValueText.text = (1 + PlayDataManager.Instance.playData.atkCoinLevels[(int)myUpType]).ToString("F2") + "%";
                break;
            case AtkUpgradeType.ġ��Ÿ����:
                curValueText.text = "x" + PlayDataManager.Instance.CritFactorFormula(SceneType.Main).ToString("F2");
                break;
            case AtkUpgradeType.����:
                curValueText.text = (PlayDataManager.Instance.RangeFormula(SceneType.Main)).ToString("F2") + "m";
                break;
            case AtkUpgradeType.�Ÿ��絥����:
                curValueText.fontSize = 40;
                curValueText.text = "x" + (1 + (.0008f * PlayDataManager.Instance.playData.atkCoinLevels[(int)myUpType])).ToString("F4") + " / m";
                break;
            case AtkUpgradeType.��Ƽ��Ȯ��:
                curValueText.text = (.5f * PlayDataManager.Instance.playData.atkCoinLevels[(int)myUpType]).ToString("F2") + "%";
                break;
            case AtkUpgradeType.��Ƽ��ǥ��:
                curValueText.text = 2 + PlayDataManager.Instance.playData.atkCoinLevels[(int)myUpType] + "";
                break;
            case AtkUpgradeType.�ٿ��Ȯ��:
                curValueText.text = (.5f * PlayDataManager.Instance.playData.atkCoinLevels[(int)myUpType]).ToString("F2") + "%";
                break;
            case AtkUpgradeType.�ٿ��ǥ��:
                curValueText.text = 2 + PlayDataManager.Instance.playData.atkCoinLevels[(int)myUpType] + "";
                break;
            case AtkUpgradeType.�ٿ������:
                curValueText.text = (10 + .1f * (PlayDataManager.Instance.playData.atkCoinLevels[(int)myUpType])).ToString("F1") + "m";
                break;
            default:
                break;
        }
    }

    // ���׷��̵� ��ư�� ������ �� ������ �Լ�
    public void OnUpBtClk()
    {
        PlayDataManager.Instance.MainCoin -= Mathf.FloorToInt(upCost * Sale(MainRschType.��������));

        PlayDataManager.Instance.playData.atkCoinLevels[(int)myUpType]++;

        upCost = Mathf.RoundToInt(upCost * upFactor);
    }

    public override void SetUpType(int upType)
    {
        myUpType = (AtkUpgradeType)upType;
    }
}
