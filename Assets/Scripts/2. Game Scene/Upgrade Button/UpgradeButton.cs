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
    }

    /// <summary>
    /// �� ���׷��̵� ��ư ������ ���� ��� ǥ�� ���ڸ� �ٲ��ִ� �Լ�
    /// </summary>
    /// <param name="type"></param>
    protected void SetMultiText(int type)
    {
        int multi = GameManager.instance.curMultis[type];

        multiText.text = multi != 1 ? "x" + GameManager.instance.curMultis[type] : "";
    }

    protected int SetCost(int type)
    {
        int cost = upCost;

        if (GameManager.instance.curMultis[type] == 1)
            return cost;

        for (int i = 0; i < GameManager.instance.curMultis[type]; i++)
        {
            cost += Mathf.RoundToInt(cost * upFactor);
        }

        return cost;
    }

    public virtual void SetUpType(int upType)
    {

    }
}
