using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDText : MonoBehaviour
{
    public enum InfoType { CMHealth, MyHealthRegen, MyDamage, Wave, EnemyHealth, EnemyDamage }

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
                myText.text = GameManager.instance.player.regenHp.ToString("F2") + "/sec";
                break;
            case InfoType.MyDamage:
                myText.text = GameManager.instance.player.Damage.ToString();
                break;
            case InfoType.Wave:
                myText.text = GameManager.instance.Wave + 1 + "¿þÀÌºê";
                break;
            case InfoType.EnemyHealth:
                break;
            case InfoType.EnemyDamage:
                break;
            default:
                break;
        }
    }
}
