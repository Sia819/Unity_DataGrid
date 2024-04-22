using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLiteConnection;

public class ListViewHeader : MonoBehaviour
{
    [SerializeField] private GameObject columnButton;

    public int ColumnCount => columns.Count;

    private ListView parent;
    private List<ColumnInfo> columns = new List<ColumnInfo>();
    private bool initialized = false;

    private void Start()
    {
        // Add Exist Columns
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform children = transform.GetChild(i);
            if (children.TryGetComponent<ColumnInfo>(out ColumnInfo column))
            {
                column.ColumnIndex = i;
                column.ListView = parent;
                children.name = $"Column{columns.Count + 1}";
                AddColumn(children.gameObject, column, children.name);
            }
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
        ColumnInfo columnInfo = colBtnIns.GetComponent<ColumnInfo>();
        AddColumn(colBtnIns, columnInfo, columnName);
    }

    /// <summary> Add Exist Column </summary>
    private void AddColumn(GameObject column, ColumnInfo columnInfo, string columnName)
    {
        RectTransform rectTransform = column.GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(columnInfo.Width, rectTransform.sizeDelta.y);     // Column Size ����
        columnInfo.Name = $"Column{columns.Count + 1}";
        columnInfo.ColumnIndex = columns.Count;
        columns.Add(columnInfo);
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
}
