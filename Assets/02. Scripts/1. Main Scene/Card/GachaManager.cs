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
    public Image backgroundImg;
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
    public Image backgroundImg;
}

public class GachaManager : MonoBehaviour
{
    [SerializeField]
    CardData[] cardDatas = new CardData[10];

    /// <summary>
    /// 10뽑의 최종 결과창에 나오는 10개의 카드들
    /// </summary>
    [SerializeField]
    CardUI[] resultCardUIs;

    [SerializeField]
    GachaUI[] gachaUIs = new GachaUI[2];

    [SerializeField]
    int[] reqCardCount;

    [SerializeField]
    public Sprite[] starSprites;


    [Header("# 가챠 버튼")]
    [SerializeField]
    Button gachaBt1;
    [SerializeField]
    Button gachaBt10;

    [Header("# 가챠 체크")]
    [SerializeField]
    GameObject gachaChkPN;
    [SerializeField]
    TextMeshProUGUI gachaChkDesc;

    [Header("# 가챠 결과 판넬")]
    [SerializeField]
    GameObject gachaResultPN;
    [SerializeField]
    GameObject gachaResultPN10;

    [SerializeField]
    GameObject totalResultPN;

    int gachaCount;

    void Update()
    {
        // 가챠 버튼들 보유 다이아에 따라 활성화 조정
        gachaBt1.interactable = PlayDataManager.Instance.MainDia >= 20 ? true : false;
        gachaBt10.interactable = PlayDataManager.Instance.MainDia >= 200 ? true : false;
    }

    /// <summary>
    /// 단차 뽑기
    /// </summary>
    void OneGacha()
    {
        // 가챠 UI 설정해주기
        GachaUI oneGacha = gachaUIs[0];

        // 랜덤 카드 설정해주기
        Card card = CardManager.instance.RandomCard();

        SetCard(card, oneGacha);
    }

    /// <summary>
    /// 10뽑에서 1개 뽑기
    /// </summary>
    /// <param name="index"></param>
    void OneGachaInTen(int index)
    {
        GachaUI tenGacha = gachaUIs[1];

        Card card = CardManager.instance.RandomCard();

        SetCard(card, tenGacha);

        // 뽑은 카드 데이터에 넣어주기
        cardDatas[index] = card.MyData;

        gachaCount--;
    }

    /// <summary>
    /// 카드 뽑기 시 카드 세팅
    /// </summary>
    /// <param name="card"></param>
    /// <param name="UI"></param>
    void SetCard(Card card, GachaUI UI)
    {
        // 카드를 얻은 적이 없다면 NEW 게임오브젝트 활성화
        UI.newGO.SetActive(!card.IsGet);

        // 얻은 적 없는 카드였다면      
        if (!card.IsGet)
            card.IsGet = true;

        CardData data = card.MyData;

        // 카드 UI 정보 넣어주기
        UI.nameText.text = data.cardName;
        UI.rarityText.text = data.rarity.ToString();
        UI.DescText.text = data.cardDesc + data.value[data.curLv];

        // 카드를 얻었을 때 업그레이드 가능하면 띄워주기
        UI.upGO.SetActive(card.CurCardCount + 1 == reqCardCount[data.curLv]);

        // 카드 개수 더해주고 출력
        card.CurCardCount++;
        UI.cur_nextText.text = data.curCardCount + "/" + reqCardCount[data.curLv];

        // 나머지 UI 정보들 넣어주기
        UI.icon.sprite = data.cardIcon;
        UI.starImg.sprite = starSprites[data.curLv];
        UI.backgroundImg.color = CardManager.instance.rarityColors[(int)data.rarity];
    }

    /// <summary>
    /// 10뽑 결과창 설정 함수
    /// </summary>
    void SetTotalResultPN()
    {
        gachaResultPN10.SetActive(false);
        Set10Card();
        totalResultPN.SetActive(true);
    }
    
    /// <summary>
    /// 10뽑 카드 데이터를 바탕으로 카드 UI 변경해주기
    /// </summary>
    public void Set10Card()
    {
        for (int i = 0; i < 10; i++)
        {
            CardInitSet(i);
        }
    }

    /// <summary>
    /// 10개의 카드들 각각 초기 설정
    /// </summary>
    /// <param name="index"></param>
    void CardInitSet(int index)
    {
        CardUI card = resultCardUIs[index];
        CardData data = cardDatas[index];

        card.nameText.text = data.cardName;
        card.icon.sprite = data.cardIcon;
        card.starImg.sprite = starSprites[data.curLv];
        card.backgroundImg.color = CardManager.instance.rarityColors[(int)data.rarity];
    }

    /// <summary>
    /// 각 결과 판넬 창의 다이아 개수 설정해주는 함수
    /// </summary>
    void SetDia()
    {
        if (gachaCount == 1)
        {
            // 가챠 결과창 켜주기
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
    /// 가차 버튼 클릭
    /// </summary>
    /// <param name="count">연차</param>
    public void OnGachaBtClk(int count)
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click);

        // 가챠 횟수 정해주기
        gachaCount = count;

        // 가챠 체크 판넬 켜주기
        gachaChkPN.SetActive(true);
        gachaChkDesc.text = 20 * gachaCount + "<sprite=0> 사용하여\n카드 " + gachaCount + "개를 뽑으시겠습니까?";
    }

    /// <summary>
    /// 가차 체크 판넬에서 확인 버튼 눌렀을 때
    /// </summary>
    /// <param name="isReGacha">재뽑기 버튼인가?</param>
    public void OnYesClk(bool isReGacha)
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.OkClk);

        PlayDataManager.Instance.MainDia -= 20 * gachaCount;

        // 재뽑기가 아닐 때만
        if (!isReGacha)
        {
            // 가챠 체크 꺼주기
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
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.NoClk);

        gachaChkPN.SetActive(false);
    }

    public void OnNextClk()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click);

        if (gachaCount != 0)
            OneGachaInTen(10 - gachaCount);
        else SetTotalResultPN();
    }

    /// <summary>
    /// 10뽑 스킵 버튼 클릭 시
    /// </summary>
    public void OnSkipClk()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click);

        int lastCount = gachaCount;

        // 남은 뽑기 횟수만큼 뽑아주고
        while (gachaCount != 0)
            OneGachaInTen(10 - gachaCount);

        // 10뽑 결과 판넬 띄워주기
        SetTotalResultPN();
    }

    public void OnGetClk(GameObject gameObject)
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.GetClk);

        gameObject.SetActive(false);
    }
}
