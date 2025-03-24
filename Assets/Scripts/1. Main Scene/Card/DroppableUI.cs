using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DroppableUI : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    Image img;
    RectTransform rect;
    Color oriColor;     // ī�� ������ ���� ����

    int slotIndex;

    private void Awake()
    {
        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        oriColor = img.color;

        slotIndex = GetComponent<CardSlot>().slotIndex;
    }

    /// <summary>
    /// ���콺 �����Ͱ� ���� ������ ���� ���� ���η� �� �� 1ȸ ȣ��
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        // ������ ���� ������ ���������� ����
        img.color = Color.red;
    }

    /// <summary>
    /// ���콺 �����Ͱ� ���� ������ ���� ������ �������� �� 1ȸ ȣ�� 
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        img.color = oriColor;
    }

    /// <summary>
    /// ���� ������ ���� ���� ���ο��� ����� ���� �� 1ȸ ȣ��
    /// </summary>
    public void OnDrop(PointerEventData eventData)
    {
        // pointerDrag�� ���� �巡�� �ϰ� �ִ� ���(ī��)�� ��ȯ
        // �巡�� �ϰ� �ִ� ����� ���ų� ī�� �����̶�� �� ����
        if (eventData.pointerDrag == null)
            return;

        // �̹� �ڽ��� �ִٸ� -> ���Կ� ī�尡 �ִٸ�
        if (transform.childCount != 0)
        {
            // �� ī�带 ����
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        eventData.pointerDrag.transform.SetParent(transform);
        eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;

        // ���� �������� ī�� ������ ���� �巡���� ī�� ������ �־��ֱ�
        PlayDataManager.Instance.playData.activedCardDatas[slotIndex] = eventData.pointerDrag.GetComponent<Card>().MyData;

        string name = eventData.pointerDrag.GetComponent<Card>().MyData.cardName;

        Debug.Log(slotIndex + "��° ī�� ���Կ� " + name + " ī�� ����");
        Print.Array(PlayDataManager.Instance.playData.activedCardDatas);
    }
}
