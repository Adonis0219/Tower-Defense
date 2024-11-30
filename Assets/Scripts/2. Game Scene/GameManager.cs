using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    GameObject lackDollar;

    [SerializeField]
    float initDollar = 80;
    
    float curDollar;

    public float dollarBonusFactor = 1f;

    [Header("# Coin")]
    [SerializeField]
    TextMeshProUGUI curCoinText;

    [SerializeField]
    int curCoin;

    public float initCoin;     // 초기 코인
    public float earnCoin;     // 이번 게임 획득 코인

    public float coinBonusFactor = 1f;

    public int atkCoinLevels;

    [Header("##  ManageMent")]
    [Header("# GamePuase")]
    [SerializeField]
    GameObject puasePanel;
    [SerializeField]
    public GameObject resultPanel;    

    [Header("# Wave Control")]
    public WaveData[] waveDatas;

    [SerializeField]
    Transform wavePoint;

    int wave = 1;

    [HideInInspector]
    public float waveBonusDollar = 0;

    [HideInInspector]
    public int waveTime = 20;

    [HideInInspector]
    public float gameTime;
    float maxGameTime = 5 * 10f;

    //// 프로퍼티
    public float CurDollar
    {
        get
        {
            return curDollar;
        }
        set
        {
            curDollar = value;
            curDollarText.text = "$" + curDollar;
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
            if (waveBonusDollar != 0) GoodsFactor(waveBonusDollar, wavePoint, true);
        }
    }

    private void Awake()
    {
        instance = this;

        CurDollar = initDollar;
    }

    private void Start()
    {
        PlayDataManager.Instance.LoadData();


        CurCoin = PlayDataManager.Instance.MainCoin;

        // 게임 시작 시 초기 코인에 현재 코인 초기화
        initCoin = CurCoin;

        PlayData playData = PlayDataManager.Instance.playData;
        atkCoinLevels = playData.atkCoinLevels[0];

        StartCoroutine(SpawnEnemy());
    }

    private void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime > waveTime)
        {
            Wave++;
            gameTime = 0;
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

        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                Time.timeScale = i + 1;
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        int spawnIndex;

        while (Wave < waveDatas.Length)
        {
            //spawnWFS = new WaitForSeconds(GameManager.instance.waveDatas[GameManager.instance.Wave].spawnTime);           

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

            //yield return spawnWFS;
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
            PlayDataManager.Instance.playData.totalEarnCoin += earnCoin;

            texts[0].text = "티어 1\n웨이브 " + Wave + "\n최고 웨이브 : " + PlayDataManager.Instance.BestWave;
            texts[1].text = "코인 획득량 : " + earnCoin + "<sprite=12>";
        }

        resultPanel.SetActive(isActive);
    }



    // 돈 부족 시 실행하는 코루틴
    public IEnumerator LackDollar()
    {
        lackDollar.SetActive(true);
        yield return new WaitForSeconds(.1f);
        lackDollar.SetActive(false);
    }

    // 웨이브
    public void GoodsFactor(float basicGoods, bool isDollar)
    {
        CurDollar += basicGoods * (isDollar ? dollarBonusFactor : coinBonusFactor);
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
            CurDollar += basicGoods * dollarBonusFactor;
            tempUpText.transform.position = enenyPos.position + new Vector3(0, .1f, 0);
            tempUpTextMesh.text = "$" + basicGoods * dollarBonusFactor;
        }
        else
        {
            CurCoin += (int)(basicGoods * coinBonusFactor);
            tempUpText.transform.position = enenyPos.position + new Vector3(0, .5f, 0);
            tempUpTextMesh.text = "<sprite=12>" + basicGoods * dollarBonusFactor;
        }
    }

    void ChangeScene(int sceneIndex)
    {
        PlayDataManager.Instance.SaveData(CurCoin);
        SceneManager.LoadScene(sceneIndex);
    }

    //void SaveData()
    //{
    //    PlayData.instance.SaveData(CurCoin);
    //}
}
