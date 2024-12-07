using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public enum AtkUpgradeType
{
    데미지,
    공격속도,
    치명타확률,
    치명타계수,
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
            case AtkUpgradeType.데미지:
                //curValueText.text = (GameManager.instance.player.Damage + 3 * (GameManager.instance.atkCoinLevel[(int)upType]+ dollarLevel[(int)upType])).ToString();                
                curValueText.text = GameManager.instance.player.Damage.ToString();                
                break;
            case AtkUpgradeType.공격속도:              
                curValueText.text = GameManager.instance.player.AtkSpd.ToString("F2");                
                break;
            case AtkUpgradeType.치명타확률:
                curValueText.text = GameManager.instance.player.critChance.ToString("F2") + "%";
                break;
            case AtkUpgradeType.치명타계수:
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
        // 업그레이드 비용 .2배씩 올려주기
        upCost = Mathf.RoundToInt(upCost * upFactor);
    }
}
