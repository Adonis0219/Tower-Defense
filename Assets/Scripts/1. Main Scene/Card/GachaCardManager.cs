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

        // 얻은 적 없는 카드였다면      
        if (!card.IsGet)
            card.IsGet = true;

        CardData data = card.MyData;

        nameText.text = data.cardName;
        rarityText.text = data.rarity.ToString();
        DescText.text = data.cardDesc + data.value[data.curLv];

        // 카드를 얻었을 때 업그레이드 가능하면 띄워주기
        upText.SetActive(card.CurCardCount + 1 == reqCardCount[data.curLv]);

        // 카드 개수 더해주고 출력
        card.CurCardCount++;


        cur_nextText.text = data.curCardCount +"/" + reqCardCount[data.curLv];

        icon.sprite = data.cardIcon;
        starImg.sprite = starSprites[data.curLv];

        // 단차는 리턴
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
