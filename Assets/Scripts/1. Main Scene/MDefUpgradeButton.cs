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
            case DefUpgradeType.ü��:
                // Mainȭ������ �������� �� ���ӸŴ����� ������ �� �����Ƿ� �������� �־��ֱ�
                curValueText.text = (5 * (PlayDataManager.Instance.playData.defCoinLevels[(int)upType]+1)).ToString();
                break;
            case DefUpgradeType.ü��ȸ��:
                curValueText.text = (.04f * PlayDataManager.Instance.playData.defCoinLevels[(int)upType]).ToString("F2") + "/sec";
                break;
            case DefUpgradeType.����:
                curValueText.text = (.5f * PlayDataManager.Instance.playData.defCoinLevels[(int)upType]).ToString("F2") + "%";
                break;
            case DefUpgradeType.������:
                curValueText.text = (.5f * PlayDataManager.Instance.playData.defCoinLevels[(int)upType]).ToString("F2");
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
    }

    public void OnUpBtClk()
    {
        PlayDataManager.Instance.MainCoin -= upCost;

        PlayDataManager.Instance.playData.defCoinLevels[(int)upType]++;

        upCost = Mathf.RoundToInt(upCost * upFactor);
    }
}
