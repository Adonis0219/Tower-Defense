using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
public enum UtilUpgradeType
{
    �޷����̺�,
    �޷����ʽ�,
    Length
}

public class UtilUpgradeButton : MonoBehaviour
{
    [SerializeField]
    public UtilUpgradeType upType;

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
            case UtilUpgradeType.�޷����̺�:
                curValueText.text = GameManager.instance.waveBonusDollar.ToString();
                break;
            case UtilUpgradeType.�޷����ʽ�:
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
                case UtilUpgradeType.�޷����̺�:
                    GameManager.instance.waveBonusDollar += 4;
                    // ���׷��̵� ��� .1�辿 �÷��ֱ�
                    upCost = Mathf.RoundToInt(upCost * upFactor);
                    break;
                case UtilUpgradeType.�޷����ʽ�:

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
