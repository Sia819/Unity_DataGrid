using UnityEngine;
using UnityEngine.EventSystems;

public class CursorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D cursorTexture; // Inspector에서 설정할 커서 이미지
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero; // 커서의 활성 지점 설정 (일반적으로 이미지 중앙 또는 특정 지점)

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        Debug.Log("Mouse Enter - Cursor changed");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode); // 기본 커서로 복원
        Debug.Log("Mouse Exit - Cursor reverted");
    }
}
