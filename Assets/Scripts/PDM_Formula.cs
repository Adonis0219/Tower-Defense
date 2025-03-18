using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Atk Formula
public partial class PlayDataManager : MonoBehaviour
{
    // AtkFormula 함수만 가지고 있음

    public float DmgFormula(SceneType type)
    {
        float damage = 0;

        if (type == SceneType.Main)
        {
            damage = 3 * (PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.데미지] + 1);
        }
        else
        {
            damage = 3 * (PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.데미지]
               + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.데미지] + 1);
        }

        damage *= (1 + .02f * PlayDataManager.Instance.playData.labResearchLevels[(int)ResearchType.Attak, (int)AtkRschType.데미지]);

        return damage;
    }

    public float AtkSpdFormula(SceneType type)
    {
        float atkSpd = 0;

        if (type == SceneType.Main)
        {
            atkSpd = 1 + .05f * PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.공격속도];
        }
        else
        {
            atkSpd = 1 + .05f * (PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.공격속도]
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.공격속도]);
        }

        atkSpd *= (1 + .02f * PlayDataManager.Instance.playData.labResearchLevels[(int)ResearchType.Attak, (int)AtkRschType.공격속도]);

        return atkSpd;
    }

    public float CritFactorFormula(SceneType type)
    {
        float factor = 0;

        if (type == SceneType.Main)
        {
            factor = 1.2f + .1f * PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.치명타배율];
        }
        else
        {
            factor = 1.2f + .1f * (PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.치명타배율]
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.치명타배율]);
        }

        factor *= (1 + .03f * PlayDataManager.Instance.playData.labResearchLevels[(int)ResearchType.Attak, (int)AtkRschType.치명타배율]);

        return factor;
    }

    public float RangeFormula(SceneType type)
    {
        float range = 0;

        if (type == SceneType.Main)
        {
            range = 20 + (.5f * PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.범위]);
        }
        else
        {
            range = 20 + .5f * (PlayDataManager.Instance.playData.atkCoinLevels[(int)AtkUpgradeType.범위]
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.범위]);
        }

        range *= (1 + .02f * PlayDataManager.Instance.playData.labResearchLevels[(int)ResearchType.Attak, (int)AtkRschType.범위]);

        return range;
    }
}

public partial class PlayDataManager : MonoBehaviour
{
    // DefFormula 함수만 가지고 있음
    public float HpFormula(SceneType type)
    {
        float maxHp = 0;

        if (type == SceneType.Main)
        {
            maxHp = 5 * (PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.체력] + 1);
        }
        else
        {
            maxHp = 5 * (1 + GameManager.instance.defDollarLevels[(int)DefUpgradeType.체력]
                    + PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.체력]);
        }

        maxHp *= (1 + .03f * PlayDataManager.Instance.playData.labResearchLevels[(int)ResearchType.Defense, (int)DefRschType.체력]);

        return maxHp;
    }

    public float HpRegenFormula(SceneType type)
    {
        float regenHp = 0;

        if (type == SceneType.Main)
        {
            regenHp = .04f * PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.체력회복];
        }
        else
        {
            regenHp = .04f * (GameManager.instance.defDollarLevels[(int)DefUpgradeType.체력회복]
                + PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.체력회복]);
        }

        regenHp *= (1 + .03f * PlayDataManager.Instance.playData.labResearchLevels[(int)ResearchType.Defense, (int)DefRschType.체력회복]);

        return regenHp;
    }

    public float AbsDefFormula(SceneType type)
    {
        float absDef = 0;

        if (type == SceneType.Main)
        {
            absDef = .5f * PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.절대방어];
        }
        else
        {
            absDef = .5f * (GameManager.instance.defDollarLevels[(int)DefUpgradeType.절대방어]
                + PlayDataManager.Instance.playData.defCoinLevels[(int)DefUpgradeType.절대방어]);
        }

        absDef *= (1 + .03f * PlayDataManager.Instance.playData.labResearchLevels[(int)ResearchType.Defense, (int)DefRschType.절대방어]);

        return absDef;
    }
}

public partial class PlayDataManager : MonoBehaviour
{
    // UtilFormula 함수만 가지고 있음
}