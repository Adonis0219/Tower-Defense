using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI costText;

    [SerializeField]
    public int slotIndex;

    public void OnSlotOpenClk()
    {
        slotIndex = PlayDataManager.Instance.playData.curSlotCount;
        int cost = PlayDataManager.Instance.playData.slotOpneCost[slotIndex - 1];

        if (PlayDataManager.Instance.MainDia < cost)
            return;
        
        PlayDataManager.Instance.MainDia -= cost;

        // 열린 슬롯만 드롭 가능하도록
        gameObject.AddComponent<DroppableUI>();

        // DnD를 편하게 해주기 위해 열린 슬롯이 줄어들 일이 없으니 그냥 파괴
        Destroy(transform.GetChild(0).gameObject);

        PlayDataManager.Instance.playData.curSlotCount++;

        // 현재 오픈된 슬롯 개수가 바뀌었으므로 비용 다시 설정
        cost = PlayDataManager.Instance.playData.slotOpneCost[slotIndex - 1];

        // 최대 개수가 아니면 복제하여 가격 설정해주기
        if (PlayDataManager.Instance.playData.curSlotCount != PlayDataManager.Instance.playData.maxSlotCount)
            Instantiate(CardPanelManager.instance.oriSlot, CardPanelManager.instance.activeSlotContent).GetComponent<CardSlot>().costText.text =
                cost + " <sprite=0>";
    }
}
