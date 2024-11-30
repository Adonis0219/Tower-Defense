using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class UtilUpgradeButton : MonoBehaviour
{
    public enum UpgradeType
    {
        �޷����̺�,
        �޷����ʽ�
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
            case UpgradeType.�޷����̺�:
                curValueText.text = GameManager.instance.waveBonusDollar.ToString();
                break;
            case UpgradeType.�޷����ʽ�:
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
                case UpgradeType.�޷����̺�:
                    GameManager.instance.waveBonusDollar += 4;
                    // ���׷��̵� ��� .1�辿 �÷��ֱ�
                    upCost = Mathf.RoundToInt(upCost * upFactor);
                    break;
                case UpgradeType.�޷����ʽ�:

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
