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

    [SerializeField]
    float initDollar = 80;
    
    float curDollar;

    int dollarWaveBonus;

    public int DollarWaveBonus
    {
        get
        {
            dollarWaveBonus = 4 * (utilCoinLevels[(int)UtilUpgradeType.ĳ�ÿ��̺�] + utilDollarLevels[(int)UtilUpgradeType.ĳ�ÿ��̺�]);
            return dollarWaveBonus;
        }
    }

    float dollarBonusFactor;

    public float DollarBonusFactor
    {
        get
        {
            dollarBonusFactor = 1 + .01f * (utilCoinLevels[(int)UtilUpgradeType.ĳ�ú��ʽ�] + utilDollarLevels[(int)UtilUpgradeType.ĳ�ú��ʽ�]);
            return dollarBonusFactor;
        }
    }

    [Header("# Coin")]
    [SerializeField]
    TextMeshProUGUI curCoinText;

    [SerializeField]
    int curCoin;

    public float initCoin;     // �ʱ� ����
    public float earnCoin;     // �̹� ���� ȹ�� ����

    public float coinBonusFactor = 1f;

    [HideInInspector]
    public int[] atkCoinLevels;
    [HideInInspector]
    public int[] defCoinLevels;
    [HideInInspector]
    public int[] utilCoinLevels;

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

    [Header("# Wave Control")]
    public WaveData[] waveDatas;

    [SerializeField]
    Transform wavePoint;

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
    float maxGameTime = 5 * 10f;

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
    public int CurCoin
    {
        get
        {
            return curCoin;
        }
        set
        {
            curCoin = value;
            curCoinText.text = curCoin.ToString();
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

            curDollar += DollarWaveBonus;

            // �޷� ��ġ�� Uptext
            //if (waveBonusDollar != 0) GoodsFactor(waveBonusDollar, wavePoint, true);
        }
    }

    float curTimeScale = 1.0f;

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

    private void Awake()
    {
        instance = this;

        CurDollar = initDollar;

        InitLevelSet();
    }

    private void Start()
    {
        curTimeScale = 1.5f;

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

        // ������ ���ڿ� ���� ���� �ӵ� ����
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                CurTimeScale = i + 1;
            }
        }
    }

    void InitLevelSet()
    {
        PlayData playData = PlayDataManager.Instance.playData;

        // �� ���׷��̵� ������ ���� �ʱ�ȭ���ֱ�
        atkCoinLevels = new int[(int)AtkUpgradeType.Length];
        atkDollarLevels = new int[(int)AtkUpgradeType.Length];

        defCoinLevels = new int[(int)DefUpgradeType.Length];
        defDollarLevels = new int[(int)DefUpgradeType.Length];

        utilCoinLevels = new int[(int)UtilUpgradeType.Length];
        utilDollarLevels = new int[(int)UtilUpgradeType.Length];

        // PlayData���� �޾ƿ� Coin������ GameManager���� ����� �� �ֵ��� �ʱ�ȭ
        for (int i = 0; i < playData.atkCoinLevels.Length; i++)
        {
            atkCoinLevels[i] = playData.atkCoinLevels[i];
        }

        for (int i = 0; i < playData.defCoinLevels.Length; i++)
        {
            defCoinLevels[i] = playData.defCoinLevels[i];
        }

        for (int i = 0; i < playData.utilCoinLevels.Length; i++)
        {
            utilCoinLevels[i] = playData.utilCoinLevels[i];
        }
    }

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

            yield return new WaitForSeconds(3);
        }
    }

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

    void ResultPanelSetActive(bool isActive)
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

    // ���̺�
    public void GoodsFactor(float basicGoods, bool isDollar)
    {
        if (isDollar)
        {
            curDollar += basicGoods * DollarBonusFactor;
        }
        else
        {
            curCoin += (int)(basicGoods * coinBonusFactor);
        }

        CurDollar += basicGoods * (isDollar ? DollarBonusFactor : coinBonusFactor);
    }

    // ��
    public void GoodsFactor(float basicGoods, Transform enenyPos, bool isDollar)
    {

        // UpText �����Ͽ� ���� ��ġ�� �÷��ֱ�
        // �޷���� dollar�� Ǯ������Ʈ ��������
        GameObject tempUpText = PoolManager.instance.GetPool(isDollar ? PoolObejectType.dollarText : PoolObejectType.coinText);

        // ������ ���ڸ� �����ͼ� ���ڿ� ���� ����
        TextMeshPro tempUpTextMesh = tempUpText.GetComponent<TextMeshPro>();

        if (isDollar)
        {
            CurDollar += basicGoods * DollarBonusFactor;
            tempUpText.transform.position = enenyPos.position + new Vector3(0, .1f, 0);
            tempUpTextMesh.text = "$" + basicGoods * DollarBonusFactor;
        }
        else
        {
            CurCoin += (int)(basicGoods * coinBonusFactor);
            tempUpText.transform.position = enenyPos.position + new Vector3(0, .5f, 0);
            tempUpTextMesh.text = "<sprite=12>" + basicGoods * DollarBonusFactor;
        }
    }

    void ChangeScene(int sceneIndex)
    {
        PlayDataManager.Instance.SaveData(CurCoin);
        SceneManager.LoadScene(sceneIndex);
    }

    public void OnScaleUpClick(bool isUp)
    {
        // isUp(Plus) ��ư�̸� .5�� �����ְ�, �ƴϸ� .5�� ���ش�
        CurTimeScale += isUp ? .5f : -.5f;
        Time.timeScale = CurTimeScale;
    }
}
