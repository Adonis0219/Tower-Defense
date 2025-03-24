using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPanelManager : MonoBehaviour
{
    public static CardPanelManager instance;

    [SerializeField]        // 모든 카드 데이터
    public CardData[] cardDatas;

    [SerializeField]
    public Transform inventoryContent;
    [SerializeField]
    public Transform activeSlotContent;

    [SerializeField]
    public Transform oriCard;
    [SerializeField]
    public Transform oriSlot;

    private void Awake()
    {
        instance = this;

        // 데이터가 없을 때만 새로 만들어서 넣기
        if (PlayDataManager.Instance.playData.cardLvs != null)
        {
            PlayDataManager.Instance.playData.cardLvs = new int[cardDatas.Length];
        }

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
            Transform temp = Instantiate(oriSlot, activeSlotContent);

            // 드롭이 가능하도록 해주기
            temp.AddComponent<DroppableUI>();
            // 슬롯 인덱스 넣어주기
            temp.GetComponent<CardSlot>().slotIndex = i;

            // 열린 슬롯의 잠금해제 판넬 제거
            Destroy(temp.GetChild(0).gameObject);
        }

        // 마지막 슬롯이 최대 슬롯이 아닐 때는 마지막 슬롯 복제 후 잠금해제 가격 설정해주기
        if (PlayDataManager.Instance.playData.curSlotCount != PlayDataManager.Instance.playData.maxSlotCount)
            Instantiate(oriSlot, activeSlotContent).GetComponent<CardSlot>().costText.text = PlayDataManager.Instance.playData.slotOpneCost[PlayDataManager.Instance.playData.curSlotCount - 1] + " <sprite=0>";

        // 인벤토리에 카드 복제
        for (int i = 0; i < cardDatas.Length; i++)
        {
            Transform temp = Instantiate(oriCard, inventoryContent);
            temp.GetComponent<Card>().MyData = cardDatas[i];
        }
    }
}
