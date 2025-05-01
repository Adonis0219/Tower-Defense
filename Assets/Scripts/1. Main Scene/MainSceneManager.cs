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

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        coinText.text = PlayDataManager.Instance.MainCoin.ToString();
        diaText.text = PlayDataManager.Instance.MainDia.ToString();

        AudioManager.instance.PlayBgm(true);
    }

    public void OnStartBtClk()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.GameStart);

        StartCoroutine(Wait_Start());
    }

    IEnumerator Wait_Start()
    {
        yield return new WaitForSeconds(0.7f);

        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }

    public void OnLockBtClk(int index)
    {
        switch (index)
        {
            case 0:
                lockText.text = "10웨이브 달성 시 해금";
                break;
            case 1:
                lockText.text = "100<sprite=12> 얻을 시 해금";
                break;
            case 2:
                lockText.text = "추후 업데이트";
                break;
            default:
                break;
        }

        AudioManager.instance.PlaySfx(AudioManager.Sfx.NoClk);

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
        PlayDataManager.Instance.SaveData(PlayDataManager.Instance.MainCoin, PlayDataManager.Instance.MainDia);
    }
}
