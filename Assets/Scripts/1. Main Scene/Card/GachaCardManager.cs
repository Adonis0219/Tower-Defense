using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GachaCardManager : MonoBehaviour
{
    [SerializeField]
    CardData[] cardDatas = new CardData[10];

    [SerializeField]
    Transform[] cards = new Transform[10];

    [SerializeField]
    GameObject newText;
    [SerializeField]
    GameObject upText;

    [SerializeField]
    int[] reqCardCount;

    [Header("# Text")]
    [SerializeField]
    TextMeshProUGUI nameText;
    [SerializeField]
    TextMeshProUGUI rarityText;
    [SerializeField]
    TextMeshProUGUI cur_nextText;
    [SerializeField]
    TextMeshProUGUI DescText;

    [Header("# Image")]
    [SerializeField]
    Image icon;
    [SerializeField]
    Image starImg;
    [SerializeField]
    public Sprite[] starSprites;

    public void SetCard(Card card, int index)
    {       
        newText.SetActive(!card.IsGet);

        // ���� �� ���� ī�忴�ٸ�      
        if (!card.IsGet)
            card.IsGet = true;

        CardData data = card.MyData;

        nameText.text = data.cardName;
        rarityText.text = data.rarity.ToString();
        DescText.text = data.cardDesc + data.value[data.curLv];

        // ī�带 ����� �� ���׷��̵� �����ϸ� ����ֱ�
        upText.SetActive(card.CurCardCount + 1 == reqCardCount[data.curLv]);

        // ī�� ���� �����ְ� ���
        card.CurCardCount++;


        cur_nextText.text = data.curCardCount +"/" + reqCardCount[data.curLv];

        icon.sprite = data.cardIcon;
        starImg.sprite = starSprites[data.curLv];

        // ������ ����
        if (index == -1)
            return;

        cardDatas[index] = data;
    }

    public void Set10Card()
    {
        for (int i = 0; i < 10; i++)
        {

        }
    }
}
