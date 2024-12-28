using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockBt : MUpgradeButton
{
    int myType;
    int createCount;

    public void SetData(string name, int cost, int createCount, int myType)
    {
        upNameText.text = name + " ���׷��̵� ��� ����";
        this.createCount = createCount;
        this.myType = myType;

        costText.text = "<sprite=12>" + cost;
    }

    public void OnClick()
    {
        // ��������
        Destroy(this.gameObject);

        PlayDataManager.Instance.playData.openCounts[myType]++;
        PlayDataManager.Instance.playData.createCounts[myType] += createCount;
    }
}
