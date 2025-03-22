using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform canvas;                   // UI�� �ҼӵǾ� �ִ� �ֻ���� Canvas Transform
    private Transform previousParent;           // �ش� ������Ʈ�� ������ �ҼӵǾ� �־��� �θ� Transform
    private RectTransform rect;                 // UI ��ġ ��� ���� RectTransform
    private CanvasGroup canvasGroup;            // UI�� ���İ��� ��ȣ�ۿ� ��� ���� CanvasGroup

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// ���� ������Ʈ�� �巡���ϱ� ������ �� 1ȸ ȣ��
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // �巡�� ������ �ҼӵǾ� �ִ� �θ� Transform ���� ����
        previousParent = transform.parent;

        // ���� �巡�� ���� UI�� ȭ���� �ֻ�ܿ� ��µǵ��� �ϱ� ����
        transform.SetParent(canvas);        // �θ� ������Ʈ�� �ֻ�� canvas�� ���� -> UI ���� �տ� ����
        transform.SetAsLastSibling();       // ���� �տ� ���̵��� �ֻ�� canvas�� ������ �ڽ����� ����

        // �巡�� ������ ������Ʈ�� �ڽŸ��� �ƴ� �ڽĵ��� ������ ���� ���� �ֱ� ������ CanvasGroup���� ����
        // ���İ��� .6���� ����, ���� �浹 ó���� ���� �ʵ��� �Ѵ�.
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }
    
    /// <summary>
    /// ���� ������Ʈ�� �巡�� ���� �� �� ������ ȣ��
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        // ���� ��ũ������ ���콺 ��ġ�� UI ��ġ�� ���� (UI�� ���콺�� �Ѿƴٴϴ� ����)
        rect.position = eventData.position;
    }

    /// <summary>
    /// ���� ������Ʈ�� �巡�׸� ������ �� 1ȸ ȣ��
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        // �巡�׸� �����ϸ� �θ� canvas�� �������ִ� ����
        // �巡�׸� ������ �� �θ� �״�� canvas�̸� ������ ������ �ƴ� ������ ����
        // ����� �ߴٴ� ���̱� ������ �巡�� ������ �ҼӵǾ� �ִ� ������ �������� ������ �̵�
        if (transform.parent == canvas)
        {
            // �������� �ҼӵǾ��־��� previousParent�� �ڽ����� �����ϰ�, �ش� ��ġ�� �̵�
            transform.SetParent (previousParent);
            rect.position = previousParent.GetComponentInParent<RectTransform>().position;
        }

        // ���İ��� 1�� �ǵ�����, ���� �浹ó���� �����ϵ��� �Ѵ�
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
    }
}
