using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableButton : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public RectTransform leftPanel;  // ���� �г�
    public RectTransform dragButton; // �巡�� ������ ��ư

    private float originalX;         // �巡�� ���� ���� �ʱ� X ��ġ
    private float initialLeftWidth;  // �巡�� ���� �� ���� �г��� �ʱ� �ʺ�

    public float minWidth = 100f;    // �ּ� �ʺ�
    public float maxWidth = 1000f;           // �ִ� �ʺ�

    private void Start()
    {
        if (leftPanel == null || dragButton == null)
        {
            Debug.LogError("Panels or buttons are not assigned");
            return;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalX = dragButton.localPosition.x; // �巡�� ���� ���� ��ġ ����
        initialLeftWidth = leftPanel.rect.width; // �巡�� ���� �� ���� �г��� �ʺ� ����
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)dragButton.parent, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            float newX = localPoint.x - originalX;  // �������κ����� ��ȭ�� ���

            // ���� �г��� ���ο� �ʺ� ���
            float newWidth = Mathf.Clamp(initialLeftWidth + newX, minWidth, maxWidth);
            leftPanel.sizeDelta = new Vector2(newWidth, leftPanel.sizeDelta.y);

            // Layout Group ����, leftPanel�� ���̾ƿ��� ����
            LayoutRebuilder.ForceRebuildLayoutImmediate(leftPanel);
            NotifySizeChanged();
        }
    }

    private void NotifySizeChanged()
    {
        leftPanel.// TODO : Header���� Column1�� Info �˾Ƴ� ���� Width ����
    }
}
