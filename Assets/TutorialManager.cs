using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    [SerializeField]
    public GameObject[] tutoArrows;

    [SerializeField]
    public GameObject[] tutoPanels;

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// 튜토리얼이 전부 완수되었는지 검사해주는 함수
    /// </summary>
    public void ChkAllClear()
    {
        for (int i = 0; i < (int)Achive.Length; i++)
        {
            if ((PlayDataManager.Instance.playData.alreadyTuto >> i) % 2 == 0)
                return;
        }

        // 튜토리얼이 전부 완료되었다면 튜토 매니저 파괴
        Destroy(this.gameObject);
    }

    public void OpenTutoPanel(int index)
    {
        tutoArrows[index].gameObject.SetActive(false);

        tutoPanels[index].gameObject.SetActive(true);
    }

    public void OnGetBtClk(int index)
    {
        PlayDataManager.Instance.playData.alreadyTuto |= 1 << index;

        tutoPanels[index].gameObject.SetActive(false);

        switch (index)
        {
            case 0:
                PlayDataManager.Instance.MainCoin += 50;
                break;
            case 1:
                PlayDataManager.Instance.MainDia += 40;
                break;
            case 2:
                PlayDataManager.Instance.MainCoin += 1000;
                break;
            default:
                break;
        }
    }
}
