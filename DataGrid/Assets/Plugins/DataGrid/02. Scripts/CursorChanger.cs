using UnityEngine;
using UnityEngine.EventSystems;

public class CursorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D cursorTexture; // Inspector에서 설정할 커서 이미지
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot;

    void Start()
    {
        // 이미지의 중앙을 hotSpot으로 설정
        hotSpot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode); // 기본 커서로 복원
    }
}