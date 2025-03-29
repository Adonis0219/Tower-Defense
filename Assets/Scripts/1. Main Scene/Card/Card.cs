using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Android.Types;
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
            // 체크마크가 존재 할 때만
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
            // 레벨이 오를 때마다 별 갯수 조정해주기
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
    /// 카드를 획득할 때 실행할 함수
    /// </summary>
    void GetCard()
    {
        isOpen = true;
        // 카드를 얻고 나면 다시 잠글 일이 없으므로 그냥 지워주기
        DestroyImmediate(transform.GetChild(0).gameObject);
        // 상위에 있던 자식(0)이 사라졌으므로 자신이 다시 0이 된다
        // 카드 인포 켜주기
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

        // 인포판넬 켜주기
        infoPN.SetActive(true);

        // 인포판넬에 기본 정보 전달하기
        //infoPN.GetComponent<CardInfoPanel>().InitSet(MyData.rarity.ToString(), MyData.cardDesc, MyData.curLv);
        infoPN.GetComponent<CardInfoPanel>().InitSet(MyData);
    }
}
