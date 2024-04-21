using UnityEngine;
using UnityEngine.EventSystems;

public class CursorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D cursorTexture; // Inspector���� ������ Ŀ�� �̹���
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero; // Ŀ���� Ȱ�� ���� ���� (�Ϲ������� �̹��� �߾� �Ǵ� Ư�� ����)

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        Debug.Log("Mouse Enter - Cursor changed");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode); // �⺻ Ŀ���� ����
        Debug.Log("Mouse Exit - Cursor reverted");
    }
}
