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

    // ��í Ƚ��
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
        // Where �Լ��� Count �Լ��� �̿��� ����Ʈ ���� ���ǿ� �´� ������ ���� ���ϱ�
        int commonCount = deck.Where(n => n.rarity == CardRarity.�Ϲ�).Count();
        int rareCount = deck.Where(n => n.rarity == CardRarity.����).Count();
        int epicCount = deck.Where(n => n.rarity == CardRarity.����).Count();

        Debug.Log("�Ϲ� ī�� : " + commonCount + "��\n���� ī�� : " + rareCount + "��\n���� ī�� : " + epicCount + "��");

        float weight = 0;

        for (int i = 0; i < deck.Count; i++)
        {
            switch (deck[i].rarity)
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
            deck[i].weight = weight;
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

        gachaDesc.text = 20 * gachaCount + "<sprite=0> ����Ͽ�\nī�� " + gachaCount + "���� �����ðڽ��ϱ�?";
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
