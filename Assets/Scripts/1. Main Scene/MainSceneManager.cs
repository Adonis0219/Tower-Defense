using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI coinText;

    [SerializeField]
    TextMeshProUGUI lockText;

    private void Start()
    {
        coinText.text = PlayDataManager.Instance.MainCoin.ToString();
    }

    public void OnStartBtClk()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }

    public void OnLockBtClk(int index)
    {
        switch (index)
        {
            case 0:
                lockText.text = "10���̺� �޼� �� �ر�";
                break;
            case 1:
                lockText.text = "100<sprite=12> ���� �� �ر�";
                break;
            case 2:
                lockText.text = "���� ������Ʈ";
                break;
            default:
                break;
        }

        StartCoroutine(lockTextActive());
    }

    IEnumerator lockTextActive()
    {
        lockText.gameObject.SetActive(true);
        yield return new WaitForSeconds(.1f);
        lockText.gameObject.SetActive(false);
    }
}
