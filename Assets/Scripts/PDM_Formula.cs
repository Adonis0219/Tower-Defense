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
            damage = 3 * (playData.atkCoinLevels[(int)AtkUpgradeType.데미지] + 1);
        }
        else
        {
            damage = 3 * (playData.atkCoinLevels[(int)AtkUpgradeType.데미지]
               + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.데미지] + 1);
        }

        // 연구 적용
        damage *= (1 + .02f * playData.labResearchLevels[(int)ResearchType.Attak, (int)AtkRschType.데미지]);

        // 카드 적용
        if (CheckCard(CardID.대미지))
        {
            CardData myCard = MainSceneManager.instance.cardDatas[(int)CardID.대미지];
            damage *= myCard.value[myCard.curLv];
        }

        return damage;
    }

    public float AtkSpdFormula(SceneType type)
    {
        float atkSpd = 0;

        if (type == SceneType.Main)
        {
            atkSpd = 1 + .05f * playData.atkCoinLevels[(int)AtkUpgradeType.공격속도];
        }
        else
        {
            atkSpd = 1 + .05f * (playData.atkCoinLevels[(int)AtkUpgradeType.공격속도]
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.공격속도]);
        }

        // 연구 적용
        atkSpd *= (1 + .02f * playData.labResearchLevels[(int)ResearchType.Attak, (int)AtkRschType.공격속도]);

        // 카드 적용
        if (CheckCard(CardID.공격속도))
        {
            CardData myCard = MainSceneManager.instance.cardDatas[(int)CardID.공격속도];
            atkSpd *= myCard.value[myCard.curLv];
        }

        return atkSpd;
    }

    public float CritFactorFormula(SceneType type)
    {
        float factor = 0;

        if (type == SceneType.Main)
        {
            factor = 1.2f + .1f * playData.atkCoinLevels[(int)AtkUpgradeType.치명타배율];
        }
        else
        {
            factor = 1.2f + .1f * (playData.atkCoinLevels[(int)AtkUpgradeType.치명타배율]
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.치명타배율]);
        }

        // 연구 적용
        factor *= (1 + .03f * playData.labResearchLevels[(int)ResearchType.Attak, (int)AtkRschType.치명타배율]);

        return factor;
    }

    public float RangeFormula(SceneType type)
    {
        float range = 0;

        if (type == SceneType.Main)
        {
            range = 20 + (.5f * playData.atkCoinLevels[(int)AtkUpgradeType.범위]);
        }
        else
        {
            range = 20 + .5f * (playData.atkCoinLevels[(int)AtkUpgradeType.범위]
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.범위]);
        }

        // 연구 적용
        range *= (1 + .02f * playData.labResearchLevels[(int)ResearchType.Attak, (int)AtkRschType.범위]);

        // 카드 적용
        if (CheckCard(CardID.공격범위))
        {
            CardData myCard = MainSceneManager.instance.cardDatas[(int)CardID.공격범위];
            range *= myCard.value[myCard.curLv];
        }

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
            maxHp = 5 * (playData.defCoinLevels[(int)DefUpgradeType.체력] + 1);
        }
        else
        {
            maxHp = 5 * (1 + GameManager.instance.defDollarLevels[(int)DefUpgradeType.체력]
                    + playData.defCoinLevels[(int)DefUpgradeType.체력]);
        }

        // 연구 적용
        maxHp *= (1 + .03f * playData.labResearchLevels[(int)ResearchType.Defense, (int)DefRschType.체력]);

        // 카드 적용
        if (CheckCard(CardID.체력))
        {
            CardData myCard = MainSceneManager.instance.cardDatas[(int)CardID.체력];
            maxHp *= myCard.value[myCard.curLv];
        }

        return maxHp;
    }

    public float HpRegenFormula(SceneType type)
    {
        float regenHp = 0;

        if (type == SceneType.Main)
        {
            regenHp = .04f * playData.defCoinLevels[(int)DefUpgradeType.체력회복];
        }
        else
        {
            regenHp = .04f * (GameManager.instance.defDollarLevels[(int)DefUpgradeType.체력회복]
                + playData.defCoinLevels[(int)DefUpgradeType.체력회복]);
        }

        // 연구 적용
        regenHp *= (1 + .03f * playData.labResearchLevels[(int)ResearchType.Defense, (int)DefRschType.체력회복]);

        // 카드 적용
        if (CheckCard(CardID.체력재생))
        {
            CardData myCard = MainSceneManager.instance.cardDatas[(int)CardID.체력재생];
            regenHp *= myCard.value[myCard.curLv];
        }

        return regenHp;
    }

    public float AbsDefFormula(SceneType type)
    {
        float absDef = 0;

        if (type == SceneType.Main)
        {
            absDef = .5f * playData.defCoinLevels[(int)DefUpgradeType.절대방어];
        }
        else
        {
            absDef = .5f * (GameManager.instance.defDollarLevels[(int)DefUpgradeType.절대방어]
                + playData.defCoinLevels[(int)DefUpgradeType.절대방어]);
        }

        // 연구 적용
        absDef *= (1 + .03f * playData.labResearchLevels[(int)ResearchType.Defense, (int)DefRschType.절대방어]);

        return absDef;
    }
}

public partial class PlayDataManager : MonoBehaviour
{
    // UtilFormula 함수만 가지고 있음
    public float DollarBonusFormula(SceneType type)
    {
        float dollarBonusFactor = 0;

        if (type == SceneType.Main)
        {
            dollarBonusFactor = 1 + .01f * playData.utilCoinLevels[(int)UtilUpgradeType.달러보너스];
        }
        else
        {
            dollarBonusFactor = 1 + .01f * (playData.utilCoinLevels[(int)UtilUpgradeType.달러보너스]
                + GameManager.instance.utilDollarLevels[(int)UtilUpgradeType.달러보너스]);
        }

        // 연구 적용
        dollarBonusFactor *= (1 + .02f * playData.labResearchLevels[(int)ResearchType.Utility, (int)UtilRschType.달러보너스]);

        // 카드 적용
        if (CheckCard(CardID.달러))
        {
            CardData myCard = MainSceneManager.instance.cardDatas[(int)CardID.달러];
            dollarBonusFactor *= myCard.value[myCard.curLv];
        }

        return dollarBonusFactor;
    }

    public float DollarWaveFormula(SceneType type)
    {
        float dollarWaveBonus = 0;

        if (type == SceneType.Main)
        {
            dollarWaveBonus = 4 * PlayDataManager.Instance.playData.utilCoinLevels[(int)UtilUpgradeType.달러웨이브];
        }
        else
        {
            dollarWaveBonus = 4 * (playData.utilCoinLevels[(int)UtilUpgradeType.달러웨이브]
                + GameManager.instance.utilDollarLevels[(int)UtilUpgradeType.달러웨이브]);

            // 게임씬이라면 작업장 배율 먼저 적용
            dollarWaveBonus *= GameManager.instance.DollarBonusFactor;
        }

        // 작업장 배율 적용
        dollarWaveBonus *= DollarBonusFormula(SceneType.Main);

        // 연구실 배율 적용
        dollarWaveBonus *= (1 + .02f * playData.labResearchLevels[(int)ResearchType.Utility, (int)UtilRschType.달러웨이브]);

        // 카드 적용
        if (CheckCard(CardID.달러))
        {
            CardData myCard = MainSceneManager.instance.cardDatas[(int)CardID.달러];
            dollarWaveBonus *= myCard.value[myCard.curLv];
        }

        return dollarWaveBonus;
    }

    public float CoinKillBonusFormula(SceneType type)
    {
        float coinKillBonus = 0;

        if (type == SceneType.Main)
        {
            coinKillBonus = 1 + .01f * PlayDataManager.Instance.playData.utilCoinLevels[(int)UtilUpgradeType.코인킬보너스];
        }
        else
        {
            coinKillBonus = 1 + (.01f * (playData.utilCoinLevels[(int)UtilUpgradeType.코인킬보너스]
                + GameManager.instance.utilDollarLevels[(int)UtilUpgradeType.코인킬보너스]));
        }

        // 연구실 배율 적용
        coinKillBonus *= (1 + .02f * playData.labResearchLevels[(int)ResearchType.Utility, (int)UtilRschType.코인보너스]);

        // 카드 적용
        if (CheckCard(CardID.코인))
        {
            CardData myCard = MainSceneManager.instance.cardDatas[(int)CardID.달러];
            coinKillBonus *= myCard.value[myCard.curLv];
        }

        return coinKillBonus;
    }

    public float CoinWaveFormula(SceneType type)
    {
        float coinWaveBonus = 0;

        if (type == SceneType.Main)
        {
            coinWaveBonus = playData.utilCoinLevels[(int)UtilUpgradeType.코인웨이브];
        }
        else
        {
            coinWaveBonus = playData.utilCoinLevels[(int)UtilUpgradeType.코인웨이브]
                + GameManager.instance.utilDollarLevels[(int)UtilUpgradeType.코인웨이브];
        }

        // 연구실 배율 적용
        coinWaveBonus *= (1 + .02f * playData.labResearchLevels[(int)ResearchType.Utility, (int)UtilRschType.코인웨이브]);

        // 카드 적용
        if (CheckCard(CardID.코인))
        {
            CardData myCard = MainSceneManager.instance.cardDatas[(int)CardID.달러];
            coinWaveBonus *= myCard.value[myCard.curLv];
        }

        return coinWaveBonus;
    }
}