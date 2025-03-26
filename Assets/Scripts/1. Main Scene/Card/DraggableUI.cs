using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform canvas;            // UI�� �ҼӵǾ� �ִ� �ֻ���� Canvas Transform
    Transform previousParent;    // �ش� ������Ʈ�� ������ �ҼӵǾ� �ִ� �θ� Transform
    RectTransform rect;          // UI ��ġ ��� ����
    CanvasGroup canvasGroup;     // UI�� ���İ��� ��ȣ�ۿ� ��� ���� CanvasGroup

    Transform tempCard;            // �巡�� ���� �� ����� Card

    public void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// ���� ������Ʈ�� �巡�� �ϱ� ������ �� 1ȸ ȣ��
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // ��� �ִ� ī���� �巡�� �Ұ���
        if (!gameObject.GetComponent<Card>().isOpen)
            return;

        InstantiateCard();

        // �巡�� ���� �ҼӵǾ� �ִ� �θ� Transform ���� ����
        previousParent = transform.parent;

        // ���� �巡�� ���� UI�� ȭ�� �ֻ�ܿ� ���̰� �ϱ� ����
        transform.SetParent(canvas);            // �θ� ������Ʈ�� Canvas�� ����
        transform.SetAsLastSibling();           // ���� �տ� ���̵��� Canvas�� ������ �ڽ����� �������ش�.


        // �巡�� ������ ������Ʈ�� �ϳ��� �ƴ� �ڽĵ��� ������ ���� ���� �ֱ� ������ CanvasGroup���� ��� ����
        // ���İ��� .6f�� �����ϰ�, ���� �浹ó���� ���� �ʵ��� �Ѵ�
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// ���� ������Ʈ�� �巡�� ���� �� �� ������ ȣ��
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        // ���� ��ũ������ ���콺 ��ġ�� UI�� Rect��ġ�� ���� (UI�� ���콺�� �Ѿƴٴϴ� ����)
        rect.position = eventData.position;
    }

    /// <summary>
    /// ���� ������Ʈ�� �巡�׸� ������ �� 1ȸ ȣ��
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        // ���İ��� 1��, �����浹ó�� �����ϵ��� �Ѵ�.
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

        // �巡�׸� �����ϸ� �θ� canvas�� �����Ǳ� ������
        // �巡�׸� ������ �� �θ� canvas�̸� ī�� ������ �ƴ� ������ ����
        // ����� �ߴٴ� ���̹Ƿ� �巡�� ������ �ҼӵǾ� �ִ� ������ �������� ������ �̵�
        if (transform.parent == canvas)
        {
            // ���� �Ҽ��� �θ��� �ڽ����� �־��ְ�, �ش� ��ġ�� ����
            //transform.SetParent(previousParent);
            //rect.position = previousParent.GetComponent<RectTransform>().position;

            DestroyImmediate(gameObject);
        }
    }

    /// <summary>
    /// �巡�� �� �� ���ƾ� �� ī�带 ���� ���ִ� �Լ�
    /// </summary>
    void InstantiateCard()
    {
        // �ڽ��� �����Ͽ� �ڽ��� �巡�װ� �ǰ� ������ ���� �ڽ��� ���� ��ġ�� �־��ֱ�
        tempCard = Instantiate(transform);
        // ������ �� ����� InitSet�� ���� �����Ƿ� MyData�� ����
        // ���� ���� MyData �־��ֱ�
        tempCard.GetComponent<Card>().MyData = GetComponent<Card>().MyData;
        // �̸� �ٲ��ֱ�
        tempCard.name = "Card(Clone)";
        // �θ� �ڽ��� �ִ� �θ�� �־��ֱ�
        tempCard.SetParent(transform.parent);
        // �������� ������ �°� ������ �ڽ��� ī�� ��ȣ�� �־��ֱ�
        tempCard.SetSiblingIndex(GetComponent<Card>().MyData.cardID);
    }

    // PointEventData Class
    // ���콺/��ġ ��ġ, ��ġ ��ȭ��, �巡�� ����, �巡�� ���� ���,
    // Ŭ�� Ƚ�� ���� ������Ƽ�� ����Ǿ� �ְ�,
    // OnDrag()�� ���� �޼ҵ尡 ȣ��� �� �Ű������� ������ �����Ѵ�.
}
