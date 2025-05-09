using System.Collections;
using TMPro;
using Unity.VisualScripting;
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
        coinText.text = Change.Num(PlayDataManager.Instance.MainCoin);
        diaText.text = Change.Num(PlayDataManager.Instance.MainDia);

        AudioManager.Instance.PlayBgm(SceneType.Main);
    }

    public void OnStartBtClk()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.GameStart);

        //// �ؽ�Ʈ ���� ����
        //if (coru != null) StopCoroutine(coru);

        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }

    Coroutine coru;

    public void OnLockBtClk(int index)
    {
        switch (index)
        {
            case 0:
                lockText.text = "���� �� �� ���� �� �ر�";
                break;
            case 1:
                lockText.text = "100<sprite=12> ���� �� �ر�";
                break;
            case 2:
                lockText.text = "80���̺� �޼� �� �ر�";
                break;
            default:
                break;
        }

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.NoClk);
        
        coru = StartCoroutine(LockTextActive());
    }

    IEnumerator LockTextActive()
    {
        lockText.gameObject.SetActive(true);
        yield return new WaitForSeconds(.2f);
        lockText.gameObject.SetActive(false);
    }

    private void OnApplicationQuit()
    {
        PlayDataManager.Instance.SaveData(PlayDataManager.Instance.MainCoin, PlayDataManager.Instance.MainDia);
    }
}
