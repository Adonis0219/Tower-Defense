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
            dollarWaveBonus = 4 * (utilCoinLevels[(int)UtilUpgradeType.캐시웨이브] + utilDollarLevels[(int)UtilUpgradeType.캐시웨이브]);
            return dollarWaveBonus;
        }
    }

    float dollarBonusFactor;

    public float DollarBonusFactor
    {
        get
        {
            dollarBonusFactor = 1 + .01f * (utilCoinLevels[(int)UtilUpgradeType.캐시보너스] + utilDollarLevels[(int)UtilUpgradeType.캐시보너스]);
            return dollarBonusFactor;
        }
    }

    [Header("# Coin")]
    [SerializeField]
    TextMeshProUGUI curCoinText;

    [SerializeField]
    int curCoin;

    public float initCoin;     // 초기 코인
    public float earnCoin;     // 이번 게임 획득 코인

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

    ////// 프로퍼티
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

            // 웨이브가 10의 배수인가 ? 1.5f : 1.2f
            waveDmgFactor *= wave % 10 == 0 ? 1.5f : 1.15f;
            waveHpFactor *= wave % 10 == 0 ? 1.5f : 1.20f;

            waveDmgFactorText.text = waveDmgFactor.ToString("F2");
            waveHpFactorText.text = waveHpFactor.ToString("F2");

            curDollar += DollarWaveBonus;

            // 달러 위치에 Uptext
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

        // 게임 시작 시 초기 코인에 현재 코인 초기화
        // 게임 종료 시 벌어들인 코인개수 계산을 위해 미리 초기화
        initCoin = CurCoin;

        StartCoroutine(SpawnEnemy());
    }

    private void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime > WaveTime)
        {
            Wave++;
            // 렉 방지 (초과시간 손실 방지)
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

        // 누르는 숫자에 따라 게임 속도 변경
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

        // 각 업그레이드 레벨들 길이 초기화해주기
        atkCoinLevels = new int[(int)AtkUpgradeType.Length];
        atkDollarLevels = new int[(int)AtkUpgradeType.Length];

        defCoinLevels = new int[(int)DefUpgradeType.Length];
        defDollarLevels = new int[(int)DefUpgradeType.Length];

        utilCoinLevels = new int[(int)UtilUpgradeType.Length];
        utilDollarLevels = new int[(int)UtilUpgradeType.Length];

        // PlayData에서 받아온 Coin레벨을 GameManager에서 사용할 수 있도록 초기화
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
        int spawnIndex;     // 소환해줄 적의 index(타입)

        // 웨이브 데이터의 길이 -> 총 웨이브 수
        while (Wave < waveDatas.Length)
        {
            // 원형 소환
            float t = Random.Range(0f, 2 * Mathf.PI);

            Vector3 circlePos;
            circlePos.x = Mathf.Sin(t * Mathf.Rad2Deg);
            circlePos.y = Mathf.Cos(t * Mathf.Rad2Deg);
            circlePos.z = 0;

            Vector3 spawnPos = circlePos * 7 + player.transform.position;

            spawnIndex = waveDatas[Wave-1].spawnIndex;

            Transform tempEnemy = PoolManager.instance.GetPool((PoolObejectType)spawnIndex).transform;

            tempEnemy.SetParent(poolManager.GetChild(spawnIndex));
            // 랜덤한 위치에 생성
            tempEnemy.position = spawnPos;
            // 적이 플레이어 방향 바라보게
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
            // 현재 웨이브가 최고기록을 넘겼는가를 검사
            PlayDataManager.Instance.BestWave = Wave;
            PlayDataManager.Instance.TotalEarnCoin += earnCoin;

            texts[0].text = "티어 1\n웨이브 " + Wave + "\n최고 웨이브 : " + PlayDataManager.Instance.BestWave;
            texts[1].text = "코인 획득량 : " + earnCoin + "<sprite=12>";
        }

        resultPanel.SetActive(isActive);
    }

    // 웨이브
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

    // 적
    public void GoodsFactor(float basicGoods, Transform enenyPos, bool isDollar)
    {

        // UpText 복사하여 죽은 위치에 올려주기
        // 달러라면 dollar의 풀오브젝트 가져오기
        GameObject tempUpText = PoolManager.instance.GetPool(isDollar ? PoolObejectType.dollarText : PoolObejectType.coinText);

        // 생성한 글자를 가져와서 글자와 색을 변경
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
        // isUp(Plus) 버튼이면 .5를 더해주고, 아니면 .5를 빼준다
        CurTimeScale += isUp ? .5f : -.5f;
        Time.timeScale = CurTimeScale;
    }
}
