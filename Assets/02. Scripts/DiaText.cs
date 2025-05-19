using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiaText : MonoBehaviour
{
    TextMeshProUGUI changeText;

    // Start is called before the first frame update
    void Start()
    {
        changeText = GetComponent<TextMeshProUGUI>();

        PlayDataManager.Instance.onChangedDia += ChangeText;
    }

    void ChangeText(int dia)
    {
        changeText.text = Change.Num(dia);
        //changeText.text = dia.ToString("F0");
    }
}
