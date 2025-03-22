using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI costText;

    public void OnSlotOpenClk()
    {
        int cost = PlayDataManager.Instance.playData.slotOpneCost[PlayDataManager.Instance.playData.curSlotCount - 1];

        if (PlayDataManager.Instance.MainDia < cost)
            return;

        PlayDataManager.Instance.MainDia -= cost;

        // DnD�� ���ϰ� ���ֱ� ���� ���� ������ �پ�� ���� ������ �׳� �ı�
        Destroy(transform.GetChild(0).gameObject);

        PlayDataManager.Instance.playData.curSlotCount++;

        // ���� ���µ� ���� ������ �ٲ�����Ƿ� ��� �ٽ� ����
        cost = PlayDataManager.Instance.playData.slotOpneCost[PlayDataManager.Instance.playData.curSlotCount - 1];

        // �ִ� ������ �ƴϸ� �����Ͽ� ���� �������ֱ�
        if (PlayDataManager.Instance.playData.curSlotCount != PlayDataManager.Instance.playData.maxSlotCount)
            Instantiate(CardPanelManager.instance.oriSlot, CardPanelManager.instance.activeSlotContent).GetComponent<CardSlot>().costText.text =
                cost + " <sprite=0>";
    }
}
