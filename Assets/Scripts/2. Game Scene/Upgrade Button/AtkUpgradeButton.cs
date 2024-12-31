using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public enum AtkUpgradeType
{
    ������, ���ݼӵ�,
    ġ��ŸȮ��, ġ��Ÿ������,
    ����, �Ÿ��絥����,
    ��Ƽ��Ȯ��, ��Ƽ��ǥ��,
    �ٿ��Ȯ��, �ٿ��ǥ��, �ٿ������,
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
            case AtkUpgradeType.������:
                curValueText.text = GameManager.instance.player.Damage.ToString();                
                break;
            case AtkUpgradeType.���ݼӵ�:              
                curValueText.text = GameManager.instance.player.AtkSpd.ToString("F2");                
                break;
            case AtkUpgradeType.ġ��ŸȮ��:
                curValueText.text = GameManager.instance.player.CritChance.ToString("F2") + "%";
                break;
            case AtkUpgradeType.ġ��Ÿ������:
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
        // ���� �޷��� ��븸ŭ �������ֱ�
        GameManager.instance.CurDollar -= upCost;

        // �ش� ���׷��̵� ��ư�� �ش��ϴ� �޷� ���� �÷��ֱ�
        GameManager.instance.atkDollarLevels[(int)upType]++;

        // ���׷��̵� ��� .2�辿 �÷��ֱ�
        upCost = Mathf.RoundToInt(upCost * upFactor);
    }
}
