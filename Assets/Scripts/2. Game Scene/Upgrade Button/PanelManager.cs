using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
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

    [SerializeField]
    GameObject[] panels;

    [SerializeField]
    GameObject[] curMutliBts;
    [SerializeField]
    GameObject[] multiBts;
    [SerializeField]
    GameObject[] upNameTexts;


    const string ATK_UPGRADE = "AtkUpgrade";
    const string DEF_UPGRADE = "DefUpgrade";
    const string UTIL_UPGRADE = "UtilUpgrade";

    const string UPGRADE_NAME = "�̸�";
    const string UPGRADE_COST = "���";
    const string UPGRADE_FACTOR = "���";

    private void Start()
    {
        activePanel = panels[0];
        activeIcon = btIcons[0];

        MultiBtSet();

        UpgradeBtSet(ATK_UPGRADE, PlayDataManager.Instance.playData.totalCreatCounts[(int)PanelType.Attack], atkUpBt, atkContent);
        UpgradeBtSet(DEF_UPGRADE, PlayDataManager.Instance.playData.totalCreatCounts[(int)PanelType.Defense], defUpBt, defContent);
        UpgradeBtSet(UTIL_UPGRADE, PlayDataManager.Instance.playData.totalCreatCounts[(int)PanelType.Utility], utilUpBt, utilContent);
    }



    [VisibleEnum(typeof(PanelType))]
    public void PanelBtClick(int pType)
    {
        type = (PanelType)pType;

        // Ŭ���� ���� �� �ڽ��� �ǳ��� SetActive�� True�̸�
        if (panels[(int)type].activeSelf)
        {
            // �ڽ��� �ǳ��� ���ֱ�
            panels[(int)type].SetActive(false);

            GameManager.instance.gameView.position = new Vector3(0, -4, 0);
            // ��Ŀ�� �������� y���� ����
            infoPanel.anchoredPosition = new Vector3(0, -610, 0);
        }
        else
        {
            SetPanels((int)type);
            GameManager.instance.gameView.position = Vector3.zero;
            infoPanel.anchoredPosition = new Vector3(0, 150,0);
        }
    }

    [SerializeField]
    Image[] btIcons;

    Image activeIcon;

    GameObject activePanel;
   
    void SetPanels(int type)
    {
        // ��ư ���ֱ�
        activePanel.SetActive(false);
        activePanel = panels[type];
        activePanel.SetActive(true);

        //////// �� �ٲ��ֱ�
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

        // ��ƿ ��ư�� ������ִµ�, ���� ��ư�� ������(�۾��忡�� ��ƿ ���׷��̵尡 ���� �ʾҴٸ�)
        if (oriBt == utilUpBt && createCount != 0)
        {
            // ���� �� ��� ��ư Ȱ��ȭ
            utilLock.SetActive(false);
        }

        // ���׷��̵� ��ư�� ������ֱ�
        for (int i = 0; i < createCount; i++)
        {
            UpgradeButton tempBt = Instantiate(oriBt, content);

            tempBt.SetData(datas[i][UPGRADE_NAME].ToString(), 
                (int)datas[i][UPGRADE_COST], (float)datas[i][UPGRADE_FACTOR]);

            tempBt.SetUpType(i);
        }
    }

    /// <summary>
    /// ���� ���� �� ���������� ���� ��Ƽ ��ư���� ���ִ� �Լ�
    /// </summary>
    public void MultiBtSet()
    {
        // 1. PDD�� ���� ���� Ȯ��
        int level = PlayDataManager.Instance.playData.
            labResearchLevels[(int)ResearchType.Main, (int)MainRschType.�¼�];

        // 2. ���� ������ 0�� �� ���� ��Ƽ ��ư�� ���ֱ�
        if (level != 0)
        {
            for (int i = 0; i < 3; i++)
            {
                // ���� ��� ��ư ���ֱ�
                curMutliBts[i].SetActive(true);
            }

            // 3. ���� ������ 1�� �� ���� ��Ƽ ��ư ���ְ� ������ �ش��ϴ� ��ư���� ���ֱ�
            for (int i = 0; i < level; i++)
            {
                multiBts[0].transform.GetChild(i + 1).gameObject.SetActive(true);
                multiBts[1].transform.GetChild(i + 1).gameObject.SetActive(true);
                multiBts[2].transform.GetChild(i + 1).gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// ���� ��� ��ư ������ �� ������ �Լ�
    /// </summary>
    /// <param name="type">�� �������</param>
    public void OnCurMultiBtClk(int type)
    {
        // �̸� �ؽ�Ʈ ���ְ�
        upNameTexts[type].SetActive(multiBts[type].activeSelf == false ? false : true);

        // �¼� ��ư�� ���ֱ�
        multiBts[type].SetActive(multiBts[type].activeSelf == false ? true : false);
    }

    const string ATK = "Atk Multi Bts";
    const string DEF = "Def Multi Bts";
    const string UTIL = "Util Multi Bts";

    /// <summary>
    /// ��� ��ư�� ������ �� ������ �Լ�
    /// </summary>
    /// <param name="value">�� �������</param>
    public void OnMultiBtClk(int value)
    {
        // ��� Ŭ���� ��ư�� ������ ������ ����
        GameObject clickedBt = EventSystem.current.currentSelectedGameObject;

        int type = 0;

        switch (clickedBt.transform.parent.name)
        {
            case ATK:
                type = 0;
                break;
            case DEF:
                type = 1;
                break;
            case UTIL:
                type = 2;
                break;
            default:
                break;
        }

        GameManager.instance.curMultis[type] = value;

        upNameTexts[type].SetActive(true);
        multiBts[type].SetActive(false);
    }
}
