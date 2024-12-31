using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public enum AtkUpgradeType
{
    데미지, 공격속도,
    치명타확률, 치명타데미지,
    범위, 거리당데미지,
    멀티샷확률, 멀티샷표적,
    바운스샷확률, 바운스샷표적, 바운스샷범위,
    Length
}   

public class AtkUpgradeButton : MonoBehaviour
{

    [SerializeField]
    public AtkUpgradeType upType;

    [SerializeField]
    int upCost;

    [SerializeField]
    float upFactor;

    //int[] dollarLevel = new int[(int)UpgradeType.Length];
    int dollarLevel = 0;

    [Header("# TextObjects")]
    [SerializeField]
    TextMeshProUGUI upNameText;
    [SerializeField]
    TextMeshProUGUI curValueText;
    [SerializeField]
    TextMeshProUGUI costText;

    [SerializeField]
    Button bt;

    private void Update()
    {
        curValueText.text  = ParseNumber.Parse(1000);
        bt.interactable = GameManager.instance.CurDollar < upCost ? false : true;   

        costText.text = "$" + upCost;

        switch (upType)
        {
            case AtkUpgradeType.데미지:
                curValueText.text = GameManager.instance.player.Damage.ToString();                
                break;
            case AtkUpgradeType.공격속도:              
                curValueText.text = GameManager.instance.player.AtkSpd.ToString("F2");                
                break;
            case AtkUpgradeType.치명타확률:
                curValueText.text = GameManager.instance.player.CritChance.ToString("F2") + "%";
                break;
            case AtkUpgradeType.치명타데미지:
                curValueText.text = "x" + GameManager.instance.player.CritFactor.ToString("F2");
                break;
            default:
                break;
        }
    }

    public void SetData(string name, int cost, float factor)
    {
        upNameText.text = name;
        upCost = cost;
        upFactor = factor;

        costText.text = "$" + cost;
    }

    public void OnUpBtClk()
    {
        // 현재 달러를 비용만큼 차감해주기
        GameManager.instance.CurDollar -= upCost;

        // 해당 업그레이드 버튼에 해당하는 달러 레벨 올려주기
        GameManager.instance.atkDollarLevels[(int)upType]++;

        // 업그레이드 비용 .2배씩 올려주기
        upCost = Mathf.RoundToInt(upCost * upFactor);
    }
}
