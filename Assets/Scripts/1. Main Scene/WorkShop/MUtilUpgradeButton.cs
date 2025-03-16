using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MUtilUpgradeButton : MUpgradeButton, ISetUpType
{
    [SerializeField]
    public UtilUpgradeType myUpType;

    private void Update()
    {
        bt.interactable = PlayDataManager.Instance.MainCoin > upCost ? true : false;

        costText.text = "<sprite=12>" + Mathf.FloorToInt(upCost * Sale(MainRschType.��ƿ����));

        switch (myUpType)
        {
            case UtilUpgradeType.ĳ�ú��ʽ�:
                curValueText.text = "x" + (1 + .01f * PlayDataManager.Instance.playData.utilCoinLevels[(int)myUpType]).ToString("F2");
                break;
            case UtilUpgradeType.ĳ�ÿ��̺�:
                curValueText.text = (4 * PlayDataManager.Instance.playData.utilCoinLevels[(int)myUpType]).ToString();
                break;
            case UtilUpgradeType.����ų���ʽ�:
                curValueText.text = "x" + (1 + .01f * PlayDataManager.Instance.playData.utilCoinLevels[(int)myUpType]).ToString("F2");
                break;
            case UtilUpgradeType.���ο��̺�:
                curValueText.text = PlayDataManager.Instance.playData.utilCoinLevels[(int)myUpType].ToString();
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

    public void SetUpType(int upType)
    {
        myUpType = (UtilUpgradeType)upType;
    }
}
