using System.Collections.ObjectModel;
using UnityEngine;

public class ListView : MonoBehaviour
{
    [SerializeField] private GameObject listItemParent;                  // Parents to add items

    [Space(5)]
    [Header("ListView Item Prefabs")]
    [SerializeField] private GameObject listViewHeaderPrefab;            // Column Button
    [SerializeField] private GameObject listViewRowPrefab;                  // Prefab to add items

    [field:Space(5)]
    [field:Header("ListView Header")]
    [field:SerializeField] public ListViewHeader Header { get; private set; }
    private ObservableCollection<ListViewRow> rows = new();

    private void Start()
    {
        // Add List Listener
        rows.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(
            delegate (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    Debug.Log("item changed!!");
                    ListViewWidthCal();
                    ListViewScrollCal();
                }
            }
        );

        // Add Header
        if (Header == null)
        {
            GameObject instatedListItem = Instantiate(listViewHeaderPrefab, listItemParent.transform);
            ListViewHeader listViewItem = instatedListItem.GetComponent<ListViewHeader>();
            listViewItem.Init(this);
            Header = listViewItem;
        }
        else
        {
            Header.Init(this);
        }

        // Add Exist Rows
        for (int i = 0; i < listItemParent.transform.childCount; i++)
        {
            Transform children = listItemParent.transform.GetChild(i);
            if (children.TryGetComponent<ListViewRow>(out ListViewRow row))
            {
                rows.Add(row);
            }
        }

        // Content Calculate Height
        //ListViewScrollCal();
    }

    public void AddColumn(string columnName)
    {
        Header.AddColumn(columnName);
    }

    public void AddRow(params string[] rows)
    {
        GameObject listViewRowInstant = Instantiate(listViewRowPrefab, listItemParent.transform);
        ListViewRow listViewRow = listViewRowInstant.GetComponent<ListViewRow>();
        listViewRow.Init(this, rows);
        this.rows.Add(listViewRow);
    }

    public int GetRowIndex(ListViewRow listViewRow)
    {
        return rows.IndexOf(listViewRow);
    }

    /// <summary> ������ �߰� ���� ������ ListView�� Content�� ���̸� �����մϴ�. </summary>
    private void ListViewScrollCal()
    {
        float totalHeight = 0;
        for (int i = 0; i < listItemParent.transform.childCount; i++)
        {
            RectTransform childRect = listItemParent.transform.GetChild(i) as RectTransform;
            if (childRect != null)
            {
                totalHeight += childRect.rect.height;  // �ڽ��� ���̸� ���մϴ�.
            }
        }

        // listItemParent�� RectTransform�� sizeDelta�� �����մϴ�.
        RectTransform listItemParentRect = listItemParent.transform as RectTransform;
        if (listItemParentRect != null)
        {
            // ������ width�� �����ϰ�, height�� totalHeight�� �����մϴ�.
            listItemParentRect.sizeDelta = new Vector2(listItemParentRect.sizeDelta.x, totalHeight);
        }
    }

    /// <summary> Column Width�� ���� ���� ������ ListView Row�� ��� �������� Width�� �������մϴ�. </summary>
    private void ListViewWidthCal()
    {
        for (int i = 0; i < rows.Count; i++)
        {
            for (int j = 0; j < Header.ColumnCount; j++)
            {
                float width = Header.GetColumnInfo(j).Width;
                rows[i].ChangeItemWidth(j, width);
            }
        }
    }

    public void ListViewWidthCal(int columnIndex)
    {
        for (int i = 0; i < rows.Count; i++)
        {
            //for (int j = 0; j < Header.ColumnCount; j++)
            //{
            float width = Header.GetColumnInfo(columnIndex).Width;
            rows[i].ChangeItemWidth(columnIndex, width);
            //}
        }
    }
}