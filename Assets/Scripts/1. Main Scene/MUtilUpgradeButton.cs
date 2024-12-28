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

        costText.text = "<sprite=12>" + upCost;

        switch (myUpType)
        {
            case UtilUpgradeType.�޷����̺�:
                // Mainȭ������ �������� �� ���ӸŴ����� ������ �� �����Ƿ� �������� �־��ֱ�
                //curValueText.text = (3 * (PlayDataManager.Instance.playData.atkCoinLevels[(int)upType]+1)).ToString();
                break;
            case UtilUpgradeType.�޷����ʽ�:
                //curValueText.text = (1 + .05f * PlayDataManager.Instance.playData.atkCoinLevels[(int)upType]).ToString("F2");
                break;
            default:
                break;
        }
    }

    public void OnUpBtClk()
    {
        PlayDataManager.Instance.MainCoin -= upCost;

        PlayDataManager.Instance.playData.utilCoinLevels[(int)myUpType]++;

        upCost = Mathf.RoundToInt(upCost * upFactor);
    }

    public void SetUpType(int upType)
    {
        myUpType = (UtilUpgradeType)upType;
    }
}
