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
    }

    /// <summary>
    /// 마우스 포인터가 현재 아이템 슬롯 영역 내부로 들어갈 때 1회 호출
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        //AudioManager.instance.PlaySfx(AudioManager.Sfx.Cursor);

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
        // 카드 슬롯 스크롤 뷰 드롭 무시
        if (eventData.pointerDrag.name == "Scroll View")
            return;

        int myID = eventData.pointerDrag.GetComponent<Card>().MyData.cardID;

        // pointerDrag는 현재 드래그 하고 있는 대상(카드)을 반환
        // 드래그 하고 있는 대상이 없거나 카드 슬롯이라면 얼리 리턴
        // 슬롯에 장착된 카드들 중에 똑같은 카드가 있다면 리턴
        if ((eventData.pointerDrag == null) || (PlayDataManager.Instance.CheckCard((CardID)myID)))
            return;

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.CardEqu);

        // 이미 슬롯에 카드가 있다면
        if (transform.childCount > 1)
        {
            // 원래 있던 카드 삭제
            Destroy(transform.GetChild(0).gameObject);
        }   // 후 자신을 넣기


        // 현재 드래그중인 카드의 부모를 카드 슬롯으로 해주고
        eventData.pointerDrag.transform.SetParent(transform);
        // 장비 해제 버튼이 더 위로 올라올 수 있도록 카드는 첫번째 자식으로
        eventData.pointerDrag.transform.SetAsFirstSibling();
        // 위치도 카드슬롯의 중앙으로 해준다
        eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;
        // 체크 마크 제거
        Destroy(eventData.pointerDrag.GetComponent<Card>().checkMark);
        Destroy(eventData.pointerDrag.GetComponent<Card>().upgradeMark);

        // 슬롯의 인덱스를 정해준다
        slotIndex = gameObject.GetComponent<CardSlot>().slotIndex;
        // 현재 장착중인 카드 정보에 현재 드래그한 카드 데이터 넣어주기
        PlayDataManager.Instance.playData.activedCardIDs[slotIndex] = myID;
    }
}
