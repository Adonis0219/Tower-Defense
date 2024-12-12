using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;
public enum DefUpgradeType
{
    ü��,
    ü��ȸ��,
    ����,
    ������,
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
            case DefUpgradeType.ü��:
                curValueText.text = GameManager.instance.player.MaxHp.ToString();
                break;
            case DefUpgradeType.ü��ȸ��:
                curValueText.text = GameManager.instance.player.regenHp.ToString("F2") + "/sec";
                break;
            case DefUpgradeType.����:
                curValueText.text = GameManager.instance.player.Def.ToString("F2") + "%";
                break;
            case DefUpgradeType.������:
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
        //    case UpgradeType.ü��:
        //        GameManager.instance.player.MaxHp += 5;
        //        GameManager.instance.player.CurrentHp += 5;
        //        // ���׷��̵� ��� .2�辿 �÷��ֱ�
        //        break;
        //    case UpgradeType.ü��ȸ��:
        //        GameManager.instance.player.regenHp += .1f;
        //        break;
        //    case UpgradeType.����:
        //        GameManager.instance.player.def += .5f;
        //        break;
        //    case UpgradeType.������:
        //        GameManager.instance.player.absDef += .5f;
        //        break;
        //    default:
        //        break;
        //}
        upCost = Mathf.RoundToInt(upCost * upFactor);
    }
}
