using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    PanelType type;

    [SerializeField]
    AtkUpgradeButton atkUpBt;
    [SerializeField]
    DefUpgradeButton defUpBt;
    [SerializeField]
    UtilUpgradeButton utilUpBt;

    [SerializeField]
    public Transform atkContent;
    [SerializeField]
    public Transform defContent;
    [SerializeField]
    public Transform utilContent;

    [SerializeField]
    RectTransform infoPanel;

    [SerializeField]
    GameObject utilLock;

    [SerializeField]
    GameObject[] panels;

    [SerializeField]
    GameObject[] curMutliBts;
    [SerializeField]
    GameObject[] multiBts;
    [SerializeField]
    GameObject[] upNameTexts;


    const string ATK_UPGRADE = "AtkUpgrade";
    const string DEF_UPGRADE = "DefUpgrade";
    const string UTIL_UPGRADE = "UtilUpgrade";

    const string UPGRADE_NAME = "이름";
    const string UPGRADE_COST = "비용";
    const string UPGRADE_FACTOR = "계수";

    private void Start()
    {
        activePanel = panels[0];
        activeIcon = btIcons[0];

        MultiBtSet();

        UpgradeBtSet(ATK_UPGRADE, PlayDataManager.Instance.playData.totalCreatCounts[(int)PanelType.Attack], atkUpBt, atkContent);
        UpgradeBtSet(DEF_UPGRADE, PlayDataManager.Instance.playData.totalCreatCounts[(int)PanelType.Defense], defUpBt, defContent);
        UpgradeBtSet(UTIL_UPGRADE, PlayDataManager.Instance.playData.totalCreatCounts[(int)PanelType.Utility], utilUpBt, utilContent);
    }



    [VisibleEnum(typeof(PanelType))]
    public void PanelBtClick(int pType)
    {
        type = (PanelType)pType;

        // 클릭을 했을 때 자신의 판넬의 SetActive가 True이면
        if (panels[(int)type].activeSelf)
        {
            // 자신의 판넬을 꺼주기
            panels[(int)type].SetActive(false);

            GameManager.instance.gameView.position = new Vector3(0, -4, 0);
            // 앵커를 기준으로 y값만 변경
            infoPanel.anchoredPosition = new Vector3(0, -610, 0);
        }
        else
        {
            SetPanels((int)type);
            GameManager.instance.gameView.position = Vector3.zero;
            infoPanel.anchoredPosition = new Vector3(0, 150,0);
        }
    }

    [SerializeField]
    Image[] btIcons;

    Image activeIcon;

    GameObject activePanel;
   
    void SetPanels(int type)
    {
        // 버튼 켜주기
        activePanel.SetActive(false);
        activePanel = panels[type];
        activePanel.SetActive(true);

        //////// 색 바꿔주기
        Color baseColor = activeIcon.color;

        baseColor.a = .15f;

        activeIcon.color = baseColor;

        activeIcon = btIcons[type];
        baseColor = activeIcon.color;

        baseColor.a = 1;

        btIcons[type].color = baseColor;
    }

    public void UpgradeBtSet(string csv, int createCount, UpgradeButton oriBt, Transform content)
    {
        List<Dictionary<string, object>> datas = CSVReader.Read(csv);

        // 유틸 버튼을 만들어주는데, 만들 버튼이 없으면(작업장에서 유틸 업그레이드가 되지 않았다면)
        if (oriBt == utilUpBt && createCount != 0)
        {
            // 게임 내 잠김 버튼 활성화
            utilLock.SetActive(false);
        }

        // 업그레이드 버튼들 만들어주기
        for (int i = 0; i < createCount; i++)
        {
            UpgradeButton tempBt = Instantiate(oriBt, content);

            tempBt.SetData(datas[i][UPGRADE_NAME].ToString(), 
                (int)datas[i][UPGRADE_COST], (float)datas[i][UPGRADE_FACTOR]);

            tempBt.SetUpType(i);
        }
    }

    /// <summary>
    /// 게임 시작 시 연구레벨에 따라 멀티 버튼들을 켜주는 함수
    /// </summary>
    public void MultiBtSet()
    {
        // 1. PDD의 연구 레벨 확인
        int level = PlayDataManager.Instance.playData.
            labResearchLevels[(int)ResearchType.Main, (int)MainRschType.승수];

        // 2. 연구 레벨이 0일 때 현재 멀티 버튼도 꺼주기
        if (level != 0)
        {
            for (int i = 0; i < 3; i++)
            {
                // 현재 배수 버튼 켜주기
                curMutliBts[i].SetActive(true);
            }

            // 3. 연구 레벨이 1일 때 현재 멀티 버튼 켜주고 레벨에 해당하는 버튼까지 켜주기
            for (int i = 0; i < level; i++)
            {
                multiBts[0].transform.GetChild(i + 1).gameObject.SetActive(true);
                multiBts[1].transform.GetChild(i + 1).gameObject.SetActive(true);
                multiBts[2].transform.GetChild(i + 1).gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// 현재 배수 버튼 눌렀을 때 실행할 함수
    /// </summary>
    /// <param name="type">몇 배수인지</param>
    public void OnCurMultiBtClk(int type)
    {
        // 이름 텍스트 꺼주고
        upNameTexts[type].SetActive(multiBts[type].activeSelf == false ? false : true);

        // 승수 버튼들 켜주기
        multiBts[type].SetActive(multiBts[type].activeSelf == false ? true : false);
    }

    const string ATK = "Atk Multi Bts";
    const string DEF = "Def Multi Bts";
    const string UTIL = "Util Multi Bts";

    /// <summary>
    /// 배수 버튼을 눌렀을 때 실행할 함수
    /// </summary>
    /// <param name="value">몇 배수인지</param>
    public void OnMultiBtClk(int value)
    {
        // 방금 클릭한 버튼의 정보를 가져와 저장
        GameObject clickedBt = EventSystem.current.currentSelectedGameObject;

        int type = 0;

        switch (clickedBt.transform.parent.name)
        {
            case ATK:
                type = 0;
                break;
            case DEF:
                type = 1;
                break;
            case UTIL:
                type = 2;
                break;
            default:
                break;
        }

        GameManager.instance.curMultis[type] = value;

        upNameTexts[type].SetActive(true);
        multiBts[type].SetActive(false);
    }
}
