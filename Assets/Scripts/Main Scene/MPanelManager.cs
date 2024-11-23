using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPanelManager : MonoBehaviour
{
    public enum PanelType 
    {
        Battle, Workshop, Cards, Lab
    }

    PanelType panelType;

    public enum UpPanelType
    {
        Attack,
        Defense,
        Utility
    }

    UpPanelType upPanelType;

    [SerializeField]
    List<GameObject> panels;

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


    [VisibleEnum(typeof(PanelType))]
    public void PanelBtClick(int pType)
    {
        panelType = (PanelType)pType;

        if (panels[(int)panelType].activeSelf)
            return;

        SetPanels((int)panelType);
    }

    [VisibleEnum(typeof(PanelType))]
    public void LockPanelBtClick(int pType)
    {
        panelType = (PanelType)pType;

        switch (panelType)
        {
            case PanelType.Workshop:

                break;
            case PanelType.Cards:
                break;
            case PanelType.Lab:
                break;
            default:
                break;
        }
    }

    [VisibleEnum(typeof(UpPanelType))]
    public void UpPanelBtClick(int pType)
    {
        upPanelType = (UpPanelType)pType;

        if (upPanels[(int)upPanelType].activeSelf)
            return;

        SetUpPanels((int)upPanelType);
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


    public void AtkUpgradeSet(string csv, MAtkUpgradeButton atkBt, Transform content)
    {
        List<Dictionary<string, object>> datas = CSVReader.Read(csv);

        for (int i = 0; i < datas.Count; i++)
        {
            MAtkUpgradeButton temp = Instantiate(atkBt, content);

            temp.SetData(datas[i][UPGRADE_NAME].ToString(), (int)datas[i][UPGRADE_COST], (float)datas[i][UPGRADE_FACTOR]);
            // curValue를 위한 초기화
            temp.upType = (MAtkUpgradeButton.UpgradeType)i;

            //PlayData.instance.goodsData.atkCoinLevels[i] = 0;
        }
    }
}
