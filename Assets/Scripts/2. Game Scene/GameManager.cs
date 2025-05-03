using System.Collections;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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


    [Header("# Enemy")]
    [SerializeField]
    public EnemyData[] enemyDatas = new EnemyData[5];

    int spawnCount;

    Coroutine[] enemyCorus = new Coroutine[4];

    [Header("##  Goods")]
    [Header("# Dollar")]
    [SerializeField]
    TextMeshProUGUI curDollarText;

    // ���� ���� �� �ʱ� �޷�
    int initDollar = 0;

    public int InitDollar
    {
        get
        {
            initDollar = 80 + 5 * PlayDataManager.Instance.playData.
                labResearchLevels[(int)ResearchType.Main, (int)MainRschType.���۴޷�];
            return initDollar;
        }
    }

    // ��� ���� �Ǵ� ���� �޷�
    float curDollar;
    public float CurDollar
    {
        get
        {
            return curDollar;
        }
        set
        {
            curDollar = value;
            curDollarText.text = "$" + Change.Num(curDollar);
        }
    }

    // ���̺� �� �޷� ���ʽ�
    float dollarWaveBonus;

    public float DollarWaveBonus
    {
        get
        {
            dollarWaveBonus = 4 * (PlayDataManager.Instance.playData.
                utilCoinLevels[(int)UtilUpgradeType.�޷����̺�] + utilDollarLevels[(int)UtilUpgradeType.�޷����̺�]);
            return dollarWaveBonus;
        }
    }

    // �޷� ���ʽ� ����
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
    public float CurCoin
    {
        get
        {
            return curCoin;
        }
        set
        {
            curCoin = value;
            curCoinText.text = Change.Num(curCoin);
        }
    }

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
            atkFreeUpChance = (.5f * (PlayDataManager.Instance.playData.
                utilCoinLevels[(int)UtilUpgradeType.������ݾ�] + utilDollarLevels[(int)UtilUpgradeType.������ݾ�]));
            return atkFreeUpChance;
        }
    }

    float defFreeUpChance;

    public float DefFreeUpChance
    {
        get
        {
            defFreeUpChance = (.5f * (PlayDataManager.Instance.playData.
                utilCoinLevels[(int)UtilUpgradeType.�������] + utilDollarLevels[(int)UtilUpgradeType.�������]));
            return defFreeUpChance;
        }
    }

    float utilFreeUpChance;

    public float UtilFreeUpChance
    {
        get
        {
            utilFreeUpChance = (.5f * (PlayDataManager.Instance.playData.
                utilCoinLevels[(int)UtilUpgradeType.������ƿ��] + utilDollarLevels[(int)UtilUpgradeType.������ƿ��]));
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

    float curTimeScale = 1.0f;
    public float CurTimeScale
    {
        get { return curTimeScale; }
        set
        {
            curTimeScale = value;

            if (curTimeScale == minTimeScale)
                timeScaleText.text = "Puase";
            else
                timeScaleText.text = "x" + curTimeScale;
            Time.timeScale = curTimeScale;
        }
    }

    float maxTimeScale = 1.5f;
    float minTimeScale = 0;

    public float MaxTimeScale
    {
        get
        {
            maxTimeScale = 1.5f + .5f * PlayDataManager.Instance.playData.
                labResearchLevels[(int)ResearchType.Main, (int)MainRschType.���Ӽӵ�];
            return maxTimeScale;
        }
    }

    [Header("# Wave Control")]

    [SerializeField]
    int wave = 1;
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

            // �� ���� ���׷��̵� ���� Ȯ�� �� ����
            if (IsChanceTrue(AtkFreeUpChance)) FreeUpgrade(PanelType.Attack);
            if (IsChanceTrue(DefFreeUpChance)) FreeUpgrade(PanelType.Defense);
            if (IsChanceTrue(UtilFreeUpChance)) FreeUpgrade(PanelType.Utility);
        }
    }

    // ���̺� ������ ��ٸ��� �ð��ΰ�?
    bool isWait;

    Coroutine coru;
    public int waitTime = 9;

    public bool IsWait
    {
        get
        {
            return isWait;
        }
        set
        {
            isWait = value;

            if (!IsWait)
                StartSpawn();
            else
                StopSpawn();
        }
    }

    [SerializeField]
    TextMeshProUGUI waveHpFactorText;
    [SerializeField]
    TextMeshProUGUI waveDmgFactorText;

    [SerializeField]
    public float waveHpFactor = 2.5f;
    [SerializeField]
    public float waveDmgFactor = 1.2f;

    public int WaveTime { get; private set; } = 30;

    [HideInInspector]
    public float gameTime;

    public int[] curMultis = new int[3] { 1, 1, 1 };

    private void Awake()
    {
        instance = this;

        //CurDollar = InitDollar;
        CurDollar = 9999999;

        InitLevelSet();
    }

    private void Start()
    {
        AudioManager.Instance.PlayBgm(SceneType.Game);

        InitReset();
    }

    /// <summary>
    /// �ʱ� ���� �� �� �������� �ʱ�ȭ ���ֱ� ���� �����ϴ� �Լ�
    /// </summary>
    void InitReset()
    {
        CurTimeScale = 1f;

        CurCoin = PlayDataManager.Instance.MainCoin;

        // ���� ���� �� �ʱ� ���ο� ���� ���� �ʱ�ȭ
        // ���� ���� �� ������� ���ΰ��� ����� ���� �̸� �ʱ�ȭ
        initCoin = CurCoin;

        // ù ������ ���� �ʱ�ȭ
        IsWait = false;

        waveDmgFactorText.text = waveDmgFactor.ToString("F2");
        waveHpFactorText.text = waveHpFactor.ToString("F2");
    }


    private void Update()
    {
        WaveTimeUp();
        GamePuaseCheck();
        TimeControl();
    }

    /// <summary>
    /// ���̺� �ð��� �������ִ� �Լ�
    /// </summary>
    void WaveTimeUp()
    {
        gameTime += Time.deltaTime;

        if (!IsWait)
        {
            if (gameTime > WaveTime)
            {
                // �� ���� (�ʰ��ð� �ս� ����)
                gameTime -= WaveTime;
                IsWait = true;
            }
        }
        else
        {
            if (gameTime > waitTime)
            {
                // �� ���� (�ʰ��ð� �ս� ����)
                gameTime -= waitTime;
                Wave++;
                IsWait = false;
            }
        }
    }

    /// <summary>
    /// ESC Ŭ�� üũ
    /// </summary>
    void GamePuaseCheck()
    {
        // �Ͻ����� �ǳ� Ȱ��ȭ ��
        if (puasePanel.activeSelf)
        {
            // esc ������
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // ����ϱ�
                OnUIBtClk((int)ButtonType.resume);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // �Ͻ����� �ǳ� Ȱ��ȭ
                OnUIBtClk((int)ButtonType.puase);
            }
        }
    }

    /// <summary>
    /// �ð� ���� �Լ�
    /// </summary>
    void TimeControl()
    {        
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
    /// ��ȯ ����
    /// </summary>
    void StartSpawn()
    {
        for (int i = 0; i < 4; i++)
        {
            enemyCorus[i] = StartCoroutine(OnSpawnEnemy(i));
        }
    }

    /// <summary>
    /// ��ȯ ����
    /// </summary>
    void StopSpawn()
    {
        for (int i = 0; i < 4; i++)
        {
            StopCoroutine(enemyCorus[i]);
        }
    }

    /// <summary>
    /// �� ���� ��ȯ���ִ� �ڷ�ƾ
    /// </summary>
    /// <param name="index">��ȯ�� ���� �ε��� ��ȣ</param>
    /// <returns></returns>
    IEnumerator OnSpawnEnemy(int index)
    {
        EnemyData data = enemyDatas[index];

        spawnCount = Mathf.FloorToInt(data.spawnRate * Mathf.Pow(wave, .23f));

        if (spawnCount > data.maxSpawnCount) spawnCount = data.maxSpawnCount;

        WaitForSeconds wait = new WaitForSeconds((float)(WaveTime + 1) / spawnCount);

        // ��� �ð��� �ƴ϶�� ��� ��ȯ
        while (!IsWait)
        {
            yield return wait;

            SpawnEnemy(index);
        }
    }

    /// <summary>
    /// ���� ��ȯ���ִ� �Լ�
    /// </summary>
    /// <param name="index">��ȯ�� ���� �ε���</param>
    void SpawnEnemy(int index)
    {
        // ���� ��ȯ
        float t = Random.Range(0f, 2 * Mathf.PI);

        Vector3 circlePos;
        circlePos.x = Mathf.Sin(t * Mathf.Rad2Deg);
        circlePos.y = Mathf.Cos(t * Mathf.Rad2Deg);
        circlePos.z = 0;

        Vector3 spawnPos = circlePos * 7 + player.transform.position;

        Transform tempEnemy = PoolManager.instance.GetPool((PoolObejectType)(index + 1)).transform;

        tempEnemy.SetParent(poolManager.GetChild(index + 1));
        // ������ ��ġ�� ����
        tempEnemy.position = spawnPos;
        //// ���� �÷��̾� ���� �ٶ󺸰�
        //tempEnemy.up = player.transform.position - spawnPos;
    }

    /// <summary>
    /// ���� ���׷��̵� ���� �Լ�
    /// </summary>
    /// <param name="type">���׷��̵� Ÿ��</param>
    void FreeUpgrade(PanelType type)
    {
        int max = PlayDataManager.Instance.playData.totalCreatCounts[(int)type];

        switch (type)
        {
            case PanelType.Attack:
                atkDollarLevels[Random.Range(0, max)]++;
                break;
            case PanelType.Defense:
                defDollarLevels[Random.Range(0, max)]++;
                break;
            case PanelType.Utility:
                utilDollarLevels[Random.Range(0, max)]++;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ���� ��ư���� ������ �� ������ �Լ�
    /// </summary>
    /// <param name="pType">��ư�� Ÿ��</param>
    [VisibleEnum(typeof(ButtonType))]
    public void OnUIBtClk(int pType)
    {
        buttonType = (ButtonType)pType;

        switch (buttonType)
        {
            case ButtonType.yes:
                AudioManager.Instance.PlaySfx(AudioManager.Sfx.OkClk);
                puasePanel.SetActive(false);
                ResultPanelSetActive(true);
                break;
            case ButtonType.exit:
                AudioManager.Instance.PlaySfx(AudioManager.Sfx.NoClk);
                puasePanel.SetActive(false);
                ResultPanelSetActive(false);
                ChangeScene(0);
                break;
            case ButtonType.resume:
                AudioManager.Instance.PlaySfx(AudioManager.Sfx.NoClk);
                puasePanel.SetActive(false);
                Time.timeScale = 1;
                break;
            case ButtonType.puase:
                AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click);
                puasePanel.SetActive(true);
                Time.timeScale = 0;
                break;
            case ButtonType.retry:
                AudioManager.Instance.PlaySfx(AudioManager.Sfx.GameStart);
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
        GameObject tempUpText = PoolManager.instance.GetPool(isDollar ? PoolObejectType.DollarText : PoolObejectType.CoinText);

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

    /// <summary>
    /// ��� ���� Ŭ�� �� �����ϴ� �Լ�
    /// </summary>
    /// <param name="isUp">�������� �������� Ȯ��</param>
    public void OnScaleUpClick(bool isUp)
    {
        if ((isUp && CurTimeScale == MaxTimeScale) || (!isUp && CurTimeScale == minTimeScale))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.NoClk);
            return;
        }

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click);
        // isUp(Plus) ��ư�̸� .5�� �����ְ�, �ƴϸ� .5�� ���ش�
        CurTimeScale += isUp ? .5f : -.5f;
        Time.timeScale = CurTimeScale;
    }

    private void OnApplicationQuit()
    {
        PlayDataManager.Instance.SaveData(curCoin, PlayDataManager.Instance.playData.mainDia);
    }
}
