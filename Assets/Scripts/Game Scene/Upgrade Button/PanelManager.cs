using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public enum PanelType
    {
        Attack,
        Defense,
        Utility
    }    

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

    const string ATK_UPGRADE = "AtkUpgrade";
    const string DEF_UPGRADE = "DefUpgrade";
    const string UTIL_UPGRADE = "UtilUpgrade";

    const string UPGRADE_NAME = "이름";
    const string UPGRADE_COST = "비용";
    const string UPGRADE_FACTOR = "계수";

    private void Start()
    {
        AtkUpgradeSet(ATK_UPGRADE, atkUpBt, atkContent);
        DefUpgradeSet(DEF_UPGRADE, defUpBt, defContent);
        UtilUpgradeSet(UTIL_UPGRADE, utilUpBt, utilContent);
    }

    [SerializeField]
    List<GameObject> panels;    

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
            infoPanel.anchoredPosition = new Vector3(0, -760, 0);
        }
        else
        {
            SetPanels((int)type);
            GameManager.instance.gameView.position = Vector3.zero;
            infoPanel.anchoredPosition = Vector3.zero;
        }
    }

    public void AtkUpgradeSet(string csv, AtkUpgradeButton atkBt, Transform content)
    {
        List<Dictionary<string, object>> datas = CSVReader.Read(csv);

        for (int i = 0; i < datas.Count; i++)
        {
            AtkUpgradeButton temp = Instantiate(atkBt, content);

            temp.SetData(datas[i][UPGRADE_NAME].ToString(), (int)datas[i][UPGRADE_COST], (float)datas[i][UPGRADE_FACTOR]);
            // curValue를 위한 초기화
            temp.upType = (AtkUpgradeButton.UpgradeType)i;
        }
    }

    public void DefUpgradeSet(string csv, DefUpgradeButton defBt, Transform content)
    {
        List<Dictionary<string, object>> datas = CSVReader.Read(csv);

        for (int i = 0; i < datas.Count; i++)
        {
            DefUpgradeButton temp = Instantiate(defBt, content);

            temp.SetData(datas[i][UPGRADE_NAME].ToString(), (int)datas[i][UPGRADE_COST], (float)datas[i][UPGRADE_FACTOR]);
            // curValue를 위한 초기화
            temp.upType = (DefUpgradeButton.UpgradeType)i;
        }
    }

    public void UtilUpgradeSet(string csv, UtilUpgradeButton utilBt, Transform content)
    {
        List<Dictionary<string, object>> datas = CSVReader.Read(csv);

        for (int i = 0; i < datas.Count; i++)
        {
            UtilUpgradeButton temp = Instantiate(utilBt, content);

            temp.SetData(datas[i][UPGRADE_NAME].ToString(), (int)datas[i][UPGRADE_COST], (float)datas[i][UPGRADE_FACTOR]);
            // curValue를 위한 초기화
            temp.upType = (UtilUpgradeButton.UpgradeType)i;
        }
    }

    void SetPanels(int type)
    {
        for (int i = 0; i < panels.Count; i++)
        {
            if (i == type)
            {
                panels[i].SetActive(true);
            }
            else panels[i].SetActive(false);
        }
    }
}
