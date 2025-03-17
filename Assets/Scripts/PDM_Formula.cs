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
}