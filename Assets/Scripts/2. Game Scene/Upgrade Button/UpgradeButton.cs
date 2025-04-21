using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
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
    [SerializeField]
    protected TextMeshProUGUI multiText;

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

        costText.text = "$" + cost;
    }

    /// <summary>
    /// 각 업그레이드 버튼 오른쪽 위의 배수 표시 글자를 바꿔주는 함수
    /// </summary>
    /// <param name="type"></param>
    protected void SetMultiText(int type)
    {
        int multi = GameManager.instance.curMultis[type];

        multiText.text = multi != 1 ? "x" + GameManager.instance.curMultis[type] : "";
    }

    protected int SetCost(int type)
    {
        // 자체 비용은 바꾸면 안 되기 때문에
        // 반환용 비용 선언
        int cost = upCost;
        int multi = GameManager.instance.curMultis[type];

        for (int i = 0; i < multi; i++)
        {
            cost += Mathf.RoundToInt(cost * upFactor);
        }

        return cost;
    }

    public virtual void SetUpType(int upType)
    {

    }
}
