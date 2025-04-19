using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MUtilUpgradeButton : MUpgradeButton
{
    [SerializeField]
    public UtilUpgradeType myUpType;

    private void Update()
    {
        bt.interactable = PlayDataManager.Instance.MainCoin > upCost ? true : false;

        costText.text = "<sprite=12>" + Mathf.FloorToInt(upCost * Sale(MainRschType.��ƿ����));

        switch (myUpType)
        {
            case UtilUpgradeType.�޷����ʽ�:
                curValueText.text = "x" + PlayDataManager.Instance.DollarBonusFormula(SceneType.Main).ToString("F2");
                break;
            case UtilUpgradeType.�޷����̺�:
                curValueText.text = PlayDataManager.Instance.DollarWaveFormula(SceneType.Main).ToString("F1");
                break;
            case UtilUpgradeType.����ų���ʽ�:
                curValueText.text = "x" + PlayDataManager.Instance.CoinKillBonusFormula(SceneType.Main).ToString("F2");
                break;
            case UtilUpgradeType.���ο��̺�:
                curValueText.text = PlayDataManager.Instance.CoinWaveFormula(SceneType.Main).ToString("F1");
                break;
            case UtilUpgradeType.������ݾ�:
                curValueText.text = (.5f * PlayDataManager.Instance.playData.utilCoinLevels[(int)myUpType]).ToString("F2") + "%";
                break;
            case UtilUpgradeType.�������:
                curValueText.text = (.5f * PlayDataManager.Instance.playData.utilCoinLevels[(int)myUpType]).ToString("F2") + "%";
                break;
            case UtilUpgradeType.������ƿ��:
                curValueText.text = (.5f * PlayDataManager.Instance.playData.utilCoinLevels[(int)myUpType]).ToString("F2") + "%";
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
        PlayDataManager.Instance.MainCoin -= Mathf.FloorToInt(upCost * Sale(MainRschType.��ƿ����));

        PlayDataManager.Instance.playData.utilCoinLevels[(int)myUpType]++;

        upCost = Mathf.RoundToInt(upCost * upFactor);
    }

    public override void SetUpType(int upType)
    {
        myUpType = (UtilUpgradeType)upType;
    }
}
