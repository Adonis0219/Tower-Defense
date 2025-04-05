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

    [SerializeField]
    public List<Card> deck;

    [SerializeField]
    Button gachaBt1;
    [SerializeField]
    Button gachaBt10;

    [Header("# 가챠 체크")]
    [SerializeField]
    GameObject gachaChkPN;
    [SerializeField]
    TextMeshProUGUI gachaChkDesc;

    [Header("# 가챠 결과 판넬")]
    [SerializeField]
    GameObject gachaResultPN;
    [SerializeField]
    GameObject gachaResultPNx10;

    [SerializeField]
    GameObject totalResultPN;
    


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

    // Update is called once per frame
    void Update()
    {
        gachaBt1.interactable = PlayDataManager.Instance.MainDia >= 20 ? true : false;
        gachaBt10.interactable = PlayDataManager.Instance.MainDia >= 200 ? true : false;
    }

    Card RandomCard()
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
        // Where 함수와 Count 함수를 이용해 리스트 안의 조건에 맞는 원소의 개수 구하기
        int commonCount = cardDatas.Where(n => n.rarity == CardRarity.일반).Count();
        int rareCount = cardDatas.Where(n => n.rarity == CardRarity.레어).Count();
        int epicCount = cardDatas.Where(n => n.rarity == CardRarity.에픽).Count();

        Debug.Log("일반 카드 : " + commonCount + "개\n레어 카드 : " + rareCount + "개\n에픽 카드 : " + epicCount + "개");

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

    /// <summary>
    /// 가차 버튼 클릭
    /// </summary>
    /// <param name="count">연차</param>
    public void GachaBtClk(int count)
    {
        // 가챠 횟수 정해주기
        gachaCount = count;

        // 가챠 체크 판넬 켜주기
        gachaChkPN.SetActive(true);


        gachaChkDesc.text = 20 * gachaCount + "<sprite=0> 사용하여\n카드 " + gachaCount + "개를 뽑으시겠습니까?";
    }

    public void YesClk(bool isReGacha)
    {
        PlayDataManager.Instance.MainDia -= 20 * gachaCount;

        // 재뽑기가 아닐 때만
        if (!isReGacha)
        {
            // 가챠 체크 꺼주기
            gachaChkPN.SetActive(false);
        }

        SetDia();

        if (gachaCount == 1)
            SetGachaPanel();
        else
            Set10GachaPanel();
    }

    void SetDia()
    {
        if (gachaCount == 1)
        {
            // 가챠 결과창 켜주기
            gachaResultPN.SetActive(true);
            gachaResultPN.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = PlayDataManager.Instance.MainDia + "<sprite=0>";
        }
        else
        {
            gachaResultPNx10.SetActive(true);
            gachaResultPNx10.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = PlayDataManager.Instance.MainDia + "<sprite=0>";
        }
    }

    public void OnNoClk()
    {
        gachaChkPN.SetActive(false);
    }

    public void OnNextClk()
    {
        if (gachaCount != 0)
            Set10GachaPanel();
        else SetTotalResultPN();
    }

    public void OnSkipClk()
    {
        SetTotalResultPN();
    }

    public void OnGetClk(GameObject go)
    {
        go.SetActive(false);
    }

    void SetGachaPanel()
    {
        gachaResultPN.GetComponentInChildren<GachaCardManager>().SetCard(RandomCard(), -1);
    }

    void Set10GachaPanel()
    {
        gachaResultPNx10.GetComponentInChildren<GachaCardManager>().SetCard(RandomCard(), 10 - gachaCount);
        gachaCount--;

        //if (gachaCount == 0)
        //    EventSystem.current.currentSelectedGameObject.SetActive(false);

        //Card card = RandomCard();
        //getCards.Add(card.MyData);
    }

    void SetTotalResultPN()
    {
        gachaResultPNx10.SetActive(false);
        totalResultPN.SetActive(true);
    }
}
