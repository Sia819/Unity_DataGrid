using UnityEngine;
using UnityEngine.EventSystems;

public class CursorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D cursorTexture; // Inspector���� ������ Ŀ�� �̹���
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot;

    void Start()
    {
        // �̹����� �߾��� hotSpot���� ����
        hotSpot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode); // �⺻ Ŀ���� ����
    }
}
