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
                curValueText.text = (3 * (PlayDataManager.Instance.playData.atkCoinLevels[(int)upType]+1)).ToString();

                break;
            case UpgradeType.���ݼӵ�:
                curValueText.text = (1 + .05f * PlayDataManager.Instance.playData.atkCoinLevels[(int)upType]).ToString("F2");
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
    }

    public void OnUpBtClk()
    {
        if (PlayDataManager.Instance.MainCoin >= upCost)
        {
            PlayDataManager.Instance.MainCoin -= upCost;

            PlayDataManager.Instance.playData.atkCoinLevels[(int)upType]++;
            upCost = Mathf.RoundToInt(upCost * upFactor);           
        }
        else
        {
            StartCoroutine(MainSceneManager.instance.LackCoin());
        }
    }
}
