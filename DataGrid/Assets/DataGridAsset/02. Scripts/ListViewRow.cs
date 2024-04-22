using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ListView Row Object
/// </summary>
public class ListViewRow : MonoBehaviour
{
    public List<GameObject> gameObjectItems;   // Columns of Row Gamebject. for example label, buttons...
    public bool useCrossBackgroundColor = true;

    [Header("Prefabs")]
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject textPrefab;

    public int RowIndex { get; private set; }

    private ListView parent;
    private bool initialized = false;

    private void Start()
    {
        HorizontalLayoutGroup group = this.gameObject.GetComponent<HorizontalLayoutGroup>();
        group.childControlHeight = false;
        group.childControlWidth = false;

        // Add exist cells
        for (int i = 0; i < transform.childCount; i++)
        {
            // TODO : cell.cs �� ����� TryGetComponent()�� ������ ����Ǿ����.
            Transform children = transform.GetChild(i);
            gameObjectItems.Add(children.gameObject);
        }
    }

    public void Init(ListView parent, string[] jsonData)
    {
        if (initialized == true)
        {
            Debug.LogWarning("�ι� �̻� ListViewRow�� �ʱ�ȭ�Ϸ� �߽��ϴ�.");
            return;
        }

        this.parent = parent;
        ChangeItem(jsonData);
        initialized = true;
        GetInstanceID();
    }

    public void ChangeItem(string[] itemData)
    {
        for (int i = 0; i < itemData.Length; i++)
        {
            ColumnInfo columnInfo = parent.Header.GetColumnInfo(i);
            if (columnInfo == null) return;

            GameObject textObject = Instantiate(textPrefab, this.transform);
            RectTransform rectTransform = textObject.GetComponent<RectTransform>();
            TMP_Text textComponent = textObject.GetComponent<TMP_Text>();

            rectTransform.sizeDelta = new Vector2(columnInfo.Width, rectTransform.sizeDelta.y);
            textComponent.text = itemData[i];

            gameObjectItems.Add(textObject);
        }
    }

    public void ChangeItemWidth(int cellIndex, float width)
    {
        if (cellIndex < gameObjectItems.Count)
        {
            RectTransform rectTransform = gameObjectItems[cellIndex].GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
        }
    }

}
