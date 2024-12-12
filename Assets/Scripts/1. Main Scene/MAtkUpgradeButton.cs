using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
            case UpgradeType.������:
                // Mainȭ������ �������� �� ���ӸŴ����� ������ �� �����Ƿ� �������� �־��ֱ�
                curValueText.text = (3 * (PlayDataManager.Instance.playData.atkCoinLevels[(int)upType]+1)).ToString();
                break;
            case UpgradeType.���ݼӵ�:
                curValueText.text = (1 + .05f * PlayDataManager.Instance.playData.atkCoinLevels[(int)upType]).ToString("F2");
                break;
            case UpgradeType.ġ��ŸȮ��:
                curValueText.text = (1 + PlayDataManager.Instance.playData.atkCoinLevels[(int)upType]).ToString("F2") + "%";
                break;
            case UpgradeType.ġ��Ÿ���:
                curValueText.text = "x" + (1.2f + .1f * PlayDataManager.Instance.playData.atkCoinLevels[(int)upType]).ToString("F2");
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

        PlayDataManager.Instance.playData.atkCoinLevels[(int)upType]++;

        upCost = Mathf.RoundToInt(upCost * upFactor);
    }
}
