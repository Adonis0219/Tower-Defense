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

    [HideInInspector]       // �����ð� -> �ð��� ���� ���� �پ��
    public float remainTime;

    ResearchData myData;

    public int curResearchLevel;

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

            // myData�� ������� �ʴٸ� -> ���� ���̶��
            // ���� ������ �� �帥 �ð���ŭ ���� �ð� �ٿ��ֱ�
            if (myData != null)
            {
                SetUpgradeBt();
                // ���� ����, ���� �簳
                Researching();
            }
        }
    }

    void SetUpgradeBt()
    {
        // Ŭ���� ������ �ٲ��ֱ�
        // �� ������ ���ֱ�
        transform.GetChild(1).gameObject.SetActive(false);
        // ������ ���ֱ�
        transform.GetChild(2).gameObject.SetActive(true);

        nameNLevelText.text = myData.researchName + "Lv." + (curResearchLevel + 1);
        upInfoText.text = LabManager.instance.UpInfoStrSet(myData);
        remainTime = myData.reqTimes[curResearchLevel];
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
        // �������� �ƴ϶�� �󸮸���
        // ���� �ð��� 0���� �۴ٸ� ���� || remainTime < 0
        if (IsEmpty)
        {
            return;
        }

        // �ð� �� = ����ð� - ���� �ð�
        ////////////////elapsedTime = DateTime.Now - PlayDataManager.Instance.playData.startTimes[labIndex];

        //////////////remainTime = myData.reqTimes[curResearchLevel] - (float)elapsedTime.TotalSeconds;

        remainTimeText.text = LabManager.instance.DisplayTime(PlayDataManager.Instance.playData.labRemainTime[labIndex]);
        ////////////remainTimeText.text = LabManager.instance.DisplayTime(remainTime);

        Debug.Log(labIndex + "��° ������ ���� �ð� : " + LabManager.instance.DisplayTime(remainTime));

        mySlider.value = 1 - (PlayDataManager.Instance.playData.labRemainTime[labIndex] / myData.reqTimes[curResearchLevel]);
        /////////////mySlider.value = 1 - (remainTime / myData.reqTimes[curResearchLevel]);

        ///////////////if (remainTime < 0)
        if (PlayDataManager.Instance.playData.labRemainTime[labIndex] < 0)
        {
            // ���� �Ϸ� �ǳ� ����ֱ�
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(4).gameObject.SetActive(true);

            // ���� �Ϸ� ���� �÷��ֱ�
            //////////////LabManager.instance.LabCompleteCount++;
        }
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
        // �� �Ŵ������� Ŭ���� �ε��� ��ȣ �Ѱ��ֱ�
        LabManager.instance.clickedIndex = labIndex;
    }

    /// <summary>
    ///  ��ŵ ��ư�� �����Ϸ� ��ư ���� �� ������ �Լ�
    /// </summary>
    public void RschCompleteBtClk()
    {
        ResearchComplete();
    }

    void Researching()
    {
        // �ش� ��ư ������
        PlayDataManager.Instance.playData.isResearching[(int)MyData.researchType, MyData.researchID] = true;

        this.enabled = true;        
    }

    /// <summary>
    /// ������ �Ϸ� ���� �� ������ �Լ�
    /// </summary>
    void ResearchComplete()
    {
        // �����Ϸ� ��ư ���ֱ�
        transform.GetChild(4).gameObject.SetActive(false);
        // �����Ϸ� ���� �ٿ��ֱ�
        /////////////////LabManager.instance.LabCompleteCount--;
        PlayDataManager.Instance.LabCompleteCount--;


        // PDM�� �ش� ���׷��̵� ���� �÷��ֱ�
        PlayDataManager.Instance.playData.labResearchLevels[(int)MyData.researchType, MyData.researchID]++;
        // ���� ������ �ƴ�
        PlayDataManager.Instance.playData.isResearching[(int)MyData.researchType, MyData.researchID] = false;
        // �������� ���������� �����ֱ�
        PlayDataManager.Instance.playData.isResearchingData[labIndex] = null;

        // �� �����Ƿ� ������ֱ�
        IsEmpty = true;

        ///////////////// �̰� �� �� ����..?
        this.enabled = false;

        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(false);

        Print.Array2D(PlayDataManager.Instance.playData.labResearchLevels);
        Print.Array2D(PlayDataManager.Instance.playData.isResearching);

        Debug.Log("���� ����");
    }
}
