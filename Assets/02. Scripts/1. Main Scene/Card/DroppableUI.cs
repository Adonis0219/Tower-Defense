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
    }

    /// <summary>
    /// ���콺 �����Ͱ� ���� ������ ���� ���� ���η� �� �� 1ȸ ȣ��
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        //AudioManager.instance.PlaySfx(AudioManager.Sfx.Cursor);

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
        // ī�� ���� ��ũ�� �� ��� ����
        if (eventData.pointerDrag.name == "Scroll View")
            return;

        int myID = eventData.pointerDrag.GetComponent<Card>().MyData.cardID;

        // pointerDrag�� ���� �巡�� �ϰ� �ִ� ���(ī��)�� ��ȯ
        // �巡�� �ϰ� �ִ� ����� ���ų� ī�� �����̶�� �� ����
        // ���Կ� ������ ī��� �߿� �Ȱ��� ī�尡 �ִٸ� ����
        if ((eventData.pointerDrag == null) || (PlayDataManager.Instance.CheckCard((CardID)myID)))
            return;

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.CardEqu);

        // �̹� ���Կ� ī�尡 �ִٸ�
        if (transform.childCount > 1)
        {
            // ���� �ִ� ī�� ����
            Destroy(transform.GetChild(0).gameObject);
        }   // �� �ڽ��� �ֱ�


        // ���� �巡������ ī���� �θ� ī�� �������� ���ְ�
        eventData.pointerDrag.transform.SetParent(transform);
        // ��� ���� ��ư�� �� ���� �ö�� �� �ֵ��� ī��� ù��° �ڽ�����
        eventData.pointerDrag.transform.SetAsFirstSibling();
        // ��ġ�� ī�彽���� �߾����� ���ش�
        eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;
        // üũ ��ũ ����
        Destroy(eventData.pointerDrag.GetComponent<Card>().checkMark);
        Destroy(eventData.pointerDrag.GetComponent<Card>().upgradeMark);

        // ������ �ε����� �����ش�
        slotIndex = gameObject.GetComponent<CardSlot>().slotIndex;
        // ���� �������� ī�� ������ ���� �巡���� ī�� ������ �־��ֱ�
        PlayDataManager.Instance.playData.activedCardIDs[slotIndex] = myID;
    }
}
