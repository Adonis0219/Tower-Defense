using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LabButton : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI labNameText;

    [Header("# Upgrade")]
    [SerializeField]
    public TextMeshProUGUI nameNLevelText;
    [SerializeField]
    public TextMeshProUGUI upInfoText;
    [SerializeField]
    public TextMeshProUGUI remainTimeText;

    [SerializeField]
    Slider mySlider;

    [Header("# Unlock")]
    [SerializeField]
    TextMeshProUGUI unlockText;

    [SerializeField]
    bool isEmpty = true;
    [SerializeField]
    bool isOpen = false;

    /// <summary>
    /// ������ �������ΰ�? Empty -> ����ִ�
    /// </summary>
    public bool IsEmpty
    {
        get { return isEmpty; }
        set
        {
            isEmpty = value;
            LabBtSet();
        }
    }

    public bool IsOpen
    {
        get { return isOpen; }
        set
        {
            isOpen = value;
            LabBtSet();
        }
    }

    public int labIndex;

    [HideInInspector]
    public float requireTime;
    [HideInInspector]
    public float remainTime;

    private void Start()
    {
        labNameText.text = "Lab " + (labIndex + 1);

        // ������ �������� �ƴϰ� && ��� �������̶��
        if (!isOpen)
        {
            unlockText.text = (labIndex + 1) + "��° ������������\n" + LabManager.instance.labOpenCost[labIndex] + "<sprite=0>";
        }
    }

    private void Update()
    {
        if (!transform.GetChild(2).gameObject.activeSelf && remainTime < 0)
        {
            remainTime = 0;
            return;
        }

        remainTime -= Time.deltaTime;

        remainTimeText.text = LabManager.instance.DisplayTime(remainTime);

        mySlider.value = 1 - (remainTime / requireTime);
    }

    void LabBtSet()
    {
        if (isOpen)
        {
            // �� ������
            transform.GetChild(1).gameObject.SetActive(IsEmpty ? true : false);
            // ���׷��̵���
            transform.GetChild(2).gameObject.SetActive(IsEmpty ? false : true);
            // ���
            transform.GetChild(3).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// �� �������� Ŭ�� ���� �� ������ �Լ�
    /// </summary>
    public void EmptyLabClick()
    {
        // ������� �ʴٸ� -> �������̶��
        if (!isEmpty)
            return;

        // ��� �������̸�
        if (!isOpen)
        {
            // ��� ����
            PlayDataManager.Instance.MainDia -= LabManager.instance.labOpenCost[PlayDataManager.Instance.playData.openLabCount];
            
            PlayDataManager.Instance.playData.openLabCount++;

            // �����ְ�
            LabManager.instance.OpenLab(PlayDataManager.Instance.playData.openLabCount);

            // �������� �ٲٱ�
            IsOpen = true;
            return;
        }

        LabManager.instance.researchListPN.SetActive(true);
        LabManager.instance.clickedIndex = labIndex;
    }

    public IEnumerator WaitReqTime(ResearchType type, int researchID)
    {
        IsEmpty = false;

        // �ش� ��ư ������
        PlayDataManager.Instance.playData.isResearching[(int)type, researchID] = true;

        Print.Array2D(PlayDataManager.Instance.playData.isResearching);

        yield return new WaitForSeconds(requireTime);

        ResearchComplete(type, researchID);
    }

    /// <summary>
    /// ������ �Ϸ� ���� �� ������ �Լ�
    /// </summary>
    void ResearchComplete(ResearchType type, int researchID)
    {
        // �ش� ��ư ������ ����
        PlayDataManager.Instance.playData.isResearching[(int)type, researchID] = false;
        // PDM�� �ش� ���׷��̵� ���� �÷��ֱ�
        PlayDataManager.Instance.playData.labResearchLevels[(int)type, researchID]++;

        // �� �����Ƿ� ������ֱ�
        IsEmpty = true;

        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(false);

        Print.Array2D(PlayDataManager.Instance.playData.labResearchLevels);
    }
}
