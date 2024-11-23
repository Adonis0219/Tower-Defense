using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class MAtkUpgradeButton : MonoBehaviour
{
    public enum UpgradeType
    {
        ������,
        ���ݼӵ�,
        ġ��ŸȮ��,
        ġ��Ÿ���
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
            case UpgradeType.������:
                //curValueText.text = "" + 3 * (PlayData.instance.goodsData.atkCoinLevels[1] + 1);
                //curValueText.text = 3(�⺻ ������) + mUpLevel * 3;
                break;
            case UpgradeType.���ݼӵ�:
                //curValueText.text = GameManager.instance.player.atkSpd.ToString();
                break;
            case UpgradeType.ġ��ŸȮ��:
                //curValueText.text = GameManager.instance.player.critChance.ToString("F2") + "%";
                break;
            case UpgradeType.ġ��Ÿ���:
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
                case UpgradeType.������:
                    GameManager.instance.player.Damage += 3;
                    // ���׷��̵� ��� .2�辿 �÷��ֱ�
                    upCost = Mathf.RoundToInt(upCost * upFactor);
                    break;
                case UpgradeType.���ݼӵ�:
                    GameManager.instance.player.atkSpd += .05f;
                    upCost = Mathf.RoundToInt(upCost * upFactor);
                    break;
                case UpgradeType.ġ��ŸȮ��:
                    GameManager.instance.player.critChance += 1f;
                    upCost = Mathf.RoundToInt(upCost * upFactor);
                    break;
                case UpgradeType.ġ��Ÿ���:
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
