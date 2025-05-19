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
        // ���� �ٽ� ����ǵ���
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
                lockText.text = "���� �� �� ���� �� �ر�";
                break;
            case MainPanelType.Lab:
                lockText.text = "10 ���̺� ���� �� �ر�";
                break;
            case MainPanelType.Cards:
                lockText.text = "30 ���̺� ���� �� �ر�";
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
    /// 5�� �ȿ� ��ư�� 10�� Ŭ���ؾ� �ϵ��� ���ִ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator AdminCoru()
    {
        yield return new WaitForSeconds(5);

        AdminCount = 0;

        adCoru = null;
    }

    /// <summary>
    /// ���� ��ư�� ���� �� �Լ�
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
    /// ���� ��带 �����Ű�� �Լ�
    /// </summary>
    void AdminModChange()
    {
        PlayDataManager.Instance.playData.isAdmin = !PlayDataManager.Instance.playData.isAdmin;

        adminText.text = "���� ���� : " + PlayDataManager.Instance.playData.isAdmin;

        if (PlayDataManager.Instance.playData.isAdmin)
            PlayDataManager.Instance.SaveData(9999999, 9999999);
        else
            PlayDataManager.Instance.playData = new PlayData();

        StartCoroutine(Admin());
    }

    /// <summary>
    /// ���� ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator Admin()
    {
        adminText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1);

        // ���� �� �ε�
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
        // ���� ���� ����
        PlayDataManager.Instance.playData.isAdmin = false;

        PlayDataManager.Instance.SaveData(PlayDataManager.Instance.MainCoin, PlayDataManager.Instance.MainDia);
    }
}
