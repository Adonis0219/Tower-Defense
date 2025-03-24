using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    CardData myData;

    public CardData MyData
    {
        get { return myData; }

        set 
        { 
            myData = value;
            InitSet();
        }
    }
    public int[] reqCardCount;      // 카드 레벨업을 위해 필요한 카드 개수
    public int[] reqDia;            // 카드 레벨업에 필요한 다이아 개수

    [SerializeField]
    public TextMeshProUGUI nameText;
    [SerializeField]
    public TextMeshProUGUI descText;
    [SerializeField]
    public Image icon;
    [SerializeField]
    public bool isOpen = false;

    private void OnEnable()
    {
        isOpen = true;
        //GetCard();
    }

    void InitSet()
    {
        nameText.text = MyData.cardName;
        //descText.text = MyData.cardDesc;
        icon.sprite = MyData.cardIcon;
    }

    /// <summary>
    /// 카드를 획득할 때 실행할 함수
    /// </summary>
    void GetCard()
    {
        isOpen = true;
        // 카드를 얻고 나면 다시 잠글 일이 없으므로 그냥 지워주기
        DestroyImmediate(transform.GetChild(0).gameObject);
        // 상위에 있던 자식(0)이 사라졌으므로 자신이 다시 0이 된다
        // 카드 인포 켜주기
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
