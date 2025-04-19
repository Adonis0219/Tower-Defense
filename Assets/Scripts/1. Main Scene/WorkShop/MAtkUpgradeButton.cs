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

        costText.text = "<sprite=12>" + Mathf.FloorToInt(upCost * Sale(MainRschType.공격할인));

        switch (myUpType)
        {
            case AtkUpgradeType.데미지:
                // Main화면으로 시작했을 때 게임매니저에 접근할 수 없으므로 계산식으로 넣어주기      
                curValueText.text = PlayDataManager.Instance.DmgFormula(SceneType.Main).ToString();
                break;
            case AtkUpgradeType.공격속도:
                curValueText.text = PlayDataManager.Instance.AtkSpdFormula(SceneType.Main).ToString("F2");
                break;
            case AtkUpgradeType.치명타확률:
                curValueText.text = (1 + PlayDataManager.Instance.playData.atkCoinLevels[(int)myUpType]).ToString("F2") + "%";
                break;
            case AtkUpgradeType.치명타배율:
                curValueText.text = "x" + PlayDataManager.Instance.CritFactorFormula(SceneType.Main).ToString("F2");
                break;
            case AtkUpgradeType.범위:
                curValueText.text = (PlayDataManager.Instance.RangeFormula(SceneType.Main)).ToString("F2") + "m";
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
            case AtkUpgradeType.바운스샷확률:
                curValueText.text = (.5f * PlayDataManager.Instance.playData.atkCoinLevels[(int)myUpType]).ToString("F2") + "%";
                break;
            case AtkUpgradeType.바운스샷표적:
                curValueText.text = 2 + PlayDataManager.Instance.playData.atkCoinLevels[(int)myUpType] + "";
                break;
            case AtkUpgradeType.바운스샷범위:
                curValueText.text = (10 + .1f * (PlayDataManager.Instance.playData.atkCoinLevels[(int)myUpType])).ToString("F1") + "m";
                break;
            default:
                break;
        }
    }

    // 업그레이드 버튼을 눌렀을 때 실행할 함수
    public void OnUpBtClk()
    {
        PlayDataManager.Instance.MainCoin -= Mathf.FloorToInt(upCost * Sale(MainRschType.공격할인));

        PlayDataManager.Instance.playData.atkCoinLevels[(int)myUpType]++;

        upCost = Mathf.RoundToInt(upCost * upFactor);
    }

    public override void SetUpType(int upType)
    {
        myUpType = (AtkUpgradeType)upType;
    }
}
