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

        //// �����Ͱ� ���� ���� ���� ���� �ֱ�
        //if (PlayDataManager.Instance.playData.cardLvs != null)
        //{
        //    PlayDataManager.Instance.playData.cardLvs = new int[cardDatas.Length];
        //}

        InitCardPanelSet();
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
        
        // ī�� ���� �߰�
        CardManager.instance.deck.Add(card);

        card.MyData = CardManager.instance.cardDatas[index];

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
    }
}
