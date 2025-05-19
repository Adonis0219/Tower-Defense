using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInfoPanel : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI rarityText;
    [SerializeField]
    public TextMeshProUGUI descText;
    [SerializeField]
    public TextMeshProUGUI lv_ValueText;

    [SerializeField]
    public TextMeshProUGUI cardName;
    [SerializeField]
    public Image cardIcon;

    [SerializeField]
    public Image starImg;

    public void InitSet(CardData myData, Sprite star )
    {
        cardName.text = myData.cardName;
        cardIcon.sprite = myData.cardIcon;

        rarityText.text = myData.rarity.ToString();
        descText.text = myData.cardDesc + myData.value[myData.curLv];

        starImg.sprite = star;

        // 텍스트 초기화
        lv_ValueText.text = "";

        for (int i = 0; i < myData.MaxLv + 1; i++)
        {
            if (i == myData.curLv)
            {
                lv_ValueText.text += "<color=white>Lv." + (i + 1) +"</color>" + "     <color=#56FFF9>x" + myData.value[i].ToString("F2") + "</color>\n";
            }
            else
            {
                lv_ValueText.text += "Lv." + (i + 1) + "     x" + myData.value[i].ToString("F2") + "\n";
            }
        }
    }

    public void OnCloseClk()
    {
        this.gameObject.SetActive(false);
    }
}
