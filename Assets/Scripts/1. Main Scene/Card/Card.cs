using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    CardData myData;

    public CardData MyData
    {
        get { return myData; }

        set 
        { 
            myData = value;
            InitSet();
        }
    }
    public int[] reqCardCount;      // 카드 레벨업을 위해 필요한 카드 개수
    public int[] reqDia;            // 카드 레벨업에 필요한 다이아 개수

    [SerializeField]
    public TextMeshProUGUI nameText;
    [SerializeField]
    public TextMeshProUGUI descText;
    [SerializeField]
    public Image icon;

    void InitSet()
    {
        nameText.text = MyData.cardName;
        //descText.text = MyData.cardDesc;
        icon.sprite = MyData.cardIcon;
    }
}
