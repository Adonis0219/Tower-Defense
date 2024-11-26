using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using System.Security.Cryptography;
using UnityEngine.UIElements;

public class MAtkUpgradeButton : MonoBehaviour
{
    public enum UpgradeType
    {
        ������,
        ���ݼӵ�,
        ġ��ŸȮ��,
        ġ��Ÿ���,
        Length
    }   

    [SerializeField]
    public UpgradeType upType;

    [SerializeField]
    int upCost;

    [SerializeField]
    float upFactor;

    int[] coinLevel = new int[(int)UpgradeType.Length];

    [Header("# TextObjects")]
    [SerializeField]
    TextMeshProUGUI upNameText;
    [SerializeField]
    TextMeshProUGUI curValueText;
    [SerializeField]
    TextMeshProUGUI costText;
   
    private void Update()
    {
        costText.text = "<sprite=12>" + upCost;

        switch (upType)
        {
            case UpgradeType.������:
                curValueText.text = (3 * (coinLevel[(int)upType]+1)).ToString();
                break;
            case UpgradeType.���ݼӵ�:
                curValueText.text = (1 + .05f * coinLevel[(int)upType]).ToString("F2");
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

    /// <summary>
    /// ���׷��̵� ��ư�� �⺻ �����͸� Set���ִ� �Լ�
    /// </summary>
    /// <param name="name">��ư�� �̸�</param>
    /// <param name="cost">��ư�� ���</param>
    /// <param name="factor">��ư�� ����</param>
    public void SetData(string name, int cost, float factor)
    {
        upNameText.text = name;
        upCost = cost;
        upFactor = factor;

        costText.text = "$" + cost;

        for (int i = 0; i < coinLevel.Length; i++)
        {
            coinLevel[i] = PlayDataManager.Instance.playData.atkCoinLevels[(int)upType];
        }
    }

    public void OnUpBtClk()
    {
        if (PlayDataManager.Instance.playData.curCoin >= upCost)
        {
            PlayDataManager.Instance.playData.curCoin -= upCost;

            switch (upType)
            {
                case UpgradeType.������:
                    coinLevel[(int)upType] += 1;
                    // ���׷��̵� ��� .2�辿 �÷��ֱ�
                    upCost = Mathf.RoundToInt(upCost * upFactor);
                    break;
                case UpgradeType.���ݼӵ�:
                    coinLevel[(int)upType] += 1;
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
            //StartCoroutine(GameManager.instance.LackDollar());
        }
    }
}
