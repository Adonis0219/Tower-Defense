using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CardUI
{
    public Transform card;
    public TextMeshProUGUI nameText;
    public Image icon;
    public Image starImg;
}

[System.Serializable]
public class GachaUI
{

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI cur_nextText;
    public TextMeshProUGUI DescText;

    public GameObject newGO;
    public GameObject upGO;

    public Image icon;
    public Image starImg;
}

public class GachaManager : MonoBehaviour
{
    [SerializeField]
    CardData[] cardDatas = new CardData[10];

    [SerializeField]
    CardUI[] cards;

    [SerializeField]
    GachaUI[] gachaUIs = new GachaUI[2];

    [SerializeField]
    int[] reqCardCount;

    [SerializeField]
    public Sprite[] starSprites;


    [Header("# ��í ��ư")]
    [SerializeField]
    Button gachaBt1;
    [SerializeField]
    Button gachaBt10;

    [Header("# ��í üũ")]
    [SerializeField]
    GameObject gachaChkPN;
    [SerializeField]
    TextMeshProUGUI gachaChkDesc;

    [Header("# ��í ��� �ǳ�")]
    [SerializeField]
    GameObject gachaResultPN;
    [SerializeField]
    GameObject gachaResultPN10;

    [SerializeField]
    GameObject totalResultPN;

    int gachaCount;

    void Update()
    {
        gachaBt1.interactable = PlayDataManager.Instance.MainDia >= 20 ? true : false;
        gachaBt10.interactable = PlayDataManager.Instance.MainDia >= 200 ? true : false;
    }

    /// <summary>
    /// ���� �̱�
    /// </summary>
    void OneGacha()
    {
        GachaUI oneGacha = gachaUIs[0];

        Card card = CardManager.instance.RandomCard();

        SetCard(card, oneGacha);
    }

    /// <summary>
    /// 10�̿��� 1�� �̱�
    /// </summary>
    /// <param name="index"></param>
    void OneGachaInTen(int index)
    {
        GachaUI tenGacha = gachaUIs[1];

        Card card = CardManager.instance.RandomCard();

        SetCard(card, tenGacha);

        // ���� ī�� �����Ϳ� �־��ֱ�
        cardDatas[index] = card.MyData;

        gachaCount--;
    }

    /// <summary>
    /// ī�� �̱� �� ī�� ����
    /// </summary>
    /// <param name="card"></param>
    /// <param name="UI"></param>
    void SetCard(Card card, GachaUI UI)
    {
        UI.newGO.SetActive(!card.IsGet);

        // ���� �� ���� ī�忴�ٸ�      
        if (!card.IsGet)
            card.IsGet = true;

        CardData data = card.MyData;

        UI.nameText.text = data.cardName;
        UI.rarityText.text = data.rarity.ToString();
        UI.DescText.text = data.cardDesc + data.value[data.curLv];

        // ī�带 ����� �� ���׷��̵� �����ϸ� ����ֱ�
        UI.upGO.SetActive(card.CurCardCount + 1 == reqCardCount[data.curLv]);

        // ī�� ���� �����ְ� ���
        card.CurCardCount++;

        UI.cur_nextText.text = data.curCardCount + "/" + reqCardCount[data.curLv];

        UI.icon.sprite = data.cardIcon;
        UI.starImg.sprite = starSprites[data.curLv];
    }

    /// <summary>
    /// 10�� ���â ���� �Լ�
    /// </summary>
    void SetTotalResultPN()
    {
        gachaResultPN10.SetActive(false);
        Set10Card();
        totalResultPN.SetActive(true);
    }
    
    /// <summary>
    /// 10�� ī�� �����͸� �������� ī�� UI �������ֱ�
    /// </summary>
    public void Set10Card()
    {
        for (int i = 0; i < 10; i++)
        {
            CardInitSet(i);
        }
    }

    void CardInitSet(int index)
    {
        CardUI card = cards[index];
        CardData data = cardDatas[index];

        card.nameText.text = data.cardName;
        card.icon.sprite = data.cardIcon;
        card.starImg.sprite = starSprites[data.curLv];
    }

    /// <summary>
    /// �� ��� �ǳ� â�� ���̾� ���� �������ִ� �Լ�
    /// </summary>
    void SetDia()
    {
        if (gachaCount == 1)
        {
            // ��í ���â ���ֱ�
            gachaResultPN.SetActive(true);
            gachaResultPN.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = PlayDataManager.Instance.MainDia + "<sprite=0>";
        }
        else
        {
            gachaResultPN10.SetActive(true);
            gachaResultPN10.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = PlayDataManager.Instance.MainDia + "<sprite=0>";
        }
    }

    /// <summary>
    /// ���� ��ư Ŭ��
    /// </summary>
    /// <param name="count">����</param>
    public void OnGachaBtClk(int count)
    {
        // ��í Ƚ�� �����ֱ�
        gachaCount = count;

        // ��í üũ �ǳ� ���ֱ�
        gachaChkPN.SetActive(true);
        gachaChkDesc.text = 20 * gachaCount + "<sprite=0> ����Ͽ�\nī�� " + gachaCount + "���� �����ðڽ��ϱ�?";
    }

    /// <summary>
    /// ���� üũ �ǳڿ��� Ȯ�� ��ư ������ ��
    /// </summary>
    /// <param name="isReGacha">��̱� ��ư�ΰ�?</param>
    public void OnYesClk(bool isReGacha)
    {
        PlayDataManager.Instance.MainDia -= 20 * gachaCount;

        // ��̱Ⱑ �ƴ� ����
        if (!isReGacha)
        {
            // ��í üũ ���ֱ�
            gachaChkPN.SetActive(false);
        }

        SetDia();

        if (gachaCount == 1)
            OneGacha();
        else
            OneGachaInTen(10 - gachaCount);
    }

    public void OnNoClk()
    {
        gachaChkPN.SetActive(false);
    }

    public void OnNextClk()
    {
        if (gachaCount != 0)
            OneGachaInTen(10 - gachaCount);
        else SetTotalResultPN();
    }

    public void OnSkipClk()
    {
        int lastCount = gachaCount;

        // ���� �̱� Ƚ����ŭ �̾��ְ�
        while (gachaCount != 0)
            OneGachaInTen(10 - gachaCount);

        // 10�� ��� �ǳ� ����ֱ�
        SetTotalResultPN();
    }

    public void OnGetClk(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
