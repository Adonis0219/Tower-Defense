using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LabButton : MonoBehaviour
{
    /// <summary>
    /// 연구가 실행중인가? Empty -> 비어있다
    /// </summary>
    bool isEmpty = true;

    public int labIndex;

    [HideInInspector]
    public float requireTime;
    [HideInInspector]
    public float remainTime;

    [SerializeField]
    public TextMeshProUGUI nameNLevelText;
    [SerializeField]
    public TextMeshProUGUI upInfoText;
    [SerializeField]
    public TextMeshProUGUI remainTimeText;

    [SerializeField]
    Slider mySlider;

    /// <summary>
    /// 빈 연구실을 클릭 했을 때 실행할 함수
    /// </summary>
    public void EmptyLabClick(GameObject researchListPN)
    {
        researchListPN.SetActive(true);
        LabManager.instance.clickedIndex = labIndex;
    }

    private void Update()
    {
        if (!transform.GetChild(2).gameObject.activeSelf)
            return;

        remainTime -= Time.deltaTime;

        remainTimeText.text = LabManager.instance.DisplayTime(remainTime);

        mySlider.value = 1 - (remainTime / requireTime);
    }   
}
