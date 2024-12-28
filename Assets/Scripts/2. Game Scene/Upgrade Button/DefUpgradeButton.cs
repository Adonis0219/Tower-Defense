using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;
public enum DefUpgradeType
{
    체력, 체력회복,
    방어력, 절대방어,
    반사데미지, 흡혈,
    넉백확률, 넉백강도,
    오브속도, 오브,
    Length
}   


public class DefUpgradeButton : MonoBehaviour
{
    [SerializeField]
    public DefUpgradeType upType;

    [SerializeField]
    int upCost;

    [SerializeField]
    float upFactor;

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
        bt.interactable = GameManager.instance.CurDollar < upCost ? false : true;

        costText.text = "$" + upCost;

        switch (upType)
        {
            case DefUpgradeType.체력:
                curValueText.text = GameManager.instance.player.MaxHp.ToString();
                break;
            case DefUpgradeType.체력회복:
                curValueText.text = GameManager.instance.player.RegenHp.ToString("F2") + "/sec";
                break;
            case DefUpgradeType.방어력:
                curValueText.text = GameManager.instance.player.Def.ToString("F2") + "%";
                break;
            case DefUpgradeType.절대방어:
                curValueText.text = GameManager.instance.player.AbsDef.ToString("F2");
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
        GameManager.instance.CurDollar -= upCost;

        GameManager.instance.defDollarLevels[(int)upType] ++;

        // 업그레이드 버튼이 체력타입일 경우 현재 체력 올려주기
        if (upType == DefUpgradeType.체력)
            GameManager.instance.player.CurrentHp += 5;

        upCost = Mathf.RoundToInt(upCost * upFactor);
    }
}
