using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void �˶�������()
    {
        ��Ʃ��.Instance.�˶�?.Invoke(-999);
    }


    public void OnClick()
    {
        SceneManager.LoadScene("Test2");
    }
}
