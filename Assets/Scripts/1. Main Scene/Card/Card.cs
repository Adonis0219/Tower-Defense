using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Android.Types;
using Unity.VisualScripting;
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
    TextMeshProUGUI nameText;
    [SerializeField]
    Image icon;
    [SerializeField]
    public GameObject checkMark;
    [SerializeField]
    public GameObject upgradeMark;
    [SerializeField]
    Image starImg;
    [SerializeField]
    public TextMeshProUGUI cur_nextText;

    [SerializeField]
    public Sprite[] starSprites;

    public int CurCardCount
    {
        get
        {
            return MyData.curCardCount;
        }

        set
        {
            MyData.curCardCount = value;

            //cur_nextText.color = CurCardCount >= reqCardCount[MyData.curLv] ? Color.red : Color.white;
            CanUpgrade = CurCardCount >= reqCardCount[MyData.curLv] ? true : false;
        }
    }

    [SerializeField]
    bool canUpgrade = false;

    public bool CanUpgrade
    {
        get
        {
            return canUpgrade;
        }

        set
        {
            canUpgrade = value;

            if (canUpgrade)
                CardUpgrdae();
            // ���׷��̵� ��ũ�� ���� �� ����
            //if (upgradeMark != null)
            //    upgradeMark.SetActive(canUpgrade);
        }
    }


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

    public bool IsGet
    {
        get
        {
            return MyData.isGet;
        }

        set
        {
            MyData.isGet = value;

            if (!MyData.isGet) return;
            CardSet();
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

    private void Update()
    {
        IsUsed = PlayDataManager.Instance.CheckCard((CardID)myData.cardID) ? true : false;

        cur_nextText.text = CurCardCount + "/" + reqCardCount[MyData.curLv];

        // ������Ʈ ���� �ٸ� ���� ����?
        StarSet();
    }

    void InitSet()
    {
        nameText.text = MyData.cardName;
        icon.sprite = MyData.cardIcon;
        StarSet();
    }

    void StarSet()
    {
        starImg.sprite = starSprites[CurLv];
    }

    public void CardSet()
    {
        GetComponent<DraggableUI>().enabled = true;

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
    }

    void CardUpgrdae()
    {
        CurCardCount -= reqCardCount[MyData.curLv];
        MyData.curLv++;
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
        infoPN.GetComponent<CardInfoPanel>().InitSet(MyData, starSprites[CurLv]);
    }
}
