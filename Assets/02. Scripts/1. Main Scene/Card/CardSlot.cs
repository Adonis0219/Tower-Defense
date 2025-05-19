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
        // 현재 장착된 카드가 없다면 리턴
        if (transform.childCount == 0)
            return;
    }

    /// <summary>
    /// 잠긴 카드 슬롯을 오픈할 때 실행할 함수
    /// </summary>
    public void OnSlotOpenClk()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.UnlockClk);

        // 슬롯 인덱스 설정
        slotIndex = PlayDataManager.Instance.playData.curSlotCount;
        // 가격 설정
        int cost = PlayDataManager.Instance.playData.slotOpneCost[slotIndex - 1];

        // 다이아 부족 시 얼리 리턴
        if (PlayDataManager.Instance.MainDia < cost)
            return;
        
        PlayDataManager.Instance.MainDia -= cost;

        // 열린 슬롯만 드롭 가능하도록
        gameObject.AddComponent<DroppableUI>();

        // DnD를 편하게 해주기 위해 열린 슬롯이 줄어들 일이 없으니
        // 언락 판넬 삭제
        Destroy(transform.GetChild(0).gameObject);

        PlayDataManager.Instance.playData.curSlotCount++;

        // 장비/해제 버튼 켜주기
        // (한 프레임 내에 실행되어 아직 0번째 자식이 사라지기 전이므로 1)
        transform.GetChild(1).gameObject.SetActive(true);

        // 현재 오픈된 슬롯 개수가 바뀌었으므로 비용 다시 설정
        cost = PlayDataManager.Instance.playData.slotOpneCost[slotIndex - 1];

        // 최대 개수가 아니면 복제하여 가격 설정해주기
        if (PlayDataManager.Instance.playData.curSlotCount != PlayDataManager.Instance.playData.maxSlotCount)
            Instantiate(CardPanelManager.instance.oriSlot, CardPanelManager.instance.activeSlotContent)
                .GetComponent<CardSlot>().costText.text = cost + " <sprite=0>";
    }

    /// <summary>
    /// 해제 클릭 시 실행할 함수
    /// </summary>
    public void OnUnequipClk()
    {
        // 장착된 카드가 없다면 리턴
        if (transform.childCount == 1)
            return;

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click);

        // 카드 지워주고
        Destroy(transform.GetChild(0).gameObject);
        // 슬롯에 장착 안 됨 넘겨주기
        PlayDataManager.Instance.playData.activedCardIDs[slotIndex] = -1;
    }
}
