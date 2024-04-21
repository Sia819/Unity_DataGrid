using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ListViewHeader : MonoBehaviour
{
    [SerializeField] private GameObject columnButton;

    public int ColumnCount => columns.Count;

    private ListView parent;
    private List<ColumnInfo> columns = new List<ColumnInfo>();
    private bool initialized = false;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform children = transform.GetChild(i);
            //TMP_Text text = children.GetChild(0).GetComponent<TMP_Text>();
            //text.text = $"Column{columns.Count + 1}";
            children.name = $"Column{columns.Count + 1}";
            AddColumn(children.gameObject, children.name);
            //columns.Add(new ColumnInfo(text, $"Column{columns.Count + 1}"));
        }
    }

    public void Init(ListView parent, params string[] columnNames)
    {
        if (initialized == true)
        {
            Debug.LogWarning("�ι� �̻� ListViewHeader�� �ʱ�ȭ�Ϸ� �߽��ϴ�.");
            return;
        }

        this.parent = parent;
        foreach(var columnName in columnNames)
        {
            AddColumn(columnName);
        }
        initialized = true;
    }

    /// <summary> Add New Column </summary>
    public void AddColumn(string columnName) // TODO : ColumnInfo -> Width, Color, Font �߰�
    {
        GameObject colBtnIns = Instantiate(columnButton, this.transform);
        AddColumn(colBtnIns, columnName);
    }

    /// <summary> Add Exist Column </summary>
    private void AddColumn(GameObject column, string columnName)
    {
        RectTransform rectTransform = column.GetComponent<RectTransform>();
        TMP_Text text = column.transform.GetChild(0).GetComponent<TMP_Text>();

        ColumnInfo columnInfo = new ColumnInfo(text, columnName);
        rectTransform.sizeDelta = new Vector2(columnInfo.Width, rectTransform.sizeDelta.y);
        text.text = columnInfo.Name;
        column.name = $"Column{columns.Count + 1}";
        columns.Add(new ColumnInfo(text, $"Column{columns.Count + 1}"));
    }

    public ColumnInfo GetColumnInfo(int index)
    {
        if (index < 0 || index >= columns.Count)
        {
            Debug.LogWarning($"Column�� ������ ����� �ε���'{index}'�� �����߽��ϴ�!");
            return null;
        }
        return columns[index];
    }

    public class ColumnInfo
    {
        public TMP_Text Text { get; }
        public string Name { get; }
        public float Width { get; set; }
        public float FontSize { get; set; }
        public Color Color { get; set; }

        public ColumnInfo(TMP_Text text, string name, float width = 110f, float fontSize = 14f, Color? color = null)
        {
            Text = text;
            Name = name;
            Width = width;
            FontSize = fontSize;
            Color = color ?? Color.black; // �⺻ ������ ���������� ����
        }
    }
}
