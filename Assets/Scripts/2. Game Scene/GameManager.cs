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
            initDollar = 5 * PlayDataManager.Instance.playData.labResearchLevels[(int)ResearchType.Main, (int)MainRschType.시작달러];
            return initDollar;
        }
    }

    float curDollar;

    float dollarWaveBonus;

    public float DollarWaveBonus
    {
        get
        {
            dollarWaveBonus = 4 * (PlayDataManager.Instance.playData.utilCoinLevels[(int)UtilUpgradeType.달러웨이브] 
                + utilDollarLevels[(int)UtilUpgradeType.달러웨이브]);
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

    public float initCoin;     // 초기 코인
    public float earnCoin;     // 이번 게임 획득 코인

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
            atkFreeUpChance = (.5f * (PlayDataManager.Instance.playData.utilCoinLevels[(int)UtilUpgradeType.무료공격업] 
                + utilDollarLevels[(int)UtilUpgradeType.무료공격업]));
            return atkFreeUpChance;
        }
    }

    float defFreeUpChance;

    public float DefFreeUpChance
    {
        get
        {
            defFreeUpChance = (.5f * (PlayDataManager.Instance.playData.utilCoinLevels[(int)UtilUpgradeType.무료방어업] 
                + utilDollarLevels[(int)UtilUpgradeType.무료방어업]));
            return defFreeUpChance;
        }
    }

    float utilFreeUpChance;

    public float UtilFreeUpChance
    {
        get
        {
            utilFreeUpChance = (.5f * (PlayDataManager.Instance.playData.utilCoinLevels[(int)UtilUpgradeType.무료유틸업] 
                + utilDollarLevels[(int)UtilUpgradeType.무료유틸업]));
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

            // 웨이브가 10의 배수인가 ? 1.5f : 1.2f
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
            maxTimeScale = 1.5f + .5f * PlayDataManager.Instance.playData.labResearchLevels[(int)ResearchType.Main, (int)MainRschType.게임속도];
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

        // 디버그 판넬 호출 키 입력
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("디버그 판넬");
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

    /// <summary>
    /// 게임씬으로 넘어올 때 PlayDataManager에게 받은 각 재화레벨 배열들을 게임매니저에서 쓸 수 있도록 변환해주는 함수
    /// </summary>
    void InitLevelSet()
    {
        atkDollarLevels = new int[(int)AtkUpgradeType.Length];
        defDollarLevels = new int[(int)DefUpgradeType.Length];
        utilDollarLevels = new int[(int)UtilUpgradeType.Length];
    }

    /// <summary>
    /// 적을 소환해주는 코루틴
    /// </summary>
    /// <returns></returns>
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

            //yield return new WaitForSeconds(1);
            yield return new WaitForSeconds(3);
        }
    }

    /// <summary>
    /// 무료 업그레이드 실행 함수
    /// </summary>
    /// <param name="type">업그레이드 타입</param>
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
    /// 일시정지를 눌렀을 때 나오는 버튼들을 눌렀을 때 실행할 함수
    /// </summary>
    /// <param name="pType">버튼의 타입</param>
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
    /// 결과창을 보여주는 함수
    /// </summary>
    /// <param name="isActive">결과창의 활성화 여부</param>
    public void ResultPanelSetActive(bool isActive)
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

    /// <summary>
    /// 각 확률에 따라 발동 됐는지
    /// </summary>
    /// <param name="chance">발동 확률</param>
    /// <returns></returns>
    public bool IsChanceTrue(float chance)
    {
        float randNum = Random.Range(0f, 100f);

        // 랜덤으로 돌린값이 chance보다 작다면
        // 참 반환 -> 확률 터짐
        if (chance >= randNum)
        {
            return true;
        }
        else return false;
    }

    // 웨이브
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
    /// 적을 죽였을 때 적용되는 달러 보너스와 킬 보너스
    /// </summary>
    /// <param name="basicGoods"></param>
    /// <param name="enemyPos"></param>
    /// <param name="isDollar"></param>
    public void GoodsFactor(float basicGoods, Transform enemyPos, bool isDollar)
    {
        // UpText 복사하여 죽은 위치에 올려주기
        // 달러라면 dollar의 풀오브젝트 가져오기
        GameObject tempUpText = PoolManager.instance.GetPool(isDollar ? PoolObejectType.dollarText : PoolObejectType.coinText);

        // 생성한 글자를 가져와서 글자와 색을 변경
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

        // isUp(Plus) 버튼이면 .5를 더해주고, 아니면 .5를 빼준다
        CurTimeScale += isUp ? .5f : -.5f;
        Time.timeScale = CurTimeScale;
    }

    private void OnApplicationQuit()
    {
        PlayDataManager.Instance.SaveData(curCoin, PlayDataManager.Instance.playData.mainDia);
    }
}
