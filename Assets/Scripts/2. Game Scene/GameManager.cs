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

    // 게임 시작 시 초기 달러
    int initDollar = 0;

    public int InitDollar
    {
        get
        {
            initDollar = 80 + 5 * PlayDataManager.Instance.playData.
                labResearchLevels[(int)ResearchType.Main, (int)MainRschType.시작달러];
            return initDollar;
        }
    }

    // 계속 갱신 되는 현재 달러
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

    // 웨이브 당 달러 보너스
    float dollarWaveBonus;

    public float DollarWaveBonus
    {
        get
        {
            dollarWaveBonus = 4 * (PlayDataManager.Instance.playData.
                utilCoinLevels[(int)UtilUpgradeType.달러웨이브] + utilDollarLevels[(int)UtilUpgradeType.달러웨이브]);
            return dollarWaveBonus;
        }
    }

    // 달러 보너스 배율
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
            atkFreeUpChance = (.5f * (PlayDataManager.Instance.playData.
                utilCoinLevels[(int)UtilUpgradeType.무료공격업] + utilDollarLevels[(int)UtilUpgradeType.무료공격업]));
            return atkFreeUpChance;
        }
    }

    float defFreeUpChance;

    public float DefFreeUpChance
    {
        get
        {
            defFreeUpChance = (.5f * (PlayDataManager.Instance.playData.
                utilCoinLevels[(int)UtilUpgradeType.무료방어업] + utilDollarLevels[(int)UtilUpgradeType.무료방어업]));
            return defFreeUpChance;
        }
    }

    float utilFreeUpChance;

    public float UtilFreeUpChance
    {
        get
        {
            utilFreeUpChance = (.5f * (PlayDataManager.Instance.playData.
                utilCoinLevels[(int)UtilUpgradeType.무료유틸업] + utilDollarLevels[(int)UtilUpgradeType.무료유틸업]));
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
                labResearchLevels[(int)ResearchType.Main, (int)MainRschType.게임속도];
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

            // 웨이브가 10의 배수인가 ? 1.5f : 1.2f
            waveDmgFactor *= wave % 10 == 0 ? 1.5f : 1.15f;
            waveHpFactor *= wave % 10 == 0 ? 1.5f : 1.20f;

            waveDmgFactorText.text = waveDmgFactor.ToString("F2");
            waveHpFactorText.text = waveHpFactor.ToString("F2");

            CurDollar += PlayDataManager.Instance.DollarWaveFormula(SceneType.Game);
            CurCoin += PlayDataManager.Instance.CoinWaveFormula(SceneType.Game);

            // 각 무료 업그레이드 찬스 확인 후 업글
            if (IsChanceTrue(AtkFreeUpChance)) FreeUpgrade(PanelType.Attack);
            if (IsChanceTrue(DefFreeUpChance)) FreeUpgrade(PanelType.Defense);
            if (IsChanceTrue(UtilFreeUpChance)) FreeUpgrade(PanelType.Utility);
        }
    }

    // 웨이브 사이의 기다리는 시간인가?
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
    /// 초기 실행 시 각 정보들을 초기화 해주기 위해 실행하는 함수
    /// </summary>
    void InitReset()
    {
        CurTimeScale = 1f;

        CurCoin = PlayDataManager.Instance.MainCoin;

        // 게임 시작 시 초기 코인에 현재 코인 초기화
        // 게임 종료 시 벌어들인 코인개수 계산을 위해 미리 초기화
        initCoin = CurCoin;

        // 첫 스폰을 위한 초기화
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
    /// 웨이브 시간을 관리해주는 함수
    /// </summary>
    void WaveTimeUp()
    {
        gameTime += Time.deltaTime;

        if (!IsWait)
        {
            if (gameTime > WaveTime)
            {
                // 렉 방지 (초과시간 손실 방지)
                gameTime -= WaveTime;
                IsWait = true;
            }
        }
        else
        {
            if (gameTime > waitTime)
            {
                // 렉 방지 (초과시간 손실 방지)
                gameTime -= waitTime;
                Wave++;
                IsWait = false;
            }
        }
    }

    /// <summary>
    /// ESC 클릭 체크
    /// </summary>
    void GamePuaseCheck()
    {
        // 일시정지 판넬 활성화 시
        if (puasePanel.activeSelf)
        {
            // esc 누르면
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // 계속하기
                OnUIBtClk((int)ButtonType.resume);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // 일시정지 판넬 활성화
                OnUIBtClk((int)ButtonType.puase);
            }
        }
    }

    /// <summary>
    /// 시간 조절 함수
    /// </summary>
    void TimeControl()
    {        
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
    /// 소환 시작
    /// </summary>
    void StartSpawn()
    {
        for (int i = 0; i < 4; i++)
        {
            enemyCorus[i] = StartCoroutine(OnSpawnEnemy(i));
        }
    }

    /// <summary>
    /// 소환 정지
    /// </summary>
    void StopSpawn()
    {
        for (int i = 0; i < 4; i++)
        {
            StopCoroutine(enemyCorus[i]);
        }
    }

    /// <summary>
    /// 각 적을 소환해주는 코루틴
    /// </summary>
    /// <param name="index">소환할 적의 인덱스 번호</param>
    /// <returns></returns>
    IEnumerator OnSpawnEnemy(int index)
    {
        EnemyData data = enemyDatas[index];

        spawnCount = Mathf.FloorToInt(data.spawnRate * Mathf.Pow(wave, .23f));

        if (spawnCount > data.maxSpawnCount) spawnCount = data.maxSpawnCount;

        WaitForSeconds wait = new WaitForSeconds((float)(WaveTime + 1) / spawnCount);

        // 대기 시간이 아니라면 계속 소환
        while (!IsWait)
        {
            yield return wait;

            SpawnEnemy(index);
        }
    }

    /// <summary>
    /// 적을 소환해주는 함수
    /// </summary>
    /// <param name="index">소환할 적의 인덱스</param>
    void SpawnEnemy(int index)
    {
        // 원형 소환
        float t = Random.Range(0f, 2 * Mathf.PI);

        Vector3 circlePos;
        circlePos.x = Mathf.Sin(t * Mathf.Rad2Deg);
        circlePos.y = Mathf.Cos(t * Mathf.Rad2Deg);
        circlePos.z = 0;

        Vector3 spawnPos = circlePos * 7 + player.transform.position;

        Transform tempEnemy = PoolManager.instance.GetPool((PoolObejectType)(index + 1)).transform;

        tempEnemy.SetParent(poolManager.GetChild(index + 1));
        // 랜덤한 위치에 생성
        tempEnemy.position = spawnPos;
        //// 적이 플레이어 방향 바라보게
        //tempEnemy.up = player.transform.position - spawnPos;
    }

    /// <summary>
    /// 무료 업그레이드 실행 함수
    /// </summary>
    /// <param name="type">업그레이드 타입</param>
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
    /// 각종 버튼들을 눌렀을 때 실행할 함수
    /// </summary>
    /// <param name="pType">버튼의 타입</param>
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
        GameObject tempUpText = PoolManager.instance.GetPool(isDollar ? PoolObejectType.DollarText : PoolObejectType.CoinText);

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

    /// <summary>
    /// 배속 조절 클릭 시 실행하는 함수
    /// </summary>
    /// <param name="isUp">증가인지 감소인지 확인</param>
    public void OnScaleUpClick(bool isUp)
    {
        if ((isUp && CurTimeScale == MaxTimeScale) || (!isUp && CurTimeScale == minTimeScale))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.NoClk);
            return;
        }

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click);
        // isUp(Plus) 버튼이면 .5를 더해주고, 아니면 .5를 빼준다
        CurTimeScale += isUp ? .5f : -.5f;
        Time.timeScale = CurTimeScale;
    }

    private void OnApplicationQuit()
    {
        PlayDataManager.Instance.SaveData(curCoin, PlayDataManager.Instance.playData.mainDia);
    }
}
