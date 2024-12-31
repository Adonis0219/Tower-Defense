using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MAtkUpgradeButton : MUpgradeButton, ISetUpType
{
    [SerializeField]
    public AtkUpgradeType myUpType;

    private void Update()
    {
        bt.interactable = PlayDataManager.Instance.MainCoin > upCost ? true : false;

        costText.text = "<sprite=12>" + upCost;

        switch (myUpType)
        {
            case AtkUpgradeType.데미지:
                // Main화면으로 시작했을 때 게임매니저에 접근할 수 없으므로 계산식으로 넣어주기
                curValueText.text = (3 * (PlayDataManager.Instance.playData.atkCoinLevels[(int)myUpType]+1)).ToString();
                break;
            case AtkUpgradeType.공격속도:
                curValueText.text = (1 + .05f * PlayDataManager.Instance.playData.atkCoinLevels[(int)myUpType]).ToString("F2");
                break;
            case AtkUpgradeType.치명타확률:
                curValueText.text = (1 + PlayDataManager.Instance.playData.atkCoinLevels[(int)myUpType]).ToString("F2") + "%";
                break;
            case AtkUpgradeType.치명타데미지:
                curValueText.text = "x" + (1.2f + .1f * PlayDataManager.Instance.playData.atkCoinLevels[(int)myUpType]).ToString("F2");
                break;
            case AtkUpgradeType.범위:
                curValueText.text = (30 + (.5f * PlayDataManager.Instance.playData.atkCoinLevels[(int)myUpType])).ToString("F2")+ "m";
                break;
            case AtkUpgradeType.거리당데미지:
                curValueText.fontSize = 40;
                curValueText.text = "x" + (1 + (.0008f * PlayDataManager.Instance.playData.atkCoinLevels[(int)myUpType])).ToString("F4") + " / m";
                break;
            case AtkUpgradeType.멀티샷확률:
                curValueText.text = (.5f * PlayDataManager.Instance.playData.atkCoinLevels[(int)myUpType]).ToString("F2") + "%";
                break;
            case AtkUpgradeType.멀티샷표적:
                curValueText.text = 2 + PlayDataManager.Instance.playData.atkCoinLevels[(int)myUpType] + "";
                break;
            default:
                break;
        }
    }

    public void OnUpBtClk()
    {
        PlayDataManager.Instance.MainCoin -= upCost;

        PlayDataManager.Instance.playData.atkCoinLevels[(int)myUpType]++;

        upCost = Mathf.RoundToInt(upCost * upFactor);
    }

    public void SetUpType(int upType)
    {
        myUpType = (AtkUpgradeType)upType;
    }
}
