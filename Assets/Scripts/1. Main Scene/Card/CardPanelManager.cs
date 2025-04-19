using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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

    [SerializeField]
    public TextMeshProUGUI active_slotText;

    private void Awake()
    {
        instance = this;

        //// �����Ͱ� ���� ���� ���� ���� �ֱ�
        //if (PlayDataManager.Instance.playData.cardLvs != null)
        //{
        //    PlayDataManager.Instance.playData.cardLvs = new int[cardDatas.Length];
        //}

        InitCardPanelSet();
    }

    int activeCardCount = 0;

    private void Update()
    {
        // �������� ���� ī�� ������ ���� �ϰ� ���� ����
        activeCardCount = PlayDataManager.Instance.playData.activedCardIDs.Where(n => n != -1).Count();

        active_slotText.text = activeCardCount + " / " + PlayDataManager.Instance.playData.curSlotCount;
    }

    /// <summary>
    /// �ʱ� ī�� �ǳ��� �������ִ� �Լ�
    /// </summary>
    public void InitCardPanelSet()
    {
        // ī�� ���� ����
        for (int i = 0; i < PlayDataManager.Instance.playData.curSlotCount; i++)
        {
            CreCardSlot(i);
        }

        // ������ ������ �ִ� ������ �ƴ� ���� ������ ���� ���� �� ������� ���� �������ֱ�
        if (PlayDataManager.Instance.playData.curSlotCount != PlayDataManager.Instance.playData.maxSlotCount)
            Instantiate(oriSlot, activeSlotContent).GetComponent<CardSlot>().costText.text = PlayDataManager.Instance.playData.slotOpneCost[PlayDataManager.Instance.playData.curSlotCount - 1] + " <sprite=0>";

        // �κ��丮�� ī�� ����
        for (int i = 0; i < CardManager.instance.cardDatas.Count; i++)
        {
            CreCard(i, inventoryContent);
        }
    }

    /// <summary>
    /// ī�� ������ �������ִ� �Լ�
    /// </summary>
    /// <param name="index">���� �ε���</param>
    void CreCardSlot(int index)
    {
        Transform temp = Instantiate(oriSlot, activeSlotContent);

        // ����� �����ϵ��� ���ֱ�
        temp.AddComponent<DroppableUI>();
        // ���� �ε��� �־��ֱ�
        temp.gameObject.GetComponent<CardSlot>().slotIndex = index;

        // ���� ������ ������� �ǳ� ����
        Destroy(temp.GetChild(0).gameObject);

        int cardID = PlayDataManager.Instance.playData.activedCardIDs[index];

        // ī�� ���Կ� �̹� ������ ī�尡 ���� �� ī�� ���������ֱ�
        if (cardID != -1)
        {
            CreCard(cardID, temp);
        }

        // ������� ��ư�� ������ �ڽ�����
        temp.GetChild(1).SetAsLastSibling();
        // ������� ��ư
        // �׻� ������ �ڽ��̹Ƿ�
        temp.GetChild(temp.childCount - 1).gameObject.SetActive(true);
    }

    /// <summary>
    /// ī�带 �������ִ� �Լ�
    /// </summary>
    /// <param name="index">ī���� ID</param>
    /// <param name="parent">�������� ��ġ(�θ�)</param>
    void CreCard(int index, Transform parent)
    {
        Transform temp = Instantiate(oriCard, parent);
        Card card = temp.GetComponent<Card>();

        card.MyData = CardManager.instance.cardDatas[index];

        // ���� ���� �ֽ�ȭ
        card.CurCardCount = card.CurCardCount;

        // ī�带 ���� ���¶��
        if (card.MyData.isGet)
            // ī�� �����ֱ�
            card.CardSet();

        // ���Կ� �����Ǵ� ī����
        if (parent != inventoryContent)
        {
            // üũ, ���׷��̵� ��ũ ����
            Destroy(temp.GetComponent<Card>().checkMark);
            Destroy(temp.GetComponent<Card>().upgradeMark);
        }
        // �κ��丮�� �����Ǵ� ī����
        else
            // ī�� ���� �߰�
            CardManager.instance.gachaDeck.Add(card);

    }
}
