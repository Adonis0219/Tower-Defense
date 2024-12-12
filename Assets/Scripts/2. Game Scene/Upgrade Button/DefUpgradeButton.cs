using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;
public enum DefUpgradeType
{
    체력,
    체력회복,
    방어력,
    절대방어,
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
                curValueText.text = GameManager.instance.player.regenHp.ToString("F2") + "/sec";
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

        //switch (upType)
        //{
        //    case UpgradeType.체력:
        //        GameManager.instance.player.MaxHp += 5;
        //        GameManager.instance.player.CurrentHp += 5;
        //        // 업그레이드 비용 .2배씩 올려주기
        //        break;
        //    case UpgradeType.체력회복:
        //        GameManager.instance.player.regenHp += .1f;
        //        break;
        //    case UpgradeType.방어력:
        //        GameManager.instance.player.def += .5f;
        //        break;
        //    case UpgradeType.절대방어:
        //        GameManager.instance.player.absDef += .5f;
        //        break;
        //    default:
        //        break;
        //}
        upCost = Mathf.RoundToInt(upCost * upFactor);
    }
}
