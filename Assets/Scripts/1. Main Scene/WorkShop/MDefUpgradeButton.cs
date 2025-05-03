using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MDefUpgradeButton : MUpgradeButton
{
    [SerializeField]
    public DefUpgradeType myUpType;

    int cost;
   
    private void Update()
    {
        cost = Mathf.FloorToInt(SetCost(1, (int)myUpType));

        bt.interactable = PlayDataManager.Instance.MainCoin > cost ? true : false;

        costText.text = "<sprite=12>" + Change.Num(cost);

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

    public override void OnUpBtClk()
    {
        base.OnUpBtClk();

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.UpBtClk);
        
        PlayDataManager.Instance.MainCoin -= cost;

        PlayDataManager.Instance.playData.defCoinLevels[(int)myUpType]++;
    }

    public override void SetUpType(int upType)
    {
        myUpType = (DefUpgradeType)upType;
    }
}
