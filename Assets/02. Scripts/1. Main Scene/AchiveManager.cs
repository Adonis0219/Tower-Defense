using UnityEngine;

public enum Achive
{
    UnlockWorkShop, 
    UnlockCards,
    UnlockLabs,
    Length
}

// 잠금해제 조건들
public class UnlockConditions
{
    /// <summary>
    /// 한 판하면 열림
    /// </summary>
    public const int BEST_WAVE_WORKSHOP = 0;
    public const int BEST_WAVE_CARD = 0;
    //public const int BEST_WAVE_CARD = 10;
    public const float BEST_WAVE_LAB = 0;
    //public const float BEST_WAVE_LAB = 30;
}

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockPanels;
    public GameObject[] unlockPanels;


    private void Awake()
    {
        for (int i = 0; i < (int)Achive.Length; i++)
        {
            // 만약 업적이 달성됐다면 판넬 해제
            if ((PlayDataManager.Instance.playData.achive >> i) % 2 == 1)
                OpenPanel(i);
        }
    }

    void OpenPanel(int index)
    {
        lockPanels[index].SetActive(false);
        unlockPanels[index].SetActive(true);

        ChkTuto(index);
    }

    void ChkTuto(int index)
    {
        // 이미 해당 튜토리얼을 완료 했다면 얼리
        if ((PlayDataManager.Instance.playData.alreadyTuto >> index) % 2 == 1)
            return;

        TutorialManager.instance.tutoArrows[index].SetActive(true);
    }
}
