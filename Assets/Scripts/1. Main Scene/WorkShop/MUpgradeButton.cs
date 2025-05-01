using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MUpgradeButton : MonoBehaviour
{
    protected int upCost;

    protected float upFactor;

    [SerializeField]
    protected Button bt;

    [Header("# TextObjects")]
    [SerializeField]
    protected TextMeshProUGUI upNameText;
    [SerializeField]
    protected TextMeshProUGUI curValueText;
    [SerializeField]
    protected TextMeshProUGUI costText;

    public int upType;

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

        //costText.text = "$" + cost;
    }

    /// <summary>
    /// 가격을 설정해주는 함수
    /// </summary>
    /// <param name="type">공, 방, 유</param>
    /// <param name="upType">각 업그레이드에서 몇 번째인지</param>
    protected float SetCost(int type, int upTypeIndex)
    {
        float cost = upCost;
        int level = 0;

        switch (type)
        {
            case 0:
                level = PlayDataManager.Instance.playData.atkCoinLevels[upTypeIndex];
                cost *= Mathf.Pow(upFactor, level);
                cost = Mathf.FloorToInt(cost * Sale(MainRschType.공격할인));
                break;
            case 1:
                level = PlayDataManager.Instance.playData.defCoinLevels[upTypeIndex];
                cost *= Mathf.Pow(upFactor, level);
                cost = Mathf.FloorToInt(cost * Sale(MainRschType.방어할인));
                break;
            case 2:
                level = PlayDataManager.Instance.playData.utilCoinLevels[upTypeIndex];
                cost *= Mathf.Pow(upFactor, level);
                cost = Mathf.FloorToInt(cost * Sale(MainRschType.유틸할인));
                break;
            default:
                break;
        }

        return cost;
    }

    /// <summary>
    /// 작업장 할인 함수
    /// </summary>
    /// <param name="type">할인할 작업장</param>
    /// <returns></returns>
    protected float Sale(MainRschType type)
    {
        return 1 - (.005f * PlayDataManager.Instance.playData.labResearchLevels
            [(int)ResearchType.Main, (int)type]);
    }

    public virtual void SetUpType(int upType)
    {

    }
}
