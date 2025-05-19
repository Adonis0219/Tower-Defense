using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockBt : MUpgradeButton
{
    int myType;
    int createCount;

    [SerializeField]
    Image unlockBtImg;

    public void SetUnlockData(string name, int cost, int createCount, int myType)
    {
        upNameText.text = name + " 업그레이드 잠금 해제";
        this.createCount = createCount;
        this.myType = myType;

        if (cost == 1234567)
        {
            costText.text = "<color=red>추후 업데이트 예정!</color>";
        }
        else
        {
            costText.text = "<sprite=12>" + Change.Num(cost);
        }
        upCost = cost;
    }

    private void Update()
    {
        Color baseColor = unlockBtImg.color;

        if (PlayDataManager.Instance.MainCoin > upCost)
        {
            bt.interactable = true;
            baseColor.a = 0;
        }
        else
        {
            bt.interactable = false;
            baseColor.a = .4f;
        }

        unlockBtImg.color = baseColor;
    }

    public void OnClick()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.UnlockClk);

        // 비용만큼 차감
        PlayDataManager.Instance.MainCoin -= upCost;

        // 언락 오픈 카운트 올리기
        PlayDataManager.Instance.playData.lineOpenCounts[myType]++;
        // 만들어야 할 개수 늘려주기
        PlayDataManager.Instance.playData.totalCreatCounts[myType] += createCount;

        // 할 행동들을 다 끝내고 사라지기
        Destroy(this.gameObject);
        DestroyImmediate(this.transform.parent.gameObject);

        MPanelManager.instance.OnUnlockClickCreate(myType, createCount);
    }
}
