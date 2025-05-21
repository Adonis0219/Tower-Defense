using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

    // 메인 화면의 업그레이드 판넬
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
    // 메인 판넬 GO를 담아둘 리스트
    [SerializeField]
    GameObject[] mainPanels;

    // 색상을 바꿔줄 메인 버튼들의 이미지 배열
    [SerializeField]
    Image[] mainBts;

    [SerializeField]
    GameObject optionPanel;

    // 현재 활성화된 버튼의 이미지
    Image activeMainBt;

    // 현재 활성화된 메인 판넬
    GameObject activeMainPanel;

    [Header("## UpPanel")]
    // 작업장 판넬을 담아둘 리스트
    [SerializeField]
    GameObject[] upPanels;

    [SerializeField]
    Image[] upBts;

    // 현재 활성화된 버튼의 이미지
    Image activeUpBt;

    // 현재 활성화된 판넬
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

    const string UPGRADE_NAME = "이름";
    const string UPGRADE_COST = "비용";
    const string UPGRADE_FACTOR = "계수";
    /// <summary>
    /// 새로 만들어줄 개수
    /// </summary>
    const string CREATE_COUNT = "해금개수";

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
        // 첫 화면의 판넬 설정
        activeMainPanel = mainPanels[0];
        activeMainBt = mainBts[0];

        // 작업장의 첫 판넬 설정
        activeUpPanel = upPanels[0];
        activeUpBt = upBts[0];

        // 각 업그레이드 버튼들 세팅해주기
        UpgradeBtSet(ATK_UPGRADE, ATK_UNLOCK, (int)PanelType.Attack, atkUpBt, atkContent);
        UpgradeBtSet(DEF_UPGRADE, DEF_UNLOCK, (int)PanelType.Defense, defUpBt, defContent);
        UpgradeBtSet(UTIL_UPGRADE, UTIL_UNLOCK, (int)PanelType.Utility, utilUpBt, utilContent);
    }

    /// <summary>
    /// 판넬의 Active 상태를 Set해주는 함수
    /// </summary>
    /// <param name="type"></param>
    void SetPanels(int type)
    {
        // 버튼 배경 기본 색상
        Color btBaseColor = activeMainBt.color;

        btBaseColor.a = 0;
        activeMainBt.color = btBaseColor;

        activeMainBt = mainBts[type];

        btBaseColor.a = 1;
        activeMainBt.color = btBaseColor;

        // 버튼 켜주기
        activeMainPanel.SetActive(false);
        activeMainPanel = mainPanels[type];
        activeMainPanel.SetActive(true);
    }

    /// <summary>
    /// 작업장 판넬의 Active 상태를 Set해주는 함수
    /// </summary>
    /// <param name="type"></param>
    void SetUpPanels(int type)
    {
        // 버튼 켜주기
        activeUpPanel.SetActive(false);
        activeUpPanel = upPanels[type];
        activeUpPanel.SetActive(true);

        //////// 색 바꿔주기
        Color baseColor = activeUpBt.color;

        baseColor.a = .15f;

        // 꺼줄 버튼 원래 색상으로
        activeUpBt.color = baseColor;

        // 활성화 버튼 바꿔주기
        activeUpBt = upBts[type];
        // 켜진 색상으로 설정
        baseColor = activeUpBt.color;

        baseColor.a = 1;

        upBts[type].color = baseColor;
    }

    /// <summary>
    /// 각 버튼들을 개수에 맞게 세팅해주는 함수 (UnlockLine에 따라)
    /// </summary>
    /// <param name="csv">참고할 csv파일명</param>
    /// <param name="unlockCsv">참고할 Unlock csv파일명</param>
    /// <param name="myType">Set할 Upgrade버튼의 타입(int형)</param>
    /// <param name="oriBt">복제할 원본 버튼</param>
    /// <param name="content">복제할 원본을 둘 위치</param>
    public void UpgradeBtSet(string csv, string unlockCsv, int myType, MUpgradeButton oriBt, Transform content)
    {
        List<Dictionary<string, object>> datas = CSVReader.Read(csv);

        /* // LineCount 활용
        //홀수면 보정
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

        // 버튼을 추가해줄 라인
        Transform btTargetLine = null;

        // 만들어줄 버튼의 개수에 따라
        for (int i = 0; i < PlayDataManager.Instance.playData.totalCreatCounts[myType]; i++)
        {
            // 개수가 0이거나 짝수일 때
            if (i % 2 == 0)
            {
                // 라인을 하나 생성해주고 그것을 타겟 라인으로 초기화
                btTargetLine = Instantiate(oriLine, content);
            }

            // 버튼을 타겟 라인에 생성해줌
            MUpgradeButton temp = Instantiate(oriBt, btTargetLine);

            // 만들어준 버튼의 기본 정보를 SetData에 넘겨줌
            temp.SetData(datas[i][UPGRADE_NAME].ToString(), (int)datas[i][UPGRADE_COST], (float)datas[i][UPGRADE_FACTOR]);
            // curValue를 위한 초기화
            // 자식이 가진 ISetUpType을 사용할 수 있도록
            temp.SetUpType(i);
        }

        // 언락 버튼을 넣어줄 라인을 하나 더 만들어줌
        btTargetLine = Instantiate(oriLine, content);

        // 언락 버튼을 만들어줌
        UnlockBtSet(unlockCsv, myType, btTargetLine);
    }

    /// <summary>
    /// Unlock Bt 클릭 시 추가해줄 버튼 셋
    /// </summary>
    /// <param name="csv">버튼을 만들 때 참고할 csv</param>
    /// <param name="unlockCsv">Unlock 버튼을 만들 때 참고할 csv</param>
    /// <param name="myType">만들어줄 버튼의 타입</param>
    /// <param name="createCount">만들 버튼의 개수</param>
    /// <param name="oriBt">만들어줄 버튼의 원본</param>
    /// <param name="content">버튼을 넣어줄 부모</param>
    public void AddUpBtSet(string csv, string unlockCsv, int myType, int createCount, MUpgradeButton oriBt, Transform content)
    {
        List<Dictionary<string, object>> datas = CSVReader.Read(csv);

        Transform btTargetLine = null;

        int totalCreateCount = PlayDataManager.Instance.playData.totalCreatCounts[myType];

        ////////// 이전 라인에 버튼이 1개
        // TotalCreateCount == 2 -> Utility 첫 업그레이드 -> 검사할 필요 X
        // 넣어줄 content의 자식들 중 마지막 라인(자식의 갯수번째 - 1)의 자식(버튼)의 개수가 1개일 때
        if (totalCreateCount > 2 && content.GetChild(content.childCount - 1).childCount == 1)
        {
            // 타겟 라인을 마지막 줄로
            btTargetLine = content.GetChild(content.childCount - 1);

            // 버튼 하나만 만들어주기
            MUpgradeButton temp = Instantiate(oriBt, btTargetLine);

            // 만들어준 버튼의 기본 정보를 SetData에 넘겨줌
            temp.SetData(datas[totalCreateCount - createCount][UPGRADE_NAME].ToString(), 
                (int)datas[totalCreateCount - createCount][UPGRADE_COST], (float)datas[totalCreateCount - createCount][UPGRADE_FACTOR]);
            // curValue를 위한 초기화
            // 자식이 가진 ISetUpType을 사용할 수 있도록
            temp.SetUpType(totalCreateCount - createCount);

            // 만들 버튼의 개수
            createCount--;
        }

        //////// 이전 라인에 버튼이 2개
        // 이전 라인에 버튼이 이미 2개 있을 때 -> 라인을 새로 만들어야 할 때
        for (int i = totalCreateCount - createCount; i < totalCreateCount; i++)
        {
            if (i % 2 == 0)
            {
                btTargetLine = Instantiate(oriLine, content);
            }

            MUpgradeButton temp = Instantiate(oriBt, btTargetLine);

            // 만들어준 버튼의 기본 정보를 SetData에 넘겨줌
            temp.SetData(datas[i][UPGRADE_NAME].ToString(), (int)datas[i][UPGRADE_COST], (float)datas[i][UPGRADE_FACTOR]);
            // curValue를 위한 초기화
            // 자식이 가진 ISetUpType을 사용할 수 있도록
            temp.SetUpType(i);
        }

        btTargetLine = Instantiate(oriLine, content);

        UnlockBtSet(unlockCsv, myType, btTargetLine);
    }

    /// <summary>
    /// 작업장에서 UnlockBt을 클릭했을 때 버튼들을 만들어주는 함수
    /// </summary>
    /// <param name="myType">업그레이드 버튼의 타입</param>
    /// <param name="createCount">만들 버튼의 개수</param>
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
    /// Unlock 버튼을 생성해주고 데이터를 넘겨주는 함수
    /// </summary>
    /// <param name="unlockCsv">참고할 csv</param>
    /// <param name="myType">Unlock 버튼의 타입</param>
    /// <param name="content">Unlock 버튼을 생성해줄 부모</param>
    public void UnlockBtSet(string unlockCsv, int myType, Transform content)
    {
        List<Dictionary<string, object>> datas = CSVReader.Read(unlockCsv);
        
        UnlockBt tempUnlockBt = Instantiate(oriUnlockBt, content);

        int unlockCount = PlayDataManager.Instance.playData.lineOpenCounts[myType];

        tempUnlockBt.SetUnlockData(datas[unlockCount][UPGRADE_NAME].ToString(), (int)datas[unlockCount][UPGRADE_COST], (int)datas[unlockCount][CREATE_COUNT], myType);
    }

    /// <summary>
    /// 작업장 안에서 각 판넬을 클릭 했을 때 실행할 함수
    /// </summary>
    /// <param name="pType">작업장 판넬 타입</param>
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
    /// 메인화면의 판넬을 눌렀을 때 실행할 함수
    /// </summary>
    /// <param name="pType">메인 판넬 타입</param>
    [VisibleEnum(typeof(MainPanelType))]
    public void PanelBtClick(int pType)
    {
        // 매개변수를 MainPanelType으로 바꿔줌
        mainPanelType = (MainPanelType)pType;

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.PanelBtClk);

        // 자신의 판넬이 활성화 돼있다면 아래 실행하지 않음
        if (mainPanels[(int)mainPanelType].activeSelf)
            return;

        SetPanels((int)mainPanelType);

        // 배틀 판넬 제외
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
        //PlayerPrefs.DeleteAll();// -> 이거 왜 안 되나요? = DontDestroy 때문
        Debug.Log( PlayerPrefs.HasKey("SaveData"));
        SceneManager.LoadScene(0);
    }
}
