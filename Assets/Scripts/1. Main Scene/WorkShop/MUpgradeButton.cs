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

        //costText.text = "$" + cost;
    }

    /// <summary>
    /// ������ �������ִ� �Լ�
    /// </summary>
    /// <param name="type">��, ��, ��</param>
    /// <param name="upType">�� ���׷��̵忡�� �� ��°����</param>
    protected float SetCost(int type, int upTypeIndex)
    {
        float cost = upCost;
        int level = 0;

        switch (type)
        {
            case 0:
                level = PlayDataManager.Instance.playData.atkCoinLevels[upTypeIndex];
                cost *= Mathf.Pow(upFactor, level);
                cost = Mathf.FloorToInt(cost * Sale(MainRschType.��������));
                break;
            case 1:
                level = PlayDataManager.Instance.playData.defCoinLevels[upTypeIndex];
                cost *= Mathf.Pow(upFactor, level);
                cost = Mathf.FloorToInt(cost * Sale(MainRschType.�������));
                break;
            case 2:
                level = PlayDataManager.Instance.playData.utilCoinLevels[upTypeIndex];
                cost *= Mathf.Pow(upFactor, level);
                cost = Mathf.FloorToInt(cost * Sale(MainRschType.��ƿ����));
                break;
            default:
                break;
        }

        return cost;
    }

    /// <summary>
    /// �۾��� ���� �Լ�
    /// </summary>
    /// <param name="type">������ �۾���</param>
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
