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
    대미지, 공격속도, 체력, 체력재생,
    공격범위, 달러, 코인, 저속오라
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

    public int[] reqCardCount;      // 카드 레벨업을 위해 필요한 카드 개수
    public int[] reqDia;            // 카드 레벨업에 필요한 다이아 개수

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
            // 업그레이드 마크가 존재 할 때만
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
            // 체크마크가 존재 할 때만
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
            // 레벨이 오를 때마다 별 갯수 조정해주기
            StarSet();
        }
    }

    private void Update()
    {
        IsUsed = PlayDataManager.Instance.CheckCard((CardID)myData.cardID) ? true : false;

        cur_nextText.text = CurCardCount + "/" + reqCardCount[MyData.curLv];

        // 업데이트 말고 다른 곳도 가능?
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

        // 인포판넬 켜주기
        infoPN.SetActive(true);

        // 인포판넬에 기본 정보 전달하기
        //infoPN.GetComponent<CardInfoPanel>().InitSet(MyData.rarity.ToString(), MyData.cardDesc, MyData.curLv);
        infoPN.GetComponent<CardInfoPanel>().InitSet(MyData, starSprites[CurLv]);
    }
}
