using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MDefUpgradeButton : MUpgradeButton, ISetUpType
{
    [SerializeField]
    public DefUpgradeType myUpType;
   
    private void Update()
    {
        bt.interactable = PlayDataManager.Instance.MainCoin > upCost ? true : false;

        costText.text = "<sprite=12>" + Mathf.FloorToInt(upCost * Sale(MainRschType.�������));

        switch (myUpType)
        {
            case DefUpgradeType.ü��:
                // Mainȭ������ �������� �� ���ӸŴ����� ������ �� �����Ƿ� �������� �־��ֱ�
                curValueText.text = PlayDataManager.Instance.HpFormula(SceneType.Main).ToString();
                break;
            case DefUpgradeType.ü��ȸ��:
                curValueText.text = PlayDataManager.Instance.HpRegenFormula(SceneType.Main).ToString("F2") + "/sec";
                break;
            case DefUpgradeType.����:
                curValueText.text = (.5f * PlayDataManager.Instance.playData.defCoinLevels[(int)myUpType]).ToString("F2") + "%";
                break;
            case DefUpgradeType.������:
                curValueText.text = PlayDataManager.Instance.AbsDefFormula(SceneType.Main).ToString("F2");
                break;
            case DefUpgradeType.���ô����:
                curValueText.text = (PlayDataManager.Instance.playData.defCoinLevels[(int)myUpType]).ToString("F2") + "%";
                break;
            case DefUpgradeType.����:
                curValueText.text = (.05f * (PlayDataManager.Instance.playData.defCoinLevels[(int)myUpType])).ToString("F2") + "%";
                break;
            case DefUpgradeType.�˹�Ȯ��:
                curValueText.text = (PlayDataManager.Instance.playData.defCoinLevels[(int)myUpType]).ToString("F2") + "%";
                break;
            case DefUpgradeType.�˹鰭��:
                curValueText.text = (.5f + .15f * (PlayDataManager.Instance.playData.defCoinLevels[(int)myUpType])).ToString("F2");
                break;
            default:
                break;
        }
    }

    public void OnUpBtClk()
    {
        PlayDataManager.Instance.MainCoin -= Mathf.FloorToInt(upCost * Sale(MainRschType.�������));

        PlayDataManager.Instance.playData.defCoinLevels[(int)myUpType]++;

        upCost = Mathf.RoundToInt(upCost * upFactor);
    }

    public void SetUpType(int upType)
    {
        myUpType = (DefUpgradeType)upType;
    }
}
