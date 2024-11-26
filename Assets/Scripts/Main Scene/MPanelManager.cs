using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // 메인 판넬 GO를 담아둘 리스트
    [SerializeField]
    List<GameObject> mainPanels;

    // 작업장 판넬을 담아둘 리스트
    [SerializeField]
    List<GameObject> upPanels;

    [SerializeField]
    MAtkUpgradeButton atkUpBt;

    [SerializeField]
    public Transform atkContent;

    const string UPGRADE_NAME = "이름";
    const string UPGRADE_COST = "비용";
    const string UPGRADE_FACTOR = "계수";

    const string ATK_UPGRADE = "AtkUpgrade";

    private void Start()
    {
        AtkUpgradeSet(ATK_UPGRADE, atkUpBt, atkContent);
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
        for (int i = 0; i < mainPanels.Count; i++)
        {
            if (i == type)
            {   
                // 넣어준 타입과 같은 판넬의 Active를 True 해줌
                mainPanels[i].SetActive(true);
            }
            // 나머지는 모두 false
            else mainPanels[i].SetActive(false);
        }
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
        for (int i = 0; i < upPanels.Count; i++)
        {
            if (i == type)
            {
                upPanels[i].SetActive(true);
            }
            else upPanels[i].SetActive(false);
        }
    }

    /// <summary>
    /// 어택 판넬의 업그레이드 버튼을 세팅해주는 함수
    /// </summary>
    /// <param name="csv">불러올 csv 파일</param>
    /// <param name="atkBt">복제해줄 버튼 타입</param>
    /// <param name="content">복제해줄 버튼의 위치 장소</param>
    public void AtkUpgradeSet(string csv, MAtkUpgradeButton atkBt, Transform content)
    {
        List<Dictionary<string, object>> datas = CSVReader.Read(csv);

        for (int i = 0; i < datas.Count; i++)
        {
            MAtkUpgradeButton temp = Instantiate(atkBt, content);

            // 만들어준 버튼의 기본 정보를 SetData에 넘겨줌
            temp.SetData(datas[i][UPGRADE_NAME].ToString(), (int)datas[i][UPGRADE_COST], (float)datas[i][UPGRADE_FACTOR]);
            // curValue를 위한 초기화
            temp.upType = (MAtkUpgradeButton.UpgradeType)i;

            //PlayData.instance.goodsData.atkCoinLevels[i] = 0;
        }
    }
}
