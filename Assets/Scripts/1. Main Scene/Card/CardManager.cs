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

    [SerializeField]
    Button gachaBt1;
    [SerializeField]
    Button gachaBt10;

    [Header("# ��í üũ")]
    [SerializeField]
    GameObject gachaChkPN;
    [SerializeField]
    TextMeshProUGUI gachaChkDesc;

    [Header("# ��í ��� �ǳ�")]
    [SerializeField]
    GameObject gachaResultPN;
    [SerializeField]
    GameObject gachaResultPNx10;

    [SerializeField]
    GameObject totalResultPN;
    


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
        // Where �Լ��� Count �Լ��� �̿��� ����Ʈ ���� ���ǿ� �´� ������ ���� ���ϱ�
        int commonCount = cardDatas.Where(n => n.rarity == CardRarity.�Ϲ�).Count();
        int rareCount = cardDatas.Where(n => n.rarity == CardRarity.����).Count();
        int epicCount = cardDatas.Where(n => n.rarity == CardRarity.����).Count();

        Debug.Log("�Ϲ� ī�� : " + commonCount + "��\n���� ī�� : " + rareCount + "��\n���� ī�� : " + epicCount + "��");

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

    /// <summary>
    /// ���� ��ư Ŭ��
    /// </summary>
    /// <param name="count">����</param>
    public void GachaBtClk(int count)
    {
        // ��í Ƚ�� �����ֱ�
        gachaCount = count;

        // ��í üũ �ǳ� ���ֱ�
        gachaChkPN.SetActive(true);


        gachaChkDesc.text = 20 * gachaCount + "<sprite=0> ����Ͽ�\nī�� " + gachaCount + "���� �����ðڽ��ϱ�?";
    }

    public void YesClk(bool isReGacha)
    {
        PlayDataManager.Instance.MainDia -= 20 * gachaCount;

        // ��̱Ⱑ �ƴ� ����
        if (!isReGacha)
        {
            // ��í üũ ���ֱ�
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
            // ��í ���â ���ֱ�
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
