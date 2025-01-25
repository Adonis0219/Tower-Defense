using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    public static MainSceneManager instance;

    [SerializeField]
    TextMeshProUGUI coinText;
    
    [SerializeField]
    TextMeshProUGUI diaText;

    [SerializeField]
    TextMeshProUGUI lockText;

    int curDia;

    private void Start()
    {
        instance = this;

        coinText.text = PlayDataManager.Instance.MainCoin.ToString();
        diaText.text = PlayDataManager.Instance.MainDia.ToString();
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

        StartCoroutine(LockTextActive());
    }

    IEnumerator LockTextActive()
    {
        lockText.gameObject.SetActive(true);
        yield return new WaitForSeconds(.1f);
        lockText.gameObject.SetActive(false);
    }

    private void OnApplicationQuit()
    {
        PlayDataManager.Instance.SaveData(PlayDataManager.Instance.playData.haveCoin, PlayDataManager.Instance.playData.haveDia);
    }
}
