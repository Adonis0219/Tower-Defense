using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public enum AtkUpgradeType
{
    ������,
    ���ݼӵ�,
    ġ��ŸȮ��,
    ġ��Ÿ���,
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

    private void Start()
    {
        //for (int i = 0; i < dollarLevel.Length; i++)
        //{
        //    dollarLevel[i] = 0;
        //}

        Debug.Log(PlayDataManager.Instance.playData.atkCoinLevels[0]);
    }


    private void Update()
    {
        bt.interactable = GameManager.instance.CurDollar < upCost ? false : true;   

        costText.text = "$" + upCost;

        switch (upType)
        {
            case AtkUpgradeType.������:
                //curValueText.text = (GameManager.instance.player.Damage + 3 * (GameManager.instance.atkCoinLevel[(int)upType]+ dollarLevel[(int)upType])).ToString();                
                curValueText.text = GameManager.instance.player.Damage.ToString();                
                break;
            case AtkUpgradeType.���ݼӵ�:              
                curValueText.text = GameManager.instance.player.AtkSpd.ToString("F2");                
                break;
            case AtkUpgradeType.ġ��ŸȮ��:
                curValueText.text = GameManager.instance.player.critChance.ToString("F2") + "%";
                break;
            case AtkUpgradeType.ġ��Ÿ���:
                curValueText.text = GameManager.instance.player.critFactor.ToString("F2");
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
        if (GameManager.instance.CurDollar < upCost)
        {
            StartCoroutine(GameManager.instance.LackDollar());
            return;
        }

        GameManager.instance.CurDollar -= upCost;

        GameManager.instance.atkDollarLevels[(int)upType]++;
        // ���׷��̵� ��� .2�辿 �÷��ֱ�
        upCost = Mathf.RoundToInt(upCost * upFactor);
    }
}
