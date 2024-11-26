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
        데미지,
        공격속도,
        치명타확률,
        치명타계수,
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
            case UpgradeType.데미지:
                curValueText.text = (3 * (coinLevel[(int)upType]+1)).ToString();
                break;
            case UpgradeType.공격속도:
                curValueText.text = (1 + .05f * coinLevel[(int)upType]).ToString("F2");
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

    /// <summary>
    /// 업그레이드 버튼의 기본 데이터를 Set해주는 함수
    /// </summary>
    /// <param name="name">버튼의 이름</param>
    /// <param name="cost">버튼의 비용</param>
    /// <param name="factor">버튼의 배율</param>
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
                case UpgradeType.데미지:
                    coinLevel[(int)upType] += 1;
                    // 업그레이드 비용 .2배씩 올려주기
                    upCost = Mathf.RoundToInt(upCost * upFactor);
                    break;
                case UpgradeType.공격속도:
                    coinLevel[(int)upType] += 1;
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
            //StartCoroutine(GameManager.instance.LackDollar());
        }
    }
}
