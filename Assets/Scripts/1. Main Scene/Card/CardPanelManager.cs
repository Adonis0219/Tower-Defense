using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPanelManager : MonoBehaviour
{
    public static CardPanelManager instance;

    [SerializeField]        // ��� ī�� ������
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

        // �����Ͱ� ���� ���� ���� ���� �ֱ�
        if (PlayDataManager.Instance.playData.cardLvs != null)
        {
            PlayDataManager.Instance.playData.cardLvs = new int[cardDatas.Length];
        }

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
            Transform temp = Instantiate(oriSlot, activeSlotContent);

            // ����� �����ϵ��� ���ֱ�
            temp.AddComponent<DroppableUI>();
            // ���� �ε��� �־��ֱ�
            temp.GetComponent<CardSlot>().slotIndex = i;

            // ���� ������ ������� �ǳ� ����
            Destroy(temp.GetChild(0).gameObject);
        }

        // ������ ������ �ִ� ������ �ƴ� ���� ������ ���� ���� �� ������� ���� �������ֱ�
        if (PlayDataManager.Instance.playData.curSlotCount != PlayDataManager.Instance.playData.maxSlotCount)
            Instantiate(oriSlot, activeSlotContent).GetComponent<CardSlot>().costText.text = PlayDataManager.Instance.playData.slotOpneCost[PlayDataManager.Instance.playData.curSlotCount - 1] + " <sprite=0>";

        // �κ��丮�� ī�� ����
        for (int i = 0; i < cardDatas.Length; i++)
        {
            Transform temp = Instantiate(oriCard, inventoryContent);
            temp.GetComponent<Card>().MyData = cardDatas[i];
        }
    }
}
