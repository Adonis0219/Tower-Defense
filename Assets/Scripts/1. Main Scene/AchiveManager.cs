using UnityEngine;

public enum Achive
{
    UnlockWorkShop, 
    UnlockLabs,
    UnlockCards,
    Length
}

// ������� ���ǵ�
public class UnlockConditions
{
    /// <summary>
    /// �� ���ϸ� ����
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
            // ���� ������ �޼��ƴٸ� �ǳ� ����
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
