using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinText : MonoBehaviour
{
    TextMeshProUGUI changeText;

    // Start is called before the first frame update
    void Start()
    {
        changeText = GetComponent<TextMeshProUGUI>();

        PlayDataManager.Instance.onChangedCoin += ChangeText;
    }

    void ChangeText(int coin)
    {
        changeText.text = coin.ToString("F0");
    }
}
