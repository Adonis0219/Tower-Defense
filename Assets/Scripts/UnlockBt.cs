using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockBt : MUpgradeButton
{
    int myType;
    int createCount;

    public void SetData(string name, int cost, int createCount, int myType)
    {
        upNameText.text = name + " 업그레이드 잠금 해제";
        this.createCount = createCount;
        this.myType = myType;

        costText.text = "<sprite=12>" + cost;
    }

    public void OnClick()
    {
        // 없어지고
        Destroy(this.gameObject);

        PlayDataManager.Instance.playData.openCounts[myType]++;
        PlayDataManager.Instance.playData.createCounts[myType] += createCount;
    }
}
