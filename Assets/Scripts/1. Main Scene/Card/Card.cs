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
    [SerializeField]
    public bool isOpen = false;

    private void OnEnable()
    {
        isOpen = true;
        //GetCard();
    }

    void InitSet()
    {
        nameText.text = MyData.cardName;
        //descText.text = MyData.cardDesc;
        icon.sprite = MyData.cardIcon;
    }

    /// <summary>
    /// ī�带 ȹ���� �� ������ �Լ�
    /// </summary>
    void GetCard()
    {
        isOpen = true;
        // ī�带 ��� ���� �ٽ� ��� ���� �����Ƿ� �׳� �����ֱ�
        DestroyImmediate(transform.GetChild(0).gameObject);
        // ������ �ִ� �ڽ�(0)�� ��������Ƿ� �ڽ��� �ٽ� 0�� �ȴ�
        // ī�� ���� ���ֱ�
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
