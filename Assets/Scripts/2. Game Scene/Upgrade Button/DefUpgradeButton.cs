using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;



public class DefUpgradeButton : MonoBehaviour
{
    public enum UpgradeType
    {
        체력,
        체력회복,
        방어력,
        절대방어
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
            case UpgradeType.체력:
                curValueText.text = GameManager.instance.player.MaxHp.ToString();
                break;
            case UpgradeType.체력회복:
                curValueText.text = GameManager.instance.player.regenHp.ToString("F2") + "/sec";
                break;
            case UpgradeType.방어력:
                curValueText.text = GameManager.instance.player.def.ToString("F2") + "%";
                break;
            case UpgradeType.절대방어:
                curValueText.text = GameManager.instance.player.absDef.ToString("F2");
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
                case UpgradeType.체력:
                    GameManager.instance.player.MaxHp += 5;
                    GameManager.instance.player.CurrentHp += 5;
                    // 업그레이드 비용 .2배씩 올려주기
                    break;
                case UpgradeType.체력회복:
                    GameManager.instance.player.regenHp += .1f;
                    break;
                case UpgradeType.방어력:
                    GameManager.instance.player.def += .5f;
                    break;
                case UpgradeType.절대방어:
                    GameManager.instance.player.absDef += .5f;
                    break;
                default:
                    break;
            }
            upCost = Mathf.RoundToInt(upCost * upFactor);
        }
        else
        {
            StartCoroutine(GameManager.instance.LackDollar());
        }
    }
}
