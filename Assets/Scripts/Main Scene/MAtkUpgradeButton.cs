using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class MAtkUpgradeButton : MonoBehaviour
{
    public enum UpgradeType
    {
        데미지,
        공격속도,
        치명타확률,
        치명타계수
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
            case UpgradeType.데미지:
                //curValueText.text = "" + 3 * (PlayData.instance.goodsData.atkCoinLevels[1] + 1);
                //curValueText.text = 3(기본 데미지) + mUpLevel * 3;
                break;
            case UpgradeType.공격속도:
                //curValueText.text = GameManager.instance.player.atkSpd.ToString();
                break;
            case UpgradeType.치명타확률:
                //curValueText.text = GameManager.instance.player.critChance.ToString("F2") + "%";
                break;
            case UpgradeType.치명타계수:
                //curValueText.text = GameManager.instance.player.critFactor.ToString("F2");
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
                case UpgradeType.데미지:
                    GameManager.instance.player.Damage += 3;
                    // 업그레이드 비용 .2배씩 올려주기
                    upCost = Mathf.RoundToInt(upCost * upFactor);
                    break;
                case UpgradeType.공격속도:
                    GameManager.instance.player.atkSpd += .05f;
                    upCost = Mathf.RoundToInt(upCost * upFactor);
                    break;
                case UpgradeType.치명타확률:
                    GameManager.instance.player.critChance += 1f;
                    upCost = Mathf.RoundToInt(upCost * upFactor);
                    break;
                case UpgradeType.치명타계수:
                    GameManager.instance.player.critFactor += .1f;
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
