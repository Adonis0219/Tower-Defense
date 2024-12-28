using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MPanelManager : MonoBehaviour
{
    // 메인 화면의 업그레이드 판넬
    public enum MainPanelType 
    {
        Battle, Workshop, Cards, Lab
    }

    MainPanelType mainPanelType;

    // 메인화면 작업장 부분의 판넬 타입
    public enum UpPanelType
    {
        Attack,
        Defense,
        Utility
    }

    UpPanelType upPanelType;

    [Header("## MainPanel")]
    // 메인 판넬 GO를 담아둘 리스트
    [SerializeField]
    GameObject[] mainPanels;

    // 색상을 바꿔줄 메인 버튼들의 이미지 배열
    [SerializeField]
    Image[] mainBts;

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

    const string ATK_UPGRADE = "AtkCoinUpgrade";
    const string DEF_UPGRADE = "DefUpgrade";
    const string UTIL_UPGRADE = "UtilUpgrade";

    const string ATK_UNLOCK = "AtkUnlock";
    const string DEF_UNLOCK = "DefUnlock";
    const string UTIL_UNLOCK = "UtilUnlock";

    private void Start()
    {
        activeMainPanel = mainPanels[0];
        activeMainBt = mainBts[0];

        activeUpPanel = upPanels[0];
        activeUpBt = upBts[0];

        UpgradeBtSet(ATK_UPGRADE, ATK_UNLOCK, 0, atkUpBt, atkContent);
        UpgradeBtSet(DEF_UPGRADE, DEF_UNLOCK, 1, defUpBt, defContent);
        UpgradeBtSet(UTIL_UPGRADE, UTIL_UNLOCK, 2, utilUpBt, utilContent);
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

        // 자신의 판넬이 활성화 돼있다면 아래 실행하지 않음
        if (mainPanels[(int)mainPanelType].activeSelf)
            return;

        SetPanels((int)mainPanelType);
    }

    // 판넬의 Active 상태를 Set해주는 함수
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
    /// 작업장 판넬을 클릭 했을 때 실행할 함수
    /// </summary>
    /// <param name="pType">작업장 판넬 타입</param>
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
        // 버튼 켜주기
        activeUpPanel.SetActive(false);
        activeUpPanel = upPanels[type];
        activeUpPanel.SetActive(true);

        //////// 색 바꿔주기
        Color baseColor = activeUpBt.color;

        baseColor.a = .15f;

        activeUpBt.color = baseColor;

        activeUpBt = upBts[type];
        baseColor = activeUpBt.color;

        baseColor.a = 1;

        upBts[type].color = baseColor;
    }

    /// <summary>
    /// 어택 판넬의 업그레이드 버튼을 세팅해주는 함수
    /// </summary>
    /// <param name="csv">불러올 csv 파일</param>
    /// <param name="oriBt">복제해줄 버튼 타입</param>
    /// <param name="content">복제해줄 버튼의 위치 장소</param>
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

        Transform btTargetLine = null;

        for (int i = 0; i < PlayDataManager.Instance.playData.createCounts[myType]; i++)
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
            temp.GetComponent<ISetUpType>().SetUpType(i);
        }

        btTargetLine = Instantiate(oriLine, content);

        UnlockBtSet(unlockCsv, myType, btTargetLine);
    }

    // 공격 5개, 방어 8개, 유틸 7개
    public void UnlockBtSet(string unlockCsv, int myType, Transform content)
    {
        List<Dictionary<string, object>> datas = CSVReader.Read(unlockCsv);

        UnlockBt tempUnlockBt = Instantiate(oriUnlockBt, content);

        int unlockCount = PlayDataManager.Instance.playData.openCounts[myType];

        tempUnlockBt.SetData(datas[unlockCount][UPGRADE_NAME].ToString(), (int)datas[unlockCount][UPGRADE_COST], (int)datas[unlockCount][CREATE_COUNT]);
    }
}
