using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class UtilUpgradeButton : MonoBehaviour
{
    public enum UpgradeType
    {
        달러웨이브,
        달러보너스
    }   

    [SerializeField]
    public UpgradeType upType;

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

    private void Update()
    {
        costText.text = "$" + upCost;

        switch (upType)
        {
            case UpgradeType.달러웨이브:
                curValueText.text = GameManager.instance.waveBonusDollar.ToString();
                break;
            case UpgradeType.달러보너스:
                curValueText.text = GameManager.instance.dollarBonusFactor.ToString();
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
        if (GameManager.instance.CurDollar >= upCost)
        {
            GameManager.instance.CurDollar -= upCost;

            switch (upType)
            {
                case UpgradeType.달러웨이브:
                    GameManager.instance.waveBonusDollar += 4;
                    // 업그레이드 비용 .1배씩 올려주기
                    upCost = Mathf.RoundToInt(upCost * upFactor);
                    break;
                case UpgradeType.달러보너스:

                    GameManager.instance.dollarBonusFactor += .5f;
                    upCost = Mathf.RoundToInt(upCost * upFactor);
                    break;
                default:
                    break;
            }
        }
        else
        {
            StartCoroutine(GameManager.instance.LackDollar());
        }
    }
}
