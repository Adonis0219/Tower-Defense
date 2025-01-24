using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LabButton : MonoBehaviour
{
    /// <summary>
    /// ������ �������ΰ�? Empty -> ����ִ�
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
    /// �� �������� Ŭ�� ���� �� ������ �Լ�
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
