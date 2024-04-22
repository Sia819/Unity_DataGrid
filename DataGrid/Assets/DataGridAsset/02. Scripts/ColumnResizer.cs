using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColumnResizer : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public RectTransform leftPanel;  // ���� �г�

    private float originalX;         // �巡�� ���� ���� �ʱ� X ��ġ
    private float initialLeftWidth;  // �巡�� ���� �� ���� �г��� �ʱ� �ʺ�

    public float minWidth = 100f;    // �ּ� �ʺ�
    public float maxWidth = 1000f;           // �ִ� �ʺ�

    private void Start()
    {
        if (leftPanel == null)
        {
            Debug.LogError("Panels or buttons are not assigned");
            return;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalX = this.transform.localPosition.x; // �巡�� ���� ���� ��ġ ����
        initialLeftWidth = leftPanel.rect.width; // �巡�� ���� �� ���� �г��� �ʺ� ����
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)this.transform.parent, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            float newX = localPoint.x - originalX;  // �������κ����� ��ȭ�� ���

            // ���� �г��� ���ο� �ʺ� ���
            float newWidth = Mathf.Clamp(initialLeftWidth + newX, minWidth, maxWidth);
            leftPanel.sizeDelta = new Vector2(newWidth, leftPanel.sizeDelta.y);

            // Layout Group ����, leftPanel�� ���̾ƿ��� ����
            LayoutRebuilder.ForceRebuildLayoutImmediate(leftPanel);
            NotifySizeChanged(newWidth);
        }
    }

    private void NotifySizeChanged(float newSize)
    {
        ColumnInfo column = leftPanel.GetComponent<ColumnInfo>();
        column.Width = newSize;
        column.ListView.ListViewWidthCal(column.ColumnIndex);

        //leftPanel.// TODO : Header���� Column1�� Info �˾Ƴ� ���� Width ����
        // Resizer ���� �ִ� Column�� ���° Column���� �𸣴� ����.
    }
}
