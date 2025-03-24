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

        // ���� ���Ը� ��� �����ϵ���
        gameObject.AddComponent<DroppableUI>();

        // DnD�� ���ϰ� ���ֱ� ���� ���� ������ �پ�� ���� ������ �׳� �ı�
        Destroy(transform.GetChild(0).gameObject);

        PlayDataManager.Instance.playData.curSlotCount++;

        // ���� ���µ� ���� ������ �ٲ�����Ƿ� ��� �ٽ� ����
        cost = PlayDataManager.Instance.playData.slotOpneCost[slotIndex - 1];

        // �ִ� ������ �ƴϸ� �����Ͽ� ���� �������ֱ�
        if (PlayDataManager.Instance.playData.curSlotCount != PlayDataManager.Instance.playData.maxSlotCount)
            Instantiate(CardPanelManager.instance.oriSlot, CardPanelManager.instance.activeSlotContent).GetComponent<CardSlot>().costText.text =
                cost + " <sprite=0>";
    }
}
