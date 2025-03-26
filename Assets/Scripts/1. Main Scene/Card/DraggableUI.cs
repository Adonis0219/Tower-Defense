using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform canvas;            // UI가 소속되어 있는 최상단의 Canvas Transform
    Transform previousParent;    // 해당 오브젝트가 직전에 소속되어 있던 부모 Transform
    RectTransform rect;          // UI 위치 제어를 위함
    CanvasGroup canvasGroup;     // UI의 알파값과 상호작용 제어를 위한 CanvasGroup

    Transform tempCard;            // 드래그 시작 시 복사된 Card

    public void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// 현재 오브젝트를 드래그 하기 시작할 때 1회 호출
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 잠겨 있는 카드라면 드래그 불가능
        if (!gameObject.GetComponent<Card>().isOpen)
            return;

        InstantiateCard();

        // 드래그 직에 소속되어 있던 부모 Transform 정보 저장
        previousParent = transform.parent;

        // 현재 드래그 중인 UI가 화면 최상단에 보이게 하기 위해
        transform.SetParent(canvas);            // 부모 오브젝트를 Canvas로 설정
        transform.SetAsLastSibling();           // 가장 앞에 보이도록 Canvas의 마지막 자식으로 설정해준다.


        // 드래그 가능한 오브젝트가 하나가 아닌 자식들을 가지고 있을 수도 있기 때문에 CanvasGroup으로 묶어서 통제
        // 알파값을 .6f로 설정하고, 광선 충돌처리가 되지 않도록 한다
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// 현재 오브젝트를 드래그 중일 때 매 프레임 호출
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        // 현재 스크린상의 마우스 위치를 UI의 Rect위치로 설정 (UI가 마우스를 쫓아다니는 상태)
        rect.position = eventData.position;
    }

    /// <summary>
    /// 현재 오브젝트이 드래그를 종료할 때 1회 호출
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        // 알파값을 1로, 광선충돌처리 가능하도록 한다.
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

        // 드래그를 시작하면 부모가 canvas로 설정되기 때문에
        // 드래그를 종료할 때 부모가 canvas이면 카드 슬롯이 아닌 엉뚱한 곳에
        // 드롭을 했다는 뜻이므로 드래그 직전에 소속되어 있던 아이템 슬롯으로 아이템 이동
        if (transform.parent == canvas)
        {
            // 이전 소속의 부모의 자식으로 넣어주고, 해당 위치로 설정
            //transform.SetParent(previousParent);
            //rect.position = previousParent.GetComponent<RectTransform>().position;

            DestroyImmediate(gameObject);
        }
    }

    /// <summary>
    /// 드래그 할 때 남아야 할 카드를 복제 해주는 함수
    /// </summary>
    void InstantiateCard()
    {
        // 자신을 복제하여 자신은 드래그가 되고 복제된 것은 자신의 원래 위치로 넣어주기
        tempCard = Instantiate(transform);
        // 복제가 된 대상은 InitSet을 하지 않으므로 MyData가 없다
        // 나와 같은 MyData 넣어주기
        tempCard.GetComponent<Card>().MyData = GetComponent<Card>().MyData;
        // 이름 바꿔주기
        tempCard.name = "Card(Clone)";
        // 부모를 자신이 있던 부모로 넣어주기
        tempCard.SetParent(transform.parent);
        // 복제본이 순서에 맞게 들어가도록 자신의 카드 번호로 넣어주기
        tempCard.SetSiblingIndex(GetComponent<Card>().MyData.cardID);
    }

    // PointEventData Class
    // 마우스/터치 위치, 위치 변화값, 드래그 여부, 드래그 중인 대상,
    // 클릭 횟수 등의 프로퍼티가 선언되어 있고,
    // OnDrag()와 같은 메소드가 호출될 때 매개변수로 정보를 전달한다.
}
