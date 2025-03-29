using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum CardID
{
    �����, ���ݼӵ�, ü��, ü�����,
    ���ݹ���, �޷�, ����, ���ӿ���
}

[System.Serializable]
public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
    public Image icon;
    [SerializeField]
    public GameObject checkMark;
    [SerializeField]
    public Image starImg;

    [SerializeField]
    public Sprite[] starSprites;

    public bool isOpen = false;

    [SerializeField]
    bool isUsed;
    public bool IsUsed
    {
        get
        {
            return isUsed;
        }

        set
        {
            isUsed = value;
            // üũ��ũ�� ���� �� ����
            if (checkMark != null)
                checkMark.SetActive(isUsed);
        }
    }

    public int CurLv
    {
        get
        {
            return MyData.curLv;
        }

        set
        {
            MyData.curLv = value;
            // ������ ���� ������ �� ���� �������ֱ�
            StarSet();
        }
    }


    private void OnEnable()
    {
        isOpen = true;
        //GetCard();
    }

    float t;

    private void Update()
    {
        IsUsed = PlayDataManager.Instance.CheckCard((CardID)myData.cardID) ? true : false;
    }

    void InitSet()
    {
        nameText.text = MyData.cardName;
        icon.sprite = MyData.cardIcon;
        StarSet();
    }

    void StarSet()
    {
        starImg.sprite = starSprites[MyData.curLv];
    }

    /////////// Temp
    public void OnUpClk()
    {
        if (CurLv == myData.MaxLv)
            return;

        CurLv++;
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

    Coroutine coru;

    public void OnPointerDown(PointerEventData eventData)
    {
        coru = StartCoroutine(MouseDownTime());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopCoroutine(coru);
    }
    IEnumerator MouseDownTime()
    {
        yield return new WaitForSeconds(1f);

        OpenCardInfo();
    }

    void OpenCardInfo()
    {
        GameObject infoPN = CardPanelManager.instance.cardInfoPanel;

        // �����ǳ� ���ֱ�
        infoPN.SetActive(true);

        // �����ǳڿ� �⺻ ���� �����ϱ�
        //infoPN.GetComponent<CardInfoPanel>().InitSet(MyData.rarity.ToString(), MyData.cardDesc, MyData.curLv);
        infoPN.GetComponent<CardInfoPanel>().InitSet(MyData);
    }
}
