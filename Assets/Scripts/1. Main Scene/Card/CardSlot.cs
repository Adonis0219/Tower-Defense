using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI costText;

    [SerializeField]
    public int slotIndex;

    private void Update()
    {
        // ���� ������ ī�尡 ���ٸ� ����
        if (transform.childCount == 0)
            return;
    }

    /// <summary>
    /// ��� ī�� ������ ������ �� ������ �Լ�
    /// </summary>
    public void OnSlotOpenClk()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.UnlockClk);

        // ���� �ε��� ����
        slotIndex = PlayDataManager.Instance.playData.curSlotCount;
        // ���� ����
        int cost = PlayDataManager.Instance.playData.slotOpneCost[slotIndex - 1];

        // ���̾� ���� �� �� ����
        if (PlayDataManager.Instance.MainDia < cost)
            return;
        
        PlayDataManager.Instance.MainDia -= cost;

        // ���� ���Ը� ��� �����ϵ���
        gameObject.AddComponent<DroppableUI>();

        // DnD�� ���ϰ� ���ֱ� ���� ���� ������ �پ�� ���� ������
        // ��� �ǳ� ����
        Destroy(transform.GetChild(0).gameObject);

        PlayDataManager.Instance.playData.curSlotCount++;

        // ���/���� ��ư ���ֱ�
        // (�� ������ ���� ����Ǿ� ���� 0��° �ڽ��� ������� ���̹Ƿ� 1)
        transform.GetChild(1).gameObject.SetActive(true);

        // ���� ���µ� ���� ������ �ٲ�����Ƿ� ��� �ٽ� ����
        cost = PlayDataManager.Instance.playData.slotOpneCost[slotIndex - 1];

        // �ִ� ������ �ƴϸ� �����Ͽ� ���� �������ֱ�
        if (PlayDataManager.Instance.playData.curSlotCount != PlayDataManager.Instance.playData.maxSlotCount)
            Instantiate(CardPanelManager.instance.oriSlot, CardPanelManager.instance.activeSlotContent)
                .GetComponent<CardSlot>().costText.text = cost + " <sprite=0>";
    }

    /// <summary>
    /// ���� Ŭ�� �� ������ �Լ�
    /// </summary>
    public void OnUnequipClk()
    {
        // ������ ī�尡 ���ٸ� ����
        if (transform.childCount == 1)
            return;

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click);

        // ī�� �����ְ�
        Destroy(transform.GetChild(0).gameObject);
        // ���Կ� ���� �� �� �Ѱ��ֱ�
        PlayDataManager.Instance.playData.activedCardIDs[slotIndex] = -1;
    }
}
