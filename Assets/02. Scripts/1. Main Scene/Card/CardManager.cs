using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;

    [SerializeField]        // ��� ī�� ������
    public List<CardData> cardDatas;

    // �̱⸦ ���� ī�� ��
    [SerializeField]
    public List<Card> gachaDeck;

    // ī�� ��� ����
    [SerializeField]
    public Color[] rarityColors = new Color[3];
    
    // ��í Ƚ��
    int gachaCount;
    
    float totalWeight = 0;

    private void Awake()
    {
        instance = this;

        // �����Ͱ� ���� ���� ���� ���� �ֱ�
        if (PlayDataManager.Instance.playData.cardLvs != null)
        {
            PlayDataManager.Instance.playData.cardLvs = new int[cardDatas.Count];
        }

        SetCardWeight();
    }

    /// <summary>
    /// ī�带 �������� �ϳ� �̾���
    /// </summary>
    /// <returns>�������� ���� ī��</returns>
    public Card RandomCard()
    {
        float weight = 0;
        float selectNum = 0;

        // �� Weight���� 0 ~ 1 ������ ������ ����
        selectNum = totalWeight * Random.Range(0.0f, 1.0f);

        for (int i = 0; i < cardDatas.Count; i++)
        {
            // ī���� �߷��� ��� ������
            weight += cardDatas[i].weight;

            // ���� ���ں��� ���� �߷��� Ŀ����
            if (selectNum <= weight)
            {
                // �� ī�带 ����
                return gachaDeck[i];
            }
        }

        return null;
    }

    /// <summary>
    /// ����Ʈ ���� �� ��� ī���� ������ ���� ī���� Weight �������ִ� �Լ�
    /// </summary>
    void SetCardWeight()
    {
        // Where �Լ��� Count �Լ��� �̿��� ����Ʈ ���� ���ǿ� �´� ������ ���� ���ϱ�
        int commonCount = cardDatas.Where(n => n.rarity == CardRarity.�Ϲ�).Count();
        int rareCount = cardDatas.Where(n => n.rarity == CardRarity.����).Count();
        int epicCount = cardDatas.Where(n => n.rarity == CardRarity.����).Count();

        float weight = 0;

        for (int i = 0; i < cardDatas.Count; i++)
        {
            switch (cardDatas[i].rarity)
            {
                case CardRarity.�Ϲ�:
                    weight = 80 / commonCount;
                    break;
                case CardRarity.����:
                    weight = 15 / rareCount;
                    break;
                case CardRarity.����:
                    weight = 5 / epicCount;
                    break;
                default:
                    break;
            }
            cardDatas[i].weight = weight;
            totalWeight += weight;
        }
    }
}
