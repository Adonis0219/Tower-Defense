using System.Collections;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[System.Serializable]
public class WaveData
{
    public float spawnTime;
    public int spawnIndex;
}

public class GameManager : MonoBehaviour
{
    public enum ButtonType
    {
        yes ,exit, resume, puase, retry
    }

    [HideInInspector]
    public ButtonType buttonType;

    public static GameManager instance;

    [SerializeField]
    public Player player;

    [SerializeField]
    public Transform poolManager;

    [SerializeField]
    public Transform gameView;

    [SerializeField]
    public Transform textCanvas;

    [Header("##  Goods")]
    [Header("# Dollar")]
    [SerializeField]
    TextMeshProUGUI curDollarText;

    int initDollar = 0;

    public int InitDollar
    {
        get
        {
            initDollar = 5 * PlayDataManager.Instance.playData.labResearchLevels[(int)ResearchType.Main, (int)MainRschType.���۴޷�];
            return initDollar;
        }
    }

    float curDollar;

    float dollarWaveBonus;

    public float DollarWaveBonus
    {
        get
        {
            dollarWaveBonus = 4 * (PlayDataManager.Instance.playData.utilCoinLevels[(int)UtilUpgradeType.�޷����̺�] 
                + utilDollarLevels[(int)UtilUpgradeType.�޷����̺�]);
            return dollarWaveBonus;
        }
    }

    float dollarBonusFactor;

    public float DollarBonusFactor
    {
        get
        {
            return PlayDataManager.Instance.DollarBonusFormula(SceneType.Game);
        }
    }

    [Header("# Coin")]
    [SerializeField]
    TextMeshProUGUI curCoinText;

    [SerializeField]
    float curCoin;

    public float initCoin;     // �ʱ� ����
    public float earnCoin;     // �̹� ���� ȹ�� ����

    float coinKillBonus;

    public float CoinKillBonus
    {
        get
        {
            return PlayDataManager.Instance.CoinKillBonusFormula(SceneType.Game);
        }
    }

    float coinWaveBonus;

    public float CoinWaveBonus
    {
        get
        {
            return PlayDataManager.Instance.CoinWaveFormula(SceneType.Game);
        }
    }

    [Header("# Util")]
    float atkFreeUpChance;

    public float AtkFreeUpChance
    {
        get
        {
            atkFreeUpChance = 100;
            atkFreeUpChance = (.5f * (PlayDataManager.Instance.playData.utilCoinLevels[(int)UtilUpgradeType.������ݾ�] 
                + utilDollarLevels[(int)UtilUpgradeType.������ݾ�]));
            return atkFreeUpChance;
        }
    }

    float defFreeUpChance;

    public float DefFreeUpChance
    {
        get
        {
            defFreeUpChance = (.5f * (PlayDataManager.Instance.playData.utilCoinLevels[(int)UtilUpgradeType.�������] 
                + utilDollarLevels[(int)UtilUpgradeType.�������]));
            return defFreeUpChance;
        }
    }

    float utilFreeUpChance;

    public float UtilFreeUpChance
    {
        get
        {
            utilFreeUpChance = (.5f * (PlayDataManager.Instance.playData.utilCoinLevels[(int)UtilUpgradeType.������ƿ��] 
                + utilDollarLevels[(int)UtilUpgradeType.������ƿ��]));
            return utilFreeUpChance;
        }
    }

    [HideInInspector]
    public int[] atkDollarLevels;
    [HideInInspector]
    public int[] defDollarLevels;
    [HideInInspector]
    public int[] utilDollarLevels;

    [Header("##  ManageMent")]
    [Header("# GamePuase")]
    [SerializeField]
    GameObject puasePanel;
    [SerializeField]
    public GameObject resultPanel;

    [Header("# GameTimeScale")]
    [SerializeField]
    TextMeshProUGUI timeScaleText;

    float maxTimeScale = 1.5f;

    float curTimeScale = 1.0f;

    [Header("# Wave Control")]
    public WaveData[] waveDatas;

    int wave = 1;

    [SerializeField]
    TextMeshProUGUI waveHpFactorText;
    [SerializeField]
    TextMeshProUGUI waveDmgFactorText;

    [SerializeField]
    public float waveHpFactor = 2.5f;
    [SerializeField]
    public float waveDmgFactor = 1.2f;

    public int WaveTime { get; private set; } = 10;

    [HideInInspector]
    public float gameTime;

    public int[] curMultis = new int[3] { 1, 1, 1 };

    ////// ������Ƽ
    public float CurDollar
    {
        get
        {
            return curDollar;
        }
        set
        {
            curDollar = value;
            curDollarText.text = "$" + curDollar.ToString("F0");
        }
    }
    public float CurCoin
    {
        get
        {
            return curCoin;
        }
        set
        {
            curCoin = value;
            curCoinText.text = curCoin.ToString("F0");
        }
    }

    public int Wave
    {
        get { return wave; }
        set
        {
            wave = value;

            // ���̺갡 10�� ����ΰ� ? 1.5f : 1.2f
            waveDmgFactor *= wave % 10 == 0 ? 1.5f : 1.15f;
            waveHpFactor *= wave % 10 == 0 ? 1.5f : 1.20f;

            waveDmgFactorText.text = waveDmgFactor.ToString("F2");
            waveHpFactorText.text = waveHpFactor.ToString("F2");

            CurDollar += PlayDataManager.Instance.DollarWaveFormula(SceneType.Game);
            CurCoin += PlayDataManager.Instance.CoinWaveFormula(SceneType.Game);

            GoodsKillFactor(CoinWaveBonus, false);

            if (IsChanceTrue(AtkFreeUpChance)) FreeUpgrade(PanelType.Attack);
            if (IsChanceTrue(DefFreeUpChance)) FreeUpgrade(PanelType.Defense);
            if (IsChanceTrue(UtilFreeUpChance)) FreeUpgrade(PanelType.Utility);
        }
    }

    public float CurTimeScale
    {
        get { return curTimeScale; }
        set
        {
            curTimeScale = value;
            timeScaleText.text = "x" + curTimeScale;
            Time.timeScale = curTimeScale;
        }
    }

    public float MaxTimeScale
    {
        get
        {
            maxTimeScale = 1.5f + .5f * PlayDataManager.Instance.playData.labResearchLevels[(int)ResearchType.Main, (int)MainRschType.���Ӽӵ�];
            return maxTimeScale;
        }
    }

    private void Awake()
    {
        instance = this;

        //CurDollar = InitDollar;
        CurDollar = 9999999;

        InitLevelSet();
    }

    private void Start()
    {
        CurTimeScale = 1f;

        CurCoin = PlayDataManager.Instance.MainCoin;

        // ���� ���� �� �ʱ� ���ο� ���� ���� �ʱ�ȭ
        // ���� ���� �� ������� ���ΰ��� ����� ���� �̸� �ʱ�ȭ
        initCoin = CurCoin;

        StartCoroutine(SpawnEnemy());
    }

    private void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime > WaveTime)
        {
            Wave++;
            // �� ���� (�ʰ��ð� �ս� ����)
            gameTime -= WaveTime;
        }

        if (puasePanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnPuaseClk(2);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnPuaseClk(3);
            }
        }

        // ����� �ǳ� ȣ�� Ű �Է�
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("����� �ǳ�");
        }

        // ������ ���ڿ� ���� ���� �ӵ� ����
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                CurTimeScale = i + 1;
            }
        }
    }

    /// <summary>
    /// ���Ӿ����� �Ѿ�� �� PlayDataManager���� ���� �� ��ȭ���� �迭���� ���ӸŴ������� �� �� �ֵ��� ��ȯ���ִ� �Լ�
    /// </summary>
    void InitLevelSet()
    {
        atkDollarLevels = new int[(int)AtkUpgradeType.Length];
        defDollarLevels = new int[(int)DefUpgradeType.Length];
        utilDollarLevels = new int[(int)UtilUpgradeType.Length];
    }

    /// <summary>
    /// ���� ��ȯ���ִ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnEnemy()
    {
        int spawnIndex;     // ��ȯ���� ���� index(Ÿ��)

        // ���̺� �������� ���� -> �� ���̺� ��
        while (Wave < waveDatas.Length)
        {
            // ���� ��ȯ
            float t = Random.Range(0f, 2 * Mathf.PI);

            Vector3 circlePos;
            circlePos.x = Mathf.Sin(t * Mathf.Rad2Deg);
            circlePos.y = Mathf.Cos(t * Mathf.Rad2Deg);
            circlePos.z = 0;

            Vector3 spawnPos = circlePos * 7 + player.transform.position;

            spawnIndex = waveDatas[Wave-1].spawnIndex;

            Transform tempEnemy = PoolManager.instance.GetPool((PoolObejectType)spawnIndex).transform;

            tempEnemy.SetParent(poolManager.GetChild(spawnIndex));
            // ������ ��ġ�� ����
            tempEnemy.position = spawnPos;
            // ���� �÷��̾� ���� �ٶ󺸰�
            tempEnemy.up = player.transform.position - spawnPos;

            //yield return new WaitForSeconds(1);
            yield return new WaitForSeconds(3);
        }
    }

    /// <summary>
    /// ���� ���׷��̵� ���� �Լ�
    /// </summary>
    /// <param name="type">���׷��̵� Ÿ��</param>
    void FreeUpgrade(PanelType type)
    {
        switch (type)
        {
            case PanelType.Attack:
                atkDollarLevels[Random.Range(0, PlayDataManager.Instance.playData.totalCreatCounts[(int)type])]++;
                break;
            case PanelType.Defense:
                defDollarLevels[Random.Range(0, PlayDataManager.Instance.playData.totalCreatCounts[(int)type])]++;
                break;
            case PanelType.Utility:
                utilDollarLevels[Random.Range(0, PlayDataManager.Instance.playData.totalCreatCounts[(int)type])]++;
                break;
            default:
                break;
        }

    }

    /// <summary>
    /// �Ͻ������� ������ �� ������ ��ư���� ������ �� ������ �Լ�
    /// </summary>
    /// <param name="pType">��ư�� Ÿ��</param>
    [VisibleEnum(typeof(ButtonType))]
    public void OnPuaseClk(int pType)
    {
        buttonType = (ButtonType)pType;

        switch (buttonType)
        {
            case ButtonType.yes:
                puasePanel.SetActive(false);
                ResultPanelSetActive(true);
                break;
            case ButtonType.exit:
                puasePanel.SetActive(false);
                ResultPanelSetActive(false);
                ChangeScene(0);
                break;
            case ButtonType.resume:
                puasePanel.SetActive(false);
                Time.timeScale = 1;
                break;
            case ButtonType.puase:
                puasePanel.SetActive(true);
                Time.timeScale = 0;
                break;
            case ButtonType.retry:
                resultPanel.SetActive(false);
                ChangeScene(1);
                Time.timeScale = 1;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ���â�� �����ִ� �Լ�
    /// </summary>
    /// <param name="isActive">���â�� Ȱ��ȭ ����</param>
    public void ResultPanelSetActive(bool isActive)
    {
        if (isActive)
        {
            earnCoin = CurCoin - initCoin;
            TextMeshProUGUI[] texts = resultPanel.GetComponentsInChildren<TextMeshProUGUI>();
            // ���� ���̺갡 �ְ����� �Ѱ�°��� �˻�
            PlayDataManager.Instance.BestWave = Wave;
            PlayDataManager.Instance.TotalEarnCoin += earnCoin;

            texts[0].text = "Ƽ�� 1\n���̺� " + Wave + "\n�ְ� ���̺� : " + PlayDataManager.Instance.BestWave;
            texts[1].text = "���� ȹ�淮 : " + earnCoin + "<sprite=12>";
        }

        resultPanel.SetActive(isActive);
    }

    /// <summary>
    /// �� Ȯ���� ���� �ߵ� �ƴ���
    /// </summary>
    /// <param name="chance">�ߵ� Ȯ��</param>
    /// <returns></returns>
    public bool IsChanceTrue(float chance)
    {
        float randNum = Random.Range(0f, 100f);

        // �������� �������� chance���� �۴ٸ�
        // �� ��ȯ -> Ȯ�� ����
        if (chance >= randNum)
        {
            return true;
        }
        else return false;
    }

    // ���̺�
    public void GoodsKillFactor(float basicGoods, bool isDollar)
    {
        if (isDollar)
        {
            CurDollar += basicGoods * DollarBonusFactor;
        }
        else
        {
            CurCoin += basicGoods;
        }
    }

    /// <summary>
    /// ���� �׿��� �� ����Ǵ� �޷� ���ʽ��� ų ���ʽ�
    /// </summary>
    /// <param name="basicGoods"></param>
    /// <param name="enemyPos"></param>
    /// <param name="isDollar"></param>
    public void GoodsFactor(float basicGoods, Transform enemyPos, bool isDollar)
    {
        // UpText �����Ͽ� ���� ��ġ�� �÷��ֱ�
        // �޷���� dollar�� Ǯ������Ʈ ��������
        GameObject tempUpText = PoolManager.instance.GetPool(isDollar ? PoolObejectType.dollarText : PoolObejectType.coinText);

        // ������ ���ڸ� �����ͼ� ���ڿ� ���� ����
        TextMeshPro tempUpTextMesh = tempUpText.GetComponent<TextMeshPro>();

        if (isDollar)
        {
            CurDollar += basicGoods * DollarBonusFactor;
            tempUpText.transform.position = enemyPos.position + new Vector3(0, .1f, 0);
            tempUpTextMesh.text = "$" + (basicGoods * DollarBonusFactor).ToString("F0");
        }
        else
        {
            CurCoin += basicGoods * CoinKillBonus;
            tempUpText.transform.position = enemyPos.position + new Vector3(0, .4f, 0);
            tempUpTextMesh.text = "<sprite=12>" + (basicGoods * CoinKillBonus).ToString("F0");
        }
    }

    void ChangeScene(int sceneIndex)
    {
        PlayDataManager.Instance.SaveData(CurCoin, PlayDataManager.Instance.playData.mainDia);
        SceneManager.LoadScene(sceneIndex);
    }

    public void OnScaleUpClick(bool isUp)
    {
        if (CurTimeScale == MaxTimeScale)
            return;

        // isUp(Plus) ��ư�̸� .5�� �����ְ�, �ƴϸ� .5�� ���ش�
        CurTimeScale += isUp ? .5f : -.5f;
        Time.timeScale = CurTimeScale;
    }

    private void OnApplicationQuit()
    {
        PlayDataManager.Instance.SaveData(curCoin, PlayDataManager.Instance.playData.mainDia);
    }
}
