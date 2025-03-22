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
    public int[] reqCardCount;      // ī�� �������� ���� �ʿ��� ī�� ����
    public int[] reqDia;            // ī�� �������� �ʿ��� ���̾� ����

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
