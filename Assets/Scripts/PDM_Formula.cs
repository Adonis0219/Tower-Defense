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
            damage = 3 * (playData.atkCoinLevels[(int)AtkUpgradeType.������] + 1);
        }
        else
        {
            damage = 3 * (playData.atkCoinLevels[(int)AtkUpgradeType.������]
               + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.������] + 1);
        }

        // ���� ����
        damage *= (1 + .02f * playData.labResearchLevels[(int)ResearchType.Attak, (int)AtkRschType.������]);

        // ī�� ����
        if (CheckCard(CardID.�����))
        {
            CardData myCard = MainSceneManager.instance.cardDatas[(int)CardID.�����];
            damage *= myCard.value[myCard.curLv];
        }

        return damage;
    }

    public float AtkSpdFormula(SceneType type)
    {
        float atkSpd = 0;

        if (type == SceneType.Main)
        {
            atkSpd = 1 + .05f * playData.atkCoinLevels[(int)AtkUpgradeType.���ݼӵ�];
        }
        else
        {
            atkSpd = 1 + .05f * (playData.atkCoinLevels[(int)AtkUpgradeType.���ݼӵ�]
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.���ݼӵ�]);
        }

        // ���� ����
        atkSpd *= (1 + .02f * playData.labResearchLevels[(int)ResearchType.Attak, (int)AtkRschType.���ݼӵ�]);

        // ī�� ����
        if (CheckCard(CardID.���ݼӵ�))
        {
            CardData myCard = MainSceneManager.instance.cardDatas[(int)CardID.���ݼӵ�];
            atkSpd *= myCard.value[myCard.curLv];
        }

        return atkSpd;
    }

    public float CritFactorFormula(SceneType type)
    {
        float factor = 0;

        if (type == SceneType.Main)
        {
            factor = 1.2f + .1f * playData.atkCoinLevels[(int)AtkUpgradeType.ġ��Ÿ����];
        }
        else
        {
            factor = 1.2f + .1f * (playData.atkCoinLevels[(int)AtkUpgradeType.ġ��Ÿ����]
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.ġ��Ÿ����]);
        }

        // ���� ����
        factor *= (1 + .03f * playData.labResearchLevels[(int)ResearchType.Attak, (int)AtkRschType.ġ��Ÿ����]);

        return factor;
    }

    public float RangeFormula(SceneType type)
    {
        float range = 0;

        if (type == SceneType.Main)
        {
            range = 20 + (.5f * playData.atkCoinLevels[(int)AtkUpgradeType.����]);
        }
        else
        {
            range = 20 + .5f * (playData.atkCoinLevels[(int)AtkUpgradeType.����]
                + GameManager.instance.atkDollarLevels[(int)AtkUpgradeType.����]);
        }

        // ���� ����
        range *= (1 + .02f * playData.labResearchLevels[(int)ResearchType.Attak, (int)AtkRschType.����]);

        // ī�� ����
        if (CheckCard(CardID.���ݹ���))
        {
            CardData myCard = MainSceneManager.instance.cardDatas[(int)CardID.���ݹ���];
            range *= myCard.value[myCard.curLv];
        }

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
            maxHp = 5 * (playData.defCoinLevels[(int)DefUpgradeType.ü��] + 1);
        }
        else
        {
            maxHp = 5 * (1 + GameManager.instance.defDollarLevels[(int)DefUpgradeType.ü��]
                    + playData.defCoinLevels[(int)DefUpgradeType.ü��]);
        }

        // ���� ����
        maxHp *= (1 + .03f * playData.labResearchLevels[(int)ResearchType.Defense, (int)DefRschType.ü��]);

        // ī�� ����
        if (CheckCard(CardID.ü��))
        {
            CardData myCard = MainSceneManager.instance.cardDatas[(int)CardID.ü��];
            maxHp *= myCard.value[myCard.curLv];
        }

        return maxHp;
    }

    public float HpRegenFormula(SceneType type)
    {
        float regenHp = 0;

        if (type == SceneType.Main)
        {
            regenHp = .04f * playData.defCoinLevels[(int)DefUpgradeType.ü��ȸ��];
        }
        else
        {
            regenHp = .04f * (GameManager.instance.defDollarLevels[(int)DefUpgradeType.ü��ȸ��]
                + playData.defCoinLevels[(int)DefUpgradeType.ü��ȸ��]);
        }

        // ���� ����
        regenHp *= (1 + .03f * playData.labResearchLevels[(int)ResearchType.Defense, (int)DefRschType.ü��ȸ��]);

        // ī�� ����
        if (CheckCard(CardID.ü�����))
        {
            CardData myCard = MainSceneManager.instance.cardDatas[(int)CardID.ü�����];
            regenHp *= myCard.value[myCard.curLv];
        }

        return regenHp;
    }

    public float AbsDefFormula(SceneType type)
    {
        float absDef = 0;

        if (type == SceneType.Main)
        {
            absDef = .5f * playData.defCoinLevels[(int)DefUpgradeType.������];
        }
        else
        {
            absDef = .5f * (GameManager.instance.defDollarLevels[(int)DefUpgradeType.������]
                + playData.defCoinLevels[(int)DefUpgradeType.������]);
        }

        // ���� ����
        absDef *= (1 + .03f * playData.labResearchLevels[(int)ResearchType.Defense, (int)DefRschType.������]);

        return absDef;
    }
}

public partial class PlayDataManager : MonoBehaviour
{
    // UtilFormula �Լ��� ������ ����
    public float DollarBonusFormula(SceneType type)
    {
        float dollarBonusFactor = 0;

        if (type == SceneType.Main)
        {
            dollarBonusFactor = 1 + .01f * playData.utilCoinLevels[(int)UtilUpgradeType.�޷����ʽ�];
        }
        else
        {
            dollarBonusFactor = 1 + .01f * (playData.utilCoinLevels[(int)UtilUpgradeType.�޷����ʽ�]
                + GameManager.instance.utilDollarLevels[(int)UtilUpgradeType.�޷����ʽ�]);
        }

        // ���� ����
        dollarBonusFactor *= (1 + .02f * playData.labResearchLevels[(int)ResearchType.Utility, (int)UtilRschType.�޷����ʽ�]);

        // ī�� ����
        if (CheckCard(CardID.�޷�))
        {
            CardData myCard = MainSceneManager.instance.cardDatas[(int)CardID.�޷�];
            dollarBonusFactor *= myCard.value[myCard.curLv];
        }

        return dollarBonusFactor;
    }

    public float DollarWaveFormula(SceneType type)
    {
        float dollarWaveBonus = 0;

        if (type == SceneType.Main)
        {
            dollarWaveBonus = 4 * PlayDataManager.Instance.playData.utilCoinLevels[(int)UtilUpgradeType.�޷����̺�];
        }
        else
        {
            dollarWaveBonus = 4 * (playData.utilCoinLevels[(int)UtilUpgradeType.�޷����̺�]
                + GameManager.instance.utilDollarLevels[(int)UtilUpgradeType.�޷����̺�]);

            // ���Ӿ��̶�� �۾��� ���� ���� ����
            dollarWaveBonus *= GameManager.instance.DollarBonusFactor;
        }

        // �۾��� ���� ����
        dollarWaveBonus *= DollarBonusFormula(SceneType.Main);

        // ������ ���� ����
        dollarWaveBonus *= (1 + .02f * playData.labResearchLevels[(int)ResearchType.Utility, (int)UtilRschType.�޷����̺�]);

        // ī�� ����
        if (CheckCard(CardID.�޷�))
        {
            CardData myCard = MainSceneManager.instance.cardDatas[(int)CardID.�޷�];
            dollarWaveBonus *= myCard.value[myCard.curLv];
        }

        return dollarWaveBonus;
    }

    public float CoinKillBonusFormula(SceneType type)
    {
        float coinKillBonus = 0;

        if (type == SceneType.Main)
        {
            coinKillBonus = 1 + .01f * PlayDataManager.Instance.playData.utilCoinLevels[(int)UtilUpgradeType.����ų���ʽ�];
        }
        else
        {
            coinKillBonus = 1 + (.01f * (playData.utilCoinLevels[(int)UtilUpgradeType.����ų���ʽ�]
                + GameManager.instance.utilDollarLevels[(int)UtilUpgradeType.����ų���ʽ�]));
        }

        // ������ ���� ����
        coinKillBonus *= (1 + .02f * playData.labResearchLevels[(int)ResearchType.Utility, (int)UtilRschType.���κ��ʽ�]);

        // ī�� ����
        if (CheckCard(CardID.����))
        {
            CardData myCard = MainSceneManager.instance.cardDatas[(int)CardID.�޷�];
            coinKillBonus *= myCard.value[myCard.curLv];
        }

        return coinKillBonus;
    }

    public float CoinWaveFormula(SceneType type)
    {
        float coinWaveBonus = 0;

        if (type == SceneType.Main)
        {
            coinWaveBonus = playData.utilCoinLevels[(int)UtilUpgradeType.���ο��̺�];
        }
        else
        {
            coinWaveBonus = playData.utilCoinLevels[(int)UtilUpgradeType.���ο��̺�]
                + GameManager.instance.utilDollarLevels[(int)UtilUpgradeType.���ο��̺�];
        }

        // ������ ���� ����
        coinWaveBonus *= (1 + .02f * playData.labResearchLevels[(int)ResearchType.Utility, (int)UtilRschType.���ο��̺�]);

        // ī�� ����
        if (CheckCard(CardID.����))
        {
            CardData myCard = MainSceneManager.instance.cardDatas[(int)CardID.�޷�];
            coinWaveBonus *= myCard.value[myCard.curLv];
        }

        return coinWaveBonus;
    }
}