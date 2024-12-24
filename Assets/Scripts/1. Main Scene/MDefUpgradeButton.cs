using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MDefUpgradeButton : MonoBehaviour
{
    [SerializeField]
    public DefUpgradeType upType;

    [SerializeField]
    int upCost;

    float upFactor;

    [SerializeField]
    Button bt;

    [Header("# TextObjects")]
    [SerializeField]
    TextMeshProUGUI upNameText;
    [SerializeField]
    TextMeshProUGUI curValueText;
    [SerializeField]
    TextMeshProUGUI costText;
   
    private void Update()
    {
        bt.interactable = PlayDataManager.Instance.MainCoin > upCost ? true : false;

        costText.text = "<sprite=12>" + upCost;

        switch (upType)
        {
            case DefUpgradeType.체력:
                // Main화면으로 시작했을 때 게임매니저에 접근할 수 없으므로 계산식으로 넣어주기
                curValueText.text = (5 * (PlayDataManager.Instance.playData.defCoinLevels[(int)upType]+1)).ToString();
                break;
            case DefUpgradeType.체력회복:
                curValueText.text = (.04f * PlayDataManager.Instance.playData.defCoinLevels[(int)upType]).ToString("F2") + "/sec";
                break;
            case DefUpgradeType.방어력:
                curValueText.text = (.5f * PlayDataManager.Instance.playData.defCoinLevels[(int)upType]).ToString("F2") + "%";
                break;
            case DefUpgradeType.절대방어:
                curValueText.text = (.5f * PlayDataManager.Instance.playData.defCoinLevels[(int)upType]).ToString("F2");
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
    }

    public void OnUpBtClk()
    {
        PlayDataManager.Instance.MainCoin -= upCost;

        PlayDataManager.Instance.playData.defCoinLevels[(int)upType]++;

        upCost = Mathf.RoundToInt(upCost * upFactor);
    }
}
