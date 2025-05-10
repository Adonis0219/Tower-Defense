using Mono.Cecil.Pdb;
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

    [SerializeField]
    TextMeshProUGUI adminText;

    [SerializeField]
    GameObject admin;

    int curDia;

    private void Awake()
    { 
        instance = this;
    }

    private void Start()
    {
        // 게임 다시 실행되도록
        Time.timeScale = 1;

        coinText.text = Change.Num(PlayDataManager.Instance.MainCoin);
        diaText.text = Change.Num(PlayDataManager.Instance.MainDia);

        AudioManager.Instance.PlayBgm(SceneType.Main);

        admin.SetActive(PlayDataManager.Instance.playData.isAdmin);
    }

    public void OnStartBtClk()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.GameStart);

        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }

    [VisibleEnum(typeof(MainPanelType))]
    public void OnLockBtClk(int index)
    {
        lockText.gameObject.SetActive(false);

        MainPanelType type = (MainPanelType)index;

        switch (type)
        {
            case MainPanelType.Battle:
                break;
            case MainPanelType.Workshop:
                lockText.text = "게임 한 번 실행 시 해금";
                break;
            case MainPanelType.Lab:
                lockText.text = "10 웨이브 도달 시 해금";
                break;
            case MainPanelType.Cards:
                lockText.text = "30 웨이브 도달 시 해금";
                break;
            default:
                break;
        }

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.NoClk);
        
        StartCoroutine(LockTextActive());
    }

    int adminCount;
    Coroutine adCoru;

    public int AdminCount
    {
        get
        {
            return adminCount; 
        }   
        set
        {
            adminCount = value;

            if (adCoru != null) return;

            adCoru = StartCoroutine(AdminCoru());
        }
    }

    /// <summary>
    /// 5초 안에 버튼을 10번 클릭해야 하도록 해주는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator AdminCoru()
    {
        yield return new WaitForSeconds(5);

        AdminCount = 0;

        adCoru = null;
    }

    /// <summary>
    /// 어드민 버튼을 누를 때 함수
    /// </summary>
    public void OnAdminClk()
    {
        if (AdminCount < 10)
        {
            AdminCount++;
        }
        else
        {
            AdminCount = 0;

            AdminModChange();
        }
    }

    /// <summary>
    /// 어드민 모드를 실행시키는 함수
    /// </summary>
    void AdminModChange()
    {
        PlayDataManager.Instance.playData.isAdmin = !PlayDataManager.Instance.playData.isAdmin;

        adminText.text = "어드민 권한 : " + PlayDataManager.Instance.playData.isAdmin;

        if (PlayDataManager.Instance.playData.isAdmin)
            PlayDataManager.Instance.SaveData(9999999, 9999999);
        else
            PlayDataManager.Instance.playData = new PlayData();

        StartCoroutine(Admin());
    }

    /// <summary>
    /// 어드민 실행 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator Admin()
    {
        adminText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1);

        // 새로 씬 로드
        SceneManager.LoadScene(0);
    }

    IEnumerator LockTextActive()
    {
        lockText.gameObject.SetActive(true);

        yield return new WaitForSeconds(.2f);

        lockText.gameObject.SetActive(false);
    }

    private void OnApplicationQuit()
    {
        // 어드민 권한 해제
        PlayDataManager.Instance.playData.isAdmin = false;

        PlayDataManager.Instance.SaveData(PlayDataManager.Instance.MainCoin, PlayDataManager.Instance.MainDia);
    }
}
