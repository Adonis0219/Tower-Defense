using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // ���� �ǳ� GO�� ��Ƶ� ����Ʈ
    [SerializeField]
    List<GameObject> mainPanels;

    // �۾��� �ǳ��� ��Ƶ� ����Ʈ
    [SerializeField]
    List<GameObject> upPanels;

    [SerializeField]
    MAtkUpgradeButton atkUpBt;

    [SerializeField]
    public Transform atkContent;

    const string UPGRADE_NAME = "�̸�";
    const string UPGRADE_COST = "���";
    const string UPGRADE_FACTOR = "���";

    const string ATK_UPGRADE = "AtkUpgrade";

    private void Start()
    {
        AtkUpgradeSet(ATK_UPGRADE, atkUpBt, atkContent);
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
        for (int i = 0; i < mainPanels.Count; i++)
        {
            if (i == type)
            {   
                // �־��� Ÿ�԰� ���� �ǳ��� Active�� True ����
                mainPanels[i].SetActive(true);
            }
            // �������� ��� false
            else mainPanels[i].SetActive(false);
        }
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
        for (int i = 0; i < upPanels.Count; i++)
        {
            if (i == type)
            {
                upPanels[i].SetActive(true);
            }
            else upPanels[i].SetActive(false);
        }
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
            temp.upType = (MAtkUpgradeButton.UpgradeType)i;

            //PlayData.instance.goodsData.atkCoinLevels[i] = 0;
        }
    }
}
