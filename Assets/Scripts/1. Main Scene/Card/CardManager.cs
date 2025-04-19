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

    [SerializeField]        // 모든 카드 데이터
    public List<CardData> cardDatas;

    // 뽑기를 위한 카드 덱
    [SerializeField]
    public List<Card> gachaDeck;

    // 카드 등급 색상
    [SerializeField]
    public Color[] rarityColors = new Color[3];
    
    // 가챠 횟수
    int gachaCount;
    
    float totalWeight = 0;

    private void Awake()
    {
        instance = this;

        // 데이터가 없을 때만 새로 만들어서 넣기
        if (PlayDataManager.Instance.playData.cardLvs != null)
        {
            PlayDataManager.Instance.playData.cardLvs = new int[cardDatas.Count];
        }

        SetCardWeight();
    }

    /// <summary>
    /// 카드를 랜덤으로 하나 뽑아줌
    /// </summary>
    /// <returns>랜덤으로 뽑힌 카드</returns>
    public Card RandomCard()
    {
        float weight = 0;
        float selectNum = 0;

        // 총 Weight에서 0 ~ 1 사이의 비율로 뽑음
        selectNum = totalWeight * Random.Range(0.0f, 1.0f);

        for (int i = 0; i < cardDatas.Count; i++)
        {
            // 카드의 중량을 계속 더해줌
            weight += cardDatas[i].weight;

            // 뽑은 숫자보다 누적 중량이 커지면
            if (selectNum <= weight)
            {
                // 그 카드를 리턴
                return gachaDeck[i];
            }
        }

        return null;
    }

    /// <summary>
    /// 리스트 내의 각 등급 카드의 개수에 따라 카드의 Weight 설정해주는 함수
    /// </summary>
    void SetCardWeight()
    {
        // Where 함수와 Count 함수를 이용해 리스트 안의 조건에 맞는 원소의 개수 구하기
        int commonCount = cardDatas.Where(n => n.rarity == CardRarity.일반).Count();
        int rareCount = cardDatas.Where(n => n.rarity == CardRarity.레어).Count();
        int epicCount = cardDatas.Where(n => n.rarity == CardRarity.에픽).Count();

        float weight = 0;

        for (int i = 0; i < cardDatas.Count; i++)
        {
            switch (cardDatas[i].rarity)
            {
                case CardRarity.일반:
                    weight = 80 / commonCount;
                    break;
                case CardRarity.레어:
                    weight = 15 / rareCount;
                    break;
                case CardRarity.에픽:
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
