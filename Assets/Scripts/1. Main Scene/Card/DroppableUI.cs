using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DroppableUI : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    Image img;
    RectTransform rect;
    Color oriColor;     // 카드 슬롯의 원래 색상

    int slotIndex;

    private void Awake()
    {
        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        oriColor = img.color;

        slotIndex = GetComponent<CardSlot>().slotIndex;
    }

    /// <summary>
    /// 마우스 포인터가 현재 아이템 슬롯 영역 내부로 들어갈 때 1회 호출
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 아이템 슬롯 색상을 빨간색으로 변경
        img.color = Color.red;
    }

    /// <summary>
    /// 마우스 포인터가 현재 아이템 슬롯 영역을 빠져나갈 때 1회 호출 
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        img.color = oriColor;
    }

    /// <summary>
    /// 현재 아이템 슬롯 영역 내부에서 드롭을 했을 때 1회 호출
    /// </summary>
    public void OnDrop(PointerEventData eventData)
    {
        // pointerDrag는 현재 드래그 하고 있는 대상(카드)을 반환
        // 드래그 하고 있는 대상이 없거나 카드 슬롯이라면 얼리 리턴
        if (eventData.pointerDrag == null)
            return;

        // 이미 자식이 있다면 -> 슬롯에 카드가 있다면
        if (transform.childCount != 0)
        {
            // 그 카드를 삭제
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        eventData.pointerDrag.transform.SetParent(transform);
        eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;

        // 현재 장착중인 카드 정보에 현재 드래그한 카드 데이터 넣어주기
        PlayDataManager.Instance.playData.activedCardDatas[slotIndex] = eventData.pointerDrag.GetComponent<Card>().MyData;

        string name = eventData.pointerDrag.GetComponent<Card>().MyData.cardName;

        Debug.Log(slotIndex + "번째 카드 슬롯에 " + name + " 카드 장착");
        Print.Array(PlayDataManager.Instance.playData.activedCardDatas);
    }
}
