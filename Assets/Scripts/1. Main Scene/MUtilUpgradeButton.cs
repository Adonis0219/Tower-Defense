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
            case UtilUpgradeType.달러웨이브:
                // Main화면으로 시작했을 때 게임매니저에 접근할 수 없으므로 계산식으로 넣어주기
                //curValueText.text = (3 * (PlayDataManager.Instance.playData.atkCoinLevels[(int)upType]+1)).ToString();
                break;
            case UtilUpgradeType.달러보너스:
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
