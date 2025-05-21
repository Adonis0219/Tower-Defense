using UnityEngine;

public enum Achive
{
    UnlockWorkShop, 
    UnlockCards,
    UnlockLabs,
    Length
}

// ������� ���ǵ�
public class UnlockConditions
{
    /// <summary>
    /// �� ���ϸ� ����
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
            // ���� ������ �޼��ƴٸ� �ǳ� ����
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
        // �̹� �ش� Ʃ�丮���� �Ϸ� �ߴٸ� ��
        if ((PlayDataManager.Instance.playData.alreadyTuto >> index) % 2 == 1)
            return;

        TutorialManager.instance.tutoArrows[index].SetActive(true);
    }
}
