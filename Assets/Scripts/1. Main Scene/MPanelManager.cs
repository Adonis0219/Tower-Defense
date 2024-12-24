using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MPanelManager : MonoBehaviour
{
    // ���� ȭ���� ���׷��̵� �ǳ�
    public enum MainPanelType 
    {
        Battle, Workshop, Cards, Lab
    }

    MainPanelType mainPanelType;

    // ����ȭ�� �۾��� �κ��� �ǳ� Ÿ��
    public enum UpPanelType
    {
        Attack,
        Defense,
        Utility
    }

    UpPanelType upPanelType;

    [Header("## MainPanel")]
    // ���� �ǳ� GO�� ��Ƶ� ����Ʈ
    [SerializeField]
    GameObject[] mainPanels;

    // ������ �ٲ��� ���� ��ư���� �̹��� �迭
    [SerializeField]
    Image[] mainBts;

    // ���� Ȱ��ȭ�� ��ư�� �̹���
    Image activeMainBt;

    // ���� Ȱ��ȭ�� ���� �ǳ�
    GameObject activeMainPanel;

    [Header("## UpPanel")]
    // �۾��� �ǳ��� ��Ƶ� ����Ʈ
    [SerializeField]
    GameObject[] upPanels;

    [SerializeField]
    Image[] upBts;

    // ���� Ȱ��ȭ�� ��ư�� �̹���
    Image activeUpBt;

    // ���� Ȱ��ȭ�� �ǳ�
    GameObject activeUpPanel;

    [SerializeField]
    MAtkUpgradeButton atkUpBt;
    [SerializeField]
    MDefUpgradeButton defUpBt;
    [SerializeField]
    MUtilUpgradeButton utilUpBt;

    [SerializeField]
    Transform atkContent;
    [SerializeField]
    Transform defContent;
    [SerializeField]
    Transform utilContent;

    const string UPGRADE_NAME = "�̸�";
    const string UPGRADE_COST = "���";
    const string UPGRADE_FACTOR = "���";

    const string ATK_UPGRADE = "AtkUpgrade";
    const string DEF_UPGRADE = "DefUpgrade";
    const string UTIL_UPGRADE = "UtilUpgrade";

    private void Start()
    {
        activeMainPanel = mainPanels[0];
        activeMainBt = mainBts[0];

        activeUpPanel = upPanels[0];
        activeUpBt = upBts[0];

        AtkUpgradeSet(ATK_UPGRADE, atkUpBt, atkContent);
        DefUpgradeSet(DEF_UPGRADE, defUpBt, defContent);
        UtilUpgradeSet(UTIL_UPGRADE, utilUpBt, utilContent);
    }


    /// <summary>
    /// ����ȭ���� �ǳ��� ������ �� ������ �Լ�
    /// </summary>
    /// <param name="pType">���� �ǳ� Ÿ��</param>
    [VisibleEnum(typeof(MainPanelType))]
    public void PanelBtClick(int pType)
    {
        // �Ű������� MainPanelType���� �ٲ���
        mainPanelType = (MainPanelType)pType;

        // �ڽ��� �ǳ��� Ȱ��ȭ ���ִٸ� �Ʒ� �������� ����
        if (mainPanels[(int)mainPanelType].activeSelf)
            return;

        SetPanels((int)mainPanelType);
    }

    // �ǳ��� Active ���¸� Set���ִ� �Լ�
    void SetPanels(int type)
    {
        // ��ư ��� �⺻ ����
        Color btBaseColor = activeMainBt.color;

        btBaseColor.a = 0;
        activeMainBt.color = btBaseColor;

        activeMainBt = mainBts[type];

        btBaseColor.a = 1;
        activeMainBt.color = btBaseColor;

        // ��ư ���ֱ�
        activeMainPanel.SetActive(false);
        activeMainPanel = mainPanels[type];
        activeMainPanel.SetActive(true);
    }

    /// <summary>
    /// �۾��� �ǳ��� Ŭ�� ���� �� ������ �Լ�
    /// </summary>
    /// <param name="pType">�۾��� �ǳ� Ÿ��</param>
    [VisibleEnum(typeof(UpPanelType))]
    public void UpPanelBtClick(int pType)
    {
        upPanelType = (UpPanelType)pType;

        if (upPanels[(int)upPanelType].activeSelf)
            return;

        SetUpPanels((int)upPanelType);
    }

    void SetUpPanels(int type)
    {
        // ��ư ���ֱ�
        activeUpPanel.SetActive(false);
        activeUpPanel = upPanels[type];
        activeUpPanel.SetActive(true);

        //////// �� �ٲ��ֱ�
        Color baseColor = activeUpBt.color;

        baseColor.a = .15f;

        activeUpBt.color = baseColor;

        activeUpBt = upBts[type];
        baseColor = activeUpBt.color;

        baseColor.a = 1;

        upBts[type].color = baseColor;
    }

    /// <summary>
    /// ���� �ǳ��� ���׷��̵� ��ư�� �������ִ� �Լ�
    /// </summary>
    /// <param name="csv">�ҷ��� csv ����</param>
    /// <param name="atkBt">�������� ��ư Ÿ��</param>
    /// <param name="content">�������� ��ư�� ��ġ ���</param>
    public void AtkUpgradeSet(string csv, MAtkUpgradeButton atkBt, Transform content)
    {
        List<Dictionary<string, object>> datas = CSVReader.Read(csv);

        for (int i = 0; i < datas.Count; i++)
        {
            MAtkUpgradeButton temp = Instantiate(atkBt, content);

            // ������� ��ư�� �⺻ ������ SetData�� �Ѱ���
            temp.SetData(datas[i][UPGRADE_NAME].ToString(), (int)datas[i][UPGRADE_COST], (float)datas[i][UPGRADE_FACTOR]);
            // curValue�� ���� �ʱ�ȭ
            temp.upType = (AtkUpgradeType)i;
        }
    }

    public void DefUpgradeSet(string csv, MDefUpgradeButton defBt, Transform content)
    {
        List<Dictionary<string, object>> datas = CSVReader.Read(csv);

        for (int i = 0; i < datas.Count; i++)
        {
            MDefUpgradeButton temp = Instantiate(defBt, content);

            // ������� ��ư�� �⺻ ������ SetData�� �Ѱ���
            temp.SetData(datas[i][UPGRADE_NAME].ToString(), (int)datas[i][UPGRADE_COST], (float)datas[i][UPGRADE_FACTOR]);
            // curValue�� ���� �ʱ�ȭ
            temp.upType = (DefUpgradeType)i;
        }
    }

    public void UtilUpgradeSet(string csv, MUtilUpgradeButton utilBt, Transform content)
    {
        List<Dictionary<string, object>> datas = CSVReader.Read(csv);

        for (int i = 0; i < datas.Count; i++)
        {
            MUtilUpgradeButton temp = Instantiate(utilBt, content);

            // ������� ��ư�� �⺻ ������ SetData�� �Ѱ���
            temp.SetData(datas[i][UPGRADE_NAME].ToString(), (int)datas[i][UPGRADE_COST], (float)datas[i][UPGRADE_FACTOR]);
            // curValue�� ���� �ʱ�ȭ
            temp.upType = (UtilUpgradeType)i;
        }
    }
}
