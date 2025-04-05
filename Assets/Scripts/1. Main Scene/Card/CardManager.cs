using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public List<CardData> deck = new List<CardData>();

    [SerializeField]
    Button gachaBt1;
    [SerializeField]
    Button gachaBt10;
    [SerializeField]
    GameObject gachaChkPN;
    [SerializeField]
    TextMeshProUGUI gachaDesc;

    // 가챠 횟수
    int gachaCount;
    

    float totalWeight = 0;

    private void Awake()
    {
        SetCardWeight();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gachaBt1.interactable = PlayDataManager.Instance.MainDia >= 20 ? true : false;
        gachaBt10.interactable = PlayDataManager.Instance.MainDia >= 200 ? true : false;
    }

    CardData RandomCard()
    {
        float weight = 0;
        float selectNum = 0;

        selectNum = totalWeight * Random.Range(0.0f, 1.0f);

        for (int i = 0; i < deck.Count; i++)
        {
            weight += deck[i].weight;
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
        int commonCount = deck.Where(n => n.rarity == CardRarity.일반).Count();
        int rareCount = deck.Where(n => n.rarity == CardRarity.레어).Count();
        int epicCount = deck.Where(n => n.rarity == CardRarity.에픽).Count();

        Debug.Log("일반 카드 : " + commonCount + "개\n레어 카드 : " + rareCount + "개\n에픽 카드 : " + epicCount + "개");

        float weight = 0;

        for (int i = 0; i < deck.Count; i++)
        {
            switch (deck[i].rarity)
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
            deck[i].weight = weight;
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

        gachaDesc.text = 20 * gachaCount + "<sprite=0> 사용하여\n카드 " + gachaCount + "개를 뽑으시겠습니까?";
    }

    public void YesClk()
    {
        PlayDataManager.Instance.MainDia -= 20 * gachaCount;

        gachaChkPN.SetActive(false);

        if (gachaCount == 1)
        {
            Debug.Log(RandomCard().cardName);
        }
        else
        {
            string printText = "";

            for (int i = 0; i < 10; i++)
            {
                printText += RandomCard().cardName + "\n";
            }

            Debug.Log(printText);
        }
    }

    public void NoClk()
    {
        gachaChkPN.SetActive(false);
    }
}
