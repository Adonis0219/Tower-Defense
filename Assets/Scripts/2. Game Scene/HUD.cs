using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Health, Wave }

    public InfoType type;

    Slider mySlider;
    TextMeshProUGUI myText;

    // Start is called before the first frame update
    void Awake()
    {
        mySlider = GetComponent<Slider>();
        myText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case InfoType.Health:
                float curHealth = GameManager.instance.player.CurrentHp;
                float maxHealth = GameManager.instance.player.MaxHp;
                mySlider.value = curHealth / maxHealth;
                break;
            case InfoType.Wave:
                mySlider.value = GameManager.instance.gameTime / GameManager.instance.WaveTime;
                break;
            default:
                break;
        }
    }
}
