using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColumnResizer : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public RectTransform leftColumn;  // 왼쪽 패널
    private float originalX;         // 드래그 시작 시의 초기 X 위치
    private float initialLeftWidth;  // 드래그 시작 시 왼쪽 패널의 초기 너비

    public float minWidth = 100f;    // 최소 너비
    public float maxWidth = 1000f;           // 최대 너비 

    private void Start()
    {
        if (leftColumn == null)
        {
            Debug.LogError("Panels or buttons are not assigned");
            return;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalX = this.transform.localPosition.x; // 드래그 시작 시의 위치 저장
        initialLeftWidth = leftColumn.rect.width; // 드래그 시작 시 왼쪽 패널의 너비 저장
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)this.transform.parent, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            float newX = localPoint.x - originalX;  // 원점으로부터의 변화량 계산

            // 왼쪽 패널의 새로운 너비 계산
            float newWidth = Mathf.Clamp(initialLeftWidth + newX, minWidth, maxWidth);
            leftColumn.sizeDelta = new Vector2(newWidth, leftColumn.sizeDelta.y);

            // Layout Group 갱신, leftPanel의 레이아웃을 갱신
            LayoutRebuilder.ForceRebuildLayoutImmediate(leftColumn);
            NotifySizeChanged(newWidth);
        }
    }

    private void NotifySizeChanged(float newSize)
    {
        ColumnInfo column = leftColumn.GetComponent<ColumnInfo>();
        column.Width = newSize;
        column.ListView.ListViewWidthCal(column.ColumnIndex);

        //leftPanel.// TODO : Header에서 Column1의 Info 알아낸 다음 Width 변경
        // Resizer 옆에 있는 Column이 몇번째 Column인지 모르는 문제.
    }
}
