using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestP : MonoBehaviour
{
    [SerializeField]
    float damage;

    [SerializeField]
    float currentHp;

    public float CurrentHp
    {
        get 
        {
            currentHp += 1000;

            return currentHp; 
        }
        set
        {
            currentHp = value;

            hpText.text = CurrentHp.ToString();

            // ����ü���� 0���Ϸ� �������� ���
            if (CurrentHp <= 0)
                Dead();
        }
    }

    [SerializeField]
    TextMeshProUGUI hpText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CurrentHp -= damage;
        }
    }

    void Dead()
    {
        Debug.Log("�׾���!");
    }
}
