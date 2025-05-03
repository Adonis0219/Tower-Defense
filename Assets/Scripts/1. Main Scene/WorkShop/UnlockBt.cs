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
        upNameText.text = name + " ���׷��̵� ��� ����";
        this.createCount = createCount;
        this.myType = myType;

        if (cost == 1234567)
        {
            costText.text = "<color=red>���� ������Ʈ ����!</color>";
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

        // ��븸ŭ ����
        PlayDataManager.Instance.MainCoin -= upCost;

        // ��� ���� ī��Ʈ �ø���
        PlayDataManager.Instance.playData.lineOpenCounts[myType]++;
        // ������ �� ���� �÷��ֱ�
        PlayDataManager.Instance.playData.totalCreatCounts[myType] += createCount;

        // �� �ൿ���� �� ������ �������
        Destroy(this.gameObject);
        DestroyImmediate(this.transform.parent.gameObject);

        MPanelManager.instance.OnUnlockClickCreate(myType, createCount);
    }
}
