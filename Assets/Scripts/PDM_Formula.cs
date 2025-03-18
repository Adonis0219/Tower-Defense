using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Atk Formula
public partial class PlayDataManager : MonoBehaviour
{
    // AtkFormula �Լ��� ������ ����

    public float DmgFormula(SceneType type)
    {
        float damage = 0;

        if (type == SceneType.Main)
        {
            damage = 3 * (PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.������] + 1);
        }
        else
        {
            damage = 3 * (PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.������]
               + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.������] + 1);
        }

        damage *= (1 + .02f * PlayDataManager.Instance.playData.labResearchLevels[(int)ResearchType.Attak, (int)AtkRschType.������]);

        return damage;
    }

    public float AtkSpdFormula(SceneType type)
    {
        float atkSpd = 0;

        if (type == SceneType.Main)
        {
            atkSpd = 1 + .05f * PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.���ݼӵ�];
        }
        else
        {
            atkSpd = 1 + .05f * (PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.���ݼӵ�]
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.���ݼӵ�]);
        }

        atkSpd *= (1 + .02f * PlayDataManager.Instance.playData.labResearchLevels[(int)ResearchType.Attak, (int)AtkRschType.���ݼӵ�]);

        return atkSpd;
    }

    public float CritFactorFormula(SceneType type)
    {
        float factor = 0;

        if (type == SceneType.Main)
        {
            factor = 1.2f + .1f * PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.ġ��Ÿ����];
        }
        else
        {
            factor = 1.2f + .1f * (PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.ġ��Ÿ����]
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.ġ��Ÿ����]);
        }

        factor *= (1 + .03f * PlayDataManager.Instance.playData.labResearchLevels[(int)ResearchType.Attak, (int)AtkRschType.ġ��Ÿ����]);

        return factor;
    }

    public float RangeFormula(SceneType type)
    {
        float range = 0;

        if (type == SceneType.Main)
        {
            range = 20 + (.5f * PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.����]);
        }
        else
        {
            range = 20 + .5f * (PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.����]
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.����]);
        }

        range *= (1 + .02f * PlayDataManager.Instance.playData.labResearchLevels[(int)ResearchType.Attak, (int)AtkRschType.����]);

        return range;
    }
}

public partial class PlayDataManager : MonoBehaviour
{
    // DefFormula �Լ��� ������ ����
    public float HpFormula(SceneType type)
    {
        float maxHp = 0;

        if (type == SceneType.Main)
        {
            maxHp = 5 * (PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.ü��] + 1);
        }
        else
        {
            maxHp = 5 * (1 + GameManager.instance.defDollarLevels[(int)DefUpgradeType.ü��]
                    + PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.ü��]);
        }

        maxHp *= (1 + .03f * PlayDataManager.Instance.playData.labResearchLevels[(int)ResearchType.Defense, (int)DefRschType.ü��]);

        return maxHp;
    }

    public float HpRegenFormula(SceneType type)
    {
        float regenHp = 0;

        if (type == SceneType.Main)
        {
            regenHp = .04f * PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.ü��ȸ��];
        }
        else
        {
            regenHp = .04f * (GameManager.instance.defDollarLevels[(int)DefUpgradeType.ü��ȸ��]
                + PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.ü��ȸ��]);
        }

        regenHp *= (1 + .03f * PlayDataManager.Instance.playData.labResearchLevels[(int)ResearchType.Defense, (int)DefRschType.ü��ȸ��]);

        return regenHp;
    }

    public float AbsDefFormula(SceneType type)
    {
        float absDef = 0;

        if (type == SceneType.Main)
        {
            absDef = .5f * PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.������];
        }
        else
        {
            absDef = .5f * (GameManager.instance.defDollarLevels[(int)DefUpgradeType.������]
                + PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.������]);
        }

        absDef *= (1 + .03f * PlayDataManager.Instance.playData.labResearchLevels[(int)ResearchType.Defense, (int)DefRschType.������]);

        return absDef;
    }
}

public partial class PlayDataManager : MonoBehaviour
{
    // UtilFormula �Լ��� ������ ����
}