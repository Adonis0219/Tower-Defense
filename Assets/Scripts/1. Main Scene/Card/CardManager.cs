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

    [SerializeField]
    public List<Card> deck;


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

    // Update is called once per frame

    public Card RandomCard()
    {
        float weight = 0;
        float selectNum = 0;

        selectNum = totalWeight * Random.Range(0.0f, 1.0f);

        for (int i = 0; i < cardDatas.Count; i++)
        {
            weight += cardDatas[i].weight;
            if (selectNum <= weight)
            {
                return deck[i];
            }
        }

        return null;
    }

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
