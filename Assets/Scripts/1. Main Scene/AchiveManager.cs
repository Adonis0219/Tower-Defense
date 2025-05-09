using UnityEngine;

public enum Achive
{
    UnlockWorkShop, 
    UnlockLabs,
    UnlockCards,
    Length
}

// 잠금해제 조건들
public class UnlockConditions
{
    /// <summary>
    /// 한 판하면 열림
    /// </summary>
    public const int BEST_WAVE_WORKSHOP = 0;
    public const float LAB_OPEN_COIN = 100;
    public const int BEST_WAVE_CARD = 80;
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
    }
}
