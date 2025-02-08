using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTime
{
    public int d;
    public int h;
    public int m;
    public int s;
}

public class LabButton : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI labNameText;

    public int labIndex;

    [Header("# Unlock")]
    [SerializeField]
    TextMeshProUGUI unlockText;

    [SerializeField]
    bool isEmpty = true;
    [SerializeField]
    bool isOpen = false;

    [Header("# Upgrade")]
    [SerializeField]
    public TextMeshProUGUI nameNLevelText;
    [SerializeField]
    public TextMeshProUGUI upInfoText;
    [SerializeField]
    public TextMeshProUGUI remainTimeText;

    [SerializeField]
    Slider mySlider;

    /// <summary>
    /// ����� �ð�
    /// </summary>
    public TimeSpan elapsedTime;

    [HideInInspector]       // �ҿ�ð� ��ȭ X
    public float requireTime;
    [HideInInspector]       // �����ð� -> �ð��� ���� ���� �پ��
    public float remainTime;

    ResearchData myData;

    /// <summary>
    /// ����ִ°�? -> false = ������
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

    public ResearchData MyData
    {
        get { return myData; }

        set
        {
            myData = value;

            if (myData != null)
            {
                requireTime = MyData.reqTimes[PlayDataManager.Instance.playData.labResearchLevels[(int)MyData.researchType, MyData.researchID]];
                StartCoroutine(WaitReqTime(requireTime - (float)(DateTime.Now - PlayDataManager.Instance.playData.startTimes[labIndex]).TotalSeconds));
            }
        }
    }


    private void Awake()
    {
        MyData = PlayDataManager.Instance.playData.isResearchingData[labIndex];
    }

    private void Start()
    {
        labNameText.text = "Lab " + (labIndex + 1);

        if (IsEmpty)
        {
            this.enabled = false;

            // ��� �������̸�
            if (!IsOpen)
            {
                unlockText.text = (labIndex + 1) + "��° ������������\n" + LabManager.instance.labOpenCost[labIndex] + "<sprite=0>";
            }
        }

    }


    private void Update()
    {
        Debug.Log(labIndex);

        //// ���׷��̵� ���̰�, ���� �ð��� 0���� �۴ٸ�
        //if (!transform.GetChild(2).gameObject.activeSelf && remainTime < 0)
        //{
        //    remainTime = 0;
        //    return;
        //}

        elapsedTime = DateTime.Now - PlayDataManager.Instance.playData.startTimes[labIndex];

        remainTime = requireTime - (float)elapsedTime.TotalSeconds;

        remainTimeText.text = LabManager.instance.DisplayTime(remainTime);

        mySlider.value = 1 - (remainTime / requireTime);
    }

    /// <summary>
    /// ����ư UI ����
    /// </summary>
    void LabBtSet()
    {
        if (IsOpen)
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

    public IEnumerator WaitReqTime(float remainTime)
    {
        // �ش� ��ư ������
        PlayDataManager.Instance.playData.isResearching[(int)MyData.researchType, MyData.researchID] = true;

        this.enabled = true;


        Print.Array2D(PlayDataManager.Instance.playData.isResearching);
        Print.ResearchArray(PlayDataManager.Instance.playData.isResearchingData);

        yield return new WaitForSeconds(remainTime);

        ResearchComplete();
    }

    /// <summary>
    /// ������ �Ϸ� ���� �� ������ �Լ�
    /// </summary>
    void ResearchComplete()
    {
        // PDM�� �ش� ���׷��̵� ���� �÷��ֱ�
        PlayDataManager.Instance.playData.labResearchLevels[(int)MyData.researchType, MyData.researchID]++;
        PlayDataManager.Instance.playData.isResearching[(int)MyData.researchType, MyData.researchID] = false;
        PlayDataManager.Instance.playData.isResearchingData[labIndex] = null;

        // �� �����Ƿ� ������ֱ�
        IsEmpty = true;

        this.enabled = false;

        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(false);

        //Print.Array2D(PlayDataManager.Instance.playData.labResearchLevels);
        Print.Array2D(PlayDataManager.Instance.playData.isResearching);

        Debug.Log("���� ����");
    }
}
