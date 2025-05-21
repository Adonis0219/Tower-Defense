using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

    // ���� ȭ���� ���׷��̵� �ǳ�
    public enum MainPanelType 
    {
        Battle, Workshop, Cards, Lab
    }

public class MPanelManager : MonoBehaviour
{
    public static MPanelManager instance;

    MainPanelType mainPanelType;

    PanelType upPanelType;

    [Header("## MainPanel")]
    // ���� �ǳ� GO�� ��Ƶ� ����Ʈ
    [SerializeField]
    GameObject[] mainPanels;

    // ������ �ٲ��� ���� ��ư���� �̹��� �迭
    [SerializeField]
    Image[] mainBts;

    [SerializeField]
    GameObject optionPanel;

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
    Transform oriLine;
    [SerializeField]
    UnlockBt oriUnlockBt;

    [SerializeField]
    Transform atkContent;
    [SerializeField]
    Transform defContent;
    [SerializeField]
    Transform utilContent;

    const string UPGRADE_NAME = "�̸�";
    const string UPGRADE_COST = "���";
    const string UPGRADE_FACTOR = "���";
    /// <summary>
    /// ���� ������� ����
    /// </summary>
    const string CREATE_COUNT = "�رݰ���";

    const string ATK_UPGRADE = "CSV Upgrade/AtkCoinUpgrade";
    const string DEF_UPGRADE = "CSV Upgrade/DefCoinUpgrade";
    const string UTIL_UPGRADE = "CSV Upgrade/UtilUpgrade";

    const string ATK_UNLOCK = "CSV Upgrade/AtkUnlock";
    const string DEF_UNLOCK = "CSV Upgrade/DefUnlock";
    const string UTIL_UNLOCK = "CSV Upgrade/UtilUnlock";

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // ù ȭ���� �ǳ� ����
        activeMainPanel = mainPanels[0];
        activeMainBt = mainBts[0];

        // �۾����� ù �ǳ� ����
        activeUpPanel = upPanels[0];
        activeUpBt = upBts[0];

        // �� ���׷��̵� ��ư�� �������ֱ�
        UpgradeBtSet(ATK_UPGRADE, ATK_UNLOCK, (int)PanelType.Attack, atkUpBt, atkContent);
        UpgradeBtSet(DEF_UPGRADE, DEF_UNLOCK, (int)PanelType.Defense, defUpBt, defContent);
        UpgradeBtSet(UTIL_UPGRADE, UTIL_UNLOCK, (int)PanelType.Utility, utilUpBt, utilContent);
    }

    /// <summary>
    /// �ǳ��� Active ���¸� Set���ִ� �Լ�
    /// </summary>
    /// <param name="type"></param>
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
    /// �۾��� �ǳ��� Active ���¸� Set���ִ� �Լ�
    /// </summary>
    /// <param name="type"></param>
    void SetUpPanels(int type)
    {
        // ��ư ���ֱ�
        activeUpPanel.SetActive(false);
        activeUpPanel = upPanels[type];
        activeUpPanel.SetActive(true);

        //////// �� �ٲ��ֱ�
        Color baseColor = activeUpBt.color;

        baseColor.a = .15f;

        // ���� ��ư ���� ��������
        activeUpBt.color = baseColor;

        // Ȱ��ȭ ��ư �ٲ��ֱ�
        activeUpBt = upBts[type];
        // ���� �������� ����
        baseColor = activeUpBt.color;

        baseColor.a = 1;

        upBts[type].color = baseColor;
    }

    /// <summary>
    /// �� ��ư���� ������ �°� �������ִ� �Լ� (UnlockLine�� ����)
    /// </summary>
    /// <param name="csv">������ csv���ϸ�</param>
    /// <param name="unlockCsv">������ Unlock csv���ϸ�</param>
    /// <param name="myType">Set�� Upgrade��ư�� Ÿ��(int��)</param>
    /// <param name="oriBt">������ ���� ��ư</param>
    /// <param name="content">������ ������ �� ��ġ</param>
    public void UpgradeBtSet(string csv, string unlockCsv, int myType, MUpgradeButton oriBt, Transform content)
    {
        List<Dictionary<string, object>> datas = CSVReader.Read(csv);

        /* // LineCount Ȱ��
        //Ȧ���� ����
        int lineCount = datas.Count % 2 == 1 ? datas.Count / 2 + 1 : datas.Count;

        for (int i = 1; i <= lineCount; i++)
        {
            GameObject tempLine = new GameObject();
            tempLine.transform.SetParent(content);

            int createCount = datas.Count - 2 * i > 1 ? 2 : 1;

            for (int j = 0; j < createCount; j++)
            {
                MAtkUpgradeButton temp = Instantiate(atkBt, tempLine.transform);
            }
        }*/

        // ��ư�� �߰����� ����
        Transform btTargetLine = null;

        // ������� ��ư�� ������ ����
        for (int i = 0; i < PlayDataManager.Instance.playData.totalCreatCounts[myType]; i++)
        {
            // ������ 0�̰ų� ¦���� ��
            if (i % 2 == 0)
            {
                // ������ �ϳ� �������ְ� �װ��� Ÿ�� �������� �ʱ�ȭ
                btTargetLine = Instantiate(oriLine, content);
            }

            // ��ư�� Ÿ�� ���ο� ��������
            MUpgradeButton temp = Instantiate(oriBt, btTargetLine);

            // ������� ��ư�� �⺻ ������ SetData�� �Ѱ���
            temp.SetData(datas[i][UPGRADE_NAME].ToString(), (int)datas[i][UPGRADE_COST], (float)datas[i][UPGRADE_FACTOR]);
            // curValue�� ���� �ʱ�ȭ
            // �ڽ��� ���� ISetUpType�� ����� �� �ֵ���
            temp.SetUpType(i);
        }

        // ��� ��ư�� �־��� ������ �ϳ� �� �������
        btTargetLine = Instantiate(oriLine, content);

        // ��� ��ư�� �������
        UnlockBtSet(unlockCsv, myType, btTargetLine);
    }

    /// <summary>
    /// Unlock Bt Ŭ�� �� �߰����� ��ư ��
    /// </summary>
    /// <param name="csv">��ư�� ���� �� ������ csv</param>
    /// <param name="unlockCsv">Unlock ��ư�� ���� �� ������ csv</param>
    /// <param name="myType">������� ��ư�� Ÿ��</param>
    /// <param name="createCount">���� ��ư�� ����</param>
    /// <param name="oriBt">������� ��ư�� ����</param>
    /// <param name="content">��ư�� �־��� �θ�</param>
    public void AddUpBtSet(string csv, string unlockCsv, int myType, int createCount, MUpgradeButton oriBt, Transform content)
    {
        List<Dictionary<string, object>> datas = CSVReader.Read(csv);

        Transform btTargetLine = null;

        int totalCreateCount = PlayDataManager.Instance.playData.totalCreatCounts[myType];

        ////////// ���� ���ο� ��ư�� 1��
        // TotalCreateCount == 2 -> Utility ù ���׷��̵� -> �˻��� �ʿ� X
        // �־��� content�� �ڽĵ� �� ������ ����(�ڽ��� ������° - 1)�� �ڽ�(��ư)�� ������ 1���� ��
        if (totalCreateCount > 2 && content.GetChild(content.childCount - 1).childCount == 1)
        {
            // Ÿ�� ������ ������ �ٷ�
            btTargetLine = content.GetChild(content.childCount - 1);

            // ��ư �ϳ��� ������ֱ�
            MUpgradeButton temp = Instantiate(oriBt, btTargetLine);

            // ������� ��ư�� �⺻ ������ SetData�� �Ѱ���
            temp.SetData(datas[totalCreateCount - createCount][UPGRADE_NAME].ToString(), 
                (int)datas[totalCreateCount - createCount][UPGRADE_COST], (float)datas[totalCreateCount - createCount][UPGRADE_FACTOR]);
            // curValue�� ���� �ʱ�ȭ
            // �ڽ��� ���� ISetUpType�� ����� �� �ֵ���
            temp.SetUpType(totalCreateCount - createCount);

            // ���� ��ư�� ����
            createCount--;
        }

        //////// ���� ���ο� ��ư�� 2��
        // ���� ���ο� ��ư�� �̹� 2�� ���� �� -> ������ ���� ������ �� ��
        for (int i = totalCreateCount - createCount; i < totalCreateCount; i++)
        {
            if (i % 2 == 0)
            {
                btTargetLine = Instantiate(oriLine, content);
            }

            MUpgradeButton temp = Instantiate(oriBt, btTargetLine);

            // ������� ��ư�� �⺻ ������ SetData�� �Ѱ���
            temp.SetData(datas[i][UPGRADE_NAME].ToString(), (int)datas[i][UPGRADE_COST], (float)datas[i][UPGRADE_FACTOR]);
            // curValue�� ���� �ʱ�ȭ
            // �ڽ��� ���� ISetUpType�� ����� �� �ֵ���
            temp.SetUpType(i);
        }

        btTargetLine = Instantiate(oriLine, content);

        UnlockBtSet(unlockCsv, myType, btTargetLine);
    }

    /// <summary>
    /// �۾��忡�� UnlockBt�� Ŭ������ �� ��ư���� ������ִ� �Լ�
    /// </summary>
    /// <param name="myType">���׷��̵� ��ư�� Ÿ��</param>
    /// <param name="createCount">���� ��ư�� ����</param>
    public void OnUnlockClickCreate(int myType, int createCount)
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.UpBtClk);

        switch (myType)
        {
            case (int)PanelType.Attack:
                AddUpBtSet(ATK_UPGRADE, ATK_UNLOCK, 
                    (int)PanelType.Attack, createCount, atkUpBt, atkContent);
                break;
            case (int)PanelType.Defense:
                AddUpBtSet(DEF_UPGRADE, DEF_UNLOCK, 
                    (int)PanelType.Defense, createCount, defUpBt, defContent);
                break;
            case (int)PanelType.Utility:
                AddUpBtSet(UTIL_UPGRADE, UTIL_UNLOCK, 
                    (int)PanelType.Utility, createCount, utilUpBt, utilContent);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Unlock ��ư�� �������ְ� �����͸� �Ѱ��ִ� �Լ�
    /// </summary>
    /// <param name="unlockCsv">������ csv</param>
    /// <param name="myType">Unlock ��ư�� Ÿ��</param>
    /// <param name="content">Unlock ��ư�� �������� �θ�</param>
    public void UnlockBtSet(string unlockCsv, int myType, Transform content)
    {
        List<Dictionary<string, object>> datas = CSVReader.Read(unlockCsv);
        
        UnlockBt tempUnlockBt = Instantiate(oriUnlockBt, content);

        int unlockCount = PlayDataManager.Instance.playData.lineOpenCounts[myType];

        tempUnlockBt.SetUnlockData(datas[unlockCount][UPGRADE_NAME].ToString(), (int)datas[unlockCount][UPGRADE_COST], (int)datas[unlockCount][CREATE_COUNT], myType);
    }

    /// <summary>
    /// �۾��� �ȿ��� �� �ǳ��� Ŭ�� ���� �� ������ �Լ�
    /// </summary>
    /// <param name="pType">�۾��� �ǳ� Ÿ��</param>
    [VisibleEnum(typeof(PanelType))]
    public void UpPanelBtClick(int pType)
    {
        upPanelType = (PanelType)pType;

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.PanelBtClk);

        if (upPanels[(int)upPanelType].activeSelf)
            return;

        SetUpPanels((int)upPanelType);
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

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.PanelBtClk);

        // �ڽ��� �ǳ��� Ȱ��ȭ ���ִٸ� �Ʒ� �������� ����
        if (mainPanels[(int)mainPanelType].activeSelf)
            return;

        SetPanels((int)mainPanelType);

        // ��Ʋ �ǳ� ����
        if (pType == 0) return;

        if ((PlayDataManager.Instance.playData.alreadyTuto >> pType - 1) % 2 == 0)
        {
            TutorialManager.instance.OpenTutoPanel(pType - 1);
        }
    }

    public void OnOptionClk(bool isOpen)
    {
        AudioManager.Instance.PlaySfx(isOpen ? AudioManager.Sfx.Click : AudioManager.Sfx.NoClk);

        optionPanel.SetActive(isOpen);
    }

    public void OnResetClk()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click);

        PlayDataManager.Instance.playData = new PlayData();
        //PlayerPrefs.DeleteAll();// -> �̰� �� �� �ǳ���? = DontDestroy ����
        Debug.Log( PlayerPrefs.HasKey("SaveData"));
        SceneManager.LoadScene(0);
    }
}
