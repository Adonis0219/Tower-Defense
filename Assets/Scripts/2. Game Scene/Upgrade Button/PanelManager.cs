using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    PanelType type;

    [SerializeField]
    AtkUpgradeButton atkUpBt;
    [SerializeField]
    DefUpgradeButton defUpBt;
    [SerializeField]
    UtilUpgradeButton utilUpBt;

    [SerializeField]
    public Transform atkContent;
    [SerializeField]
    public Transform defContent;
    [SerializeField]
    public Transform utilContent;

    [SerializeField]
    RectTransform infoPanel;

    [SerializeField]
    GameObject utilLock;

    const string ATK_UPGRADE = "AtkUpgrade";
    const string DEF_UPGRADE = "DefUpgrade";
    const string UTIL_UPGRADE = "UtilUpgrade";

    const string UPGRADE_NAME = "이름";
    const string UPGRADE_COST = "비용";
    const string UPGRADE_FACTOR = "계수";

    private void Start()
    {
        activePanel = panels[0];
        activeIcon = btIcons[0];

        UpgradeBtSet(ATK_UPGRADE, PlayDataManager.Instance.playData.totalCreatCounts[(int)PanelType.Attack], atkUpBt, atkContent);
        UpgradeBtSet(DEF_UPGRADE, PlayDataManager.Instance.playData.totalCreatCounts[(int)PanelType.Defense], defUpBt, defContent);
        UpgradeBtSet(UTIL_UPGRADE, PlayDataManager.Instance.playData.totalCreatCounts[(int)PanelType.Utility], utilUpBt, utilContent);
    }

    [SerializeField]
    GameObject[] panels;    

    [VisibleEnum(typeof(PanelType))]
    public void PanelBtClick(int pType)
    {
        type = (PanelType)pType;

        // 클릭을 했을 때 자신의 판넬의 SetActive가 True이면
        if (panels[(int)type].activeSelf)
        {
            // 자신의 판넬을 꺼주기
            panels[(int)type].SetActive(false);

            GameManager.instance.gameView.position = new Vector3(0, -4, 0);
            infoPanel.anchoredPosition = new Vector3(0, -760, 0);
        }
        else
        {
            SetPanels((int)type);
            GameManager.instance.gameView.position = Vector3.zero;
            infoPanel.anchoredPosition = Vector3.zero;
        }
    }

    [SerializeField]
    Image[] btIcons;

    Image activeIcon;

    GameObject activePanel;
   
    void SetPanels(int type)
    {
        // 버튼 켜주기
        activePanel.SetActive(false);
        activePanel = panels[type];
        activePanel.SetActive(true);

        //////// 색 바꿔주기
        Color baseColor = activeIcon.color;

        baseColor.a = .15f;

        activeIcon.color = baseColor;

        activeIcon = btIcons[type];
        baseColor = activeIcon.color;

        baseColor.a = 1;

        btIcons[type].color = baseColor;
    }

    public void UpgradeBtSet(string csv, int createCount, UpgradeButton oriBt, Transform content)
    {
        List<Dictionary<string, object>> datas = CSVReader.Read(csv);

        if (oriBt == utilUpBt && createCount != 0)
        {
            utilLock.SetActive(false);
        }

        for (int i = 0; i < createCount; i++)
        {
            UpgradeButton tempBt = Instantiate(oriBt, content);

            tempBt.SetData(datas[i][UPGRADE_NAME].ToString(), (int)datas[i][UPGRADE_COST], (float)datas[i][UPGRADE_FACTOR]);

            tempBt.GetComponent<ISetUpType>().SetUpType(i);
        }
    }   
}
