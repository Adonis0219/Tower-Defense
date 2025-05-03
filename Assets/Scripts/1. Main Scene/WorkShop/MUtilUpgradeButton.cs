using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MUtilUpgradeButton : MUpgradeButton
{
    [SerializeField]
    public UtilUpgradeType myUpType;

    int cost;

    private void Update()
    {
        cost = Mathf.FloorToInt(SetCost(2, (int)myUpType));

        bt.interactable = PlayDataManager.Instance.MainCoin > cost ? true : false;

        costText.text = "<sprite=12>" + Change.Num(cost);

        switch (myUpType)
        {
            case UtilUpgradeType.달러보너스:
                curValueText.text = "x" + PlayDataManager.Instance.DollarBonusFormula(SceneType.Main).ToString("F2");
                break;
            case UtilUpgradeType.달러웨이브:
                curValueText.text = PlayDataManager.Instance.DollarWaveFormula(SceneType.Main).ToString("F1");
                break;
            case UtilUpgradeType.코인킬보너스:
                curValueText.text = "x" + PlayDataManager.Instance.CoinKillBonusFormula(SceneType.Main).ToString("F2");
                break;
            case UtilUpgradeType.코인웨이브:
                curValueText.text = PlayDataManager.Instance.CoinWaveFormula(SceneType.Main).ToString("F1");
                break;
            case UtilUpgradeType.무료공격업:
                curValueText.text = (.5f * PlayDataManager.Instance.playData.utilCoinLevels[(int)myUpType]).ToString("F2") + "%";
                break;
            case UtilUpgradeType.무료방어업:
                curValueText.text = (.5f * PlayDataManager.Instance.playData.utilCoinLevels[(int)myUpType]).ToString("F2") + "%";
                break;
            case UtilUpgradeType.무료유틸업:
                curValueText.text = (.5f * PlayDataManager.Instance.playData.utilCoinLevels[(int)myUpType]).ToString("F2") + "%";
                break;
            case UtilUpgradeType.이자웨이브:
                break;
            case UtilUpgradeType.Length:
                break;
            default:
                break;
        }
    }

    public override void OnUpBtClk()
    {
        base.OnUpBtClk();

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.UpBtClk);

        PlayDataManager.Instance.MainCoin -= cost;

        PlayDataManager.Instance.playData.utilCoinLevels[(int)myUpType]++;
    }

    public override void SetUpType(int upType)
    {
        myUpType = (UtilUpgradeType)upType;
    }
}
