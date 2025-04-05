using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPanelManager : MonoBehaviour
{
    public static CardPanelManager instance;

    [SerializeField]
    public Transform inventoryContent;
    [SerializeField]
    public Transform activeSlotContent;

    [SerializeField]
    public Transform oriCard;
    [SerializeField]
    public Transform oriSlot;

    [SerializeField]
    public GameObject cardInfoPanel;

    private void Awake()
    {
        instance = this;

        //// 데이터가 없을 때만 새로 만들어서 넣기
        //if (PlayDataManager.Instance.playData.cardLvs != null)
        //{
        //    PlayDataManager.Instance.playData.cardLvs = new int[cardDatas.Length];
        //}

        InitCardPanelSet();
    }

    /// <summary>
    /// 초기 카드 판넬을 설정해주는 함수
    /// </summary>
    public void InitCardPanelSet()
    {
        // 카드 슬롯 복제
        for (int i = 0; i < PlayDataManager.Instance.playData.curSlotCount; i++)
        {
            CreCardSlot(i);
        }

        // 마지막 슬롯이 최대 슬롯이 아닐 때는 마지막 슬롯 복제 후 잠금해제 가격 설정해주기
        if (PlayDataManager.Instance.playData.curSlotCount != PlayDataManager.Instance.playData.maxSlotCount)
            Instantiate(oriSlot, activeSlotContent).GetComponent<CardSlot>().costText.text = PlayDataManager.Instance.playData.slotOpneCost[PlayDataManager.Instance.playData.curSlotCount - 1] + " <sprite=0>";

        // 인벤토리에 카드 복제
        for (int i = 0; i < CardManager.instance.cardDatas.Count; i++)
        {
            CreCard(i, inventoryContent);
        }
    }

    /// <summary>
    /// 카드 슬롯을 복제해주는 함수
    /// </summary>
    /// <param name="index">슬롯 인덱스</param>
    void CreCardSlot(int index)
    {
        Transform temp = Instantiate(oriSlot, activeSlotContent);

        // 드롭이 가능하도록 해주기
        temp.AddComponent<DroppableUI>();
        // 슬롯 인덱스 넣어주기
        temp.gameObject.GetComponent<CardSlot>().slotIndex = index;

        // 열린 슬롯의 잠금해제 판넬 제거
        Destroy(temp.GetChild(0).gameObject);

        int cardID = PlayDataManager.Instance.playData.activedCardIDs[index];

        // 카드 슬롯에 이미 장착된 카드가 있을 때 카드 장착시켜주기
        if (cardID != -1)
        {
            CreCard(cardID, temp);
        }

        // 장비해제 버튼을 마지막 자식으로
        temp.GetChild(1).SetAsLastSibling();
        // 장비해제 버튼
        // 항상 마지막 자식이므로
        temp.GetChild(temp.childCount - 1).gameObject.SetActive(true);
    }

    /// <summary>
    /// 카드를 복제해주는 함수
    /// </summary>
    /// <param name="index">카드의 ID</param>
    /// <param name="parent">복제해줄 위치(부모)</param>
    void CreCard(int index, Transform parent)
    {
        Transform temp = Instantiate(oriCard, parent);
        Card card = temp.GetComponent<Card>();
        
        // 카드 덱에 추가
        CardManager.instance.deck.Add(card);

        card.MyData = CardManager.instance.cardDatas[index];

        card.CurCardCount = card.CurCardCount;

        // 카드를 얻은 상태라면
        if (card.MyData.isGet)
            // 카드 열어주기
            card.CardSet();

        // 슬롯에 생성되는 카드라면
        if (parent != inventoryContent)
        {
            // 체크, 업그레이드 마크 제거
            Destroy(temp.GetComponent<Card>().checkMark);
            Destroy(temp.GetComponent<Card>().upgradeMark);
        }
    }
}
