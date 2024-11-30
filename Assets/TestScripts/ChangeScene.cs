using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void 알람보내기()
    {
        유튜버.Instance.알람?.Invoke(-999);
    }


    public void OnClick()
    {
        SceneManager.LoadScene("Test2");
    }
}
