using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Health, Wave }

    public InfoType type;

    Slider mySlider;
    [SerializeField]
    Image fill;
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
                // 웨이브 사이 시간이 아니라면
                if (!GameManager.instance.IsWait)
                {
                    fill.color = new Color(.4f, .35f, 1);
                    mySlider.value = GameManager.instance.gameTime / GameManager.instance.WaveTime;
                }
                else
                {
                    fill.color = Color.yellow;
                    mySlider.value = GameManager.instance.gameTime / GameManager.instance.waitTime;
                }
                break;
            default:
                break;
        }
    }
}
