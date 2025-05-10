using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDText : MonoBehaviour
{
    public enum InfoType { CMHealth, MyHealthRegen, MyDamage, Wave, EnemyHealth, EnemyDamage, BossCMHealth }

    public InfoType type;

    TextMeshProUGUI myText;

    // Start is called before the first frame update
    void Awake()
    {
        myText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case InfoType.CMHealth:
                myText.text = GameManager.instance.player.CurrentHp.ToString("F0") + "  /  " + GameManager.instance.player.MaxHp.ToString("F0");
                break;
            case InfoType.MyHealthRegen:
                myText.text = GameManager.instance.player.RegenHp.ToString("F2") + "/sec";
                break;
            case InfoType.MyDamage:
                myText.text = GameManager.instance.player.Damage.ToString();
                break;
            case InfoType.Wave:
                myText.text = GameManager.instance.Wave + "¿þÀÌºê";
                break;
            case InfoType.EnemyHealth:
                myText.text = GameManager.instance.waveHpFactor.ToString("F2");
                break;
            case InfoType.EnemyDamage:
                myText.text = GameManager.instance.waveDmgFactor.ToString("F2");
                break;
            case InfoType.BossCMHealth:
                myText.text = GameManager.instance.boss.CurrentHp.ToString("F0") + "/" + GameManager.instance.boss.maxHp.ToString("F0");
                break;
            default:
                break;
        }
    }
}
