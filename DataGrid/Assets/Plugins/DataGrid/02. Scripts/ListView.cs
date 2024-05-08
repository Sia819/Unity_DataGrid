#if DATAGRID_DEPENDENCY_INSTALLED
using System;
using System.Collections.ObjectModel;
using UniRx;
using UnityEngine;

namespace UIExtension.ListView
{
    public class ListView : MonoBehaviour
    {
        [SerializeField] private GameObject listItemParent;                  // Parents to add items

        [Space(5)]
        [Header("ListView Item Prefabs")]
        [SerializeField] private GameObject listViewHeaderPrefab;            // Column Button
        [SerializeField] private GameObject listViewRowPrefab;                  // Prefab to add items

        [field: Space(5)]
        [field: Header("ListView Header (Optional)")]
        [field: SerializeField] public ListViewHeader Header { get; private set; }

        public bool UseColumnResizer { get; set; } = false;

        private ObservableCollection<ListViewRow> rows = new();
        
        internal IObservable<Unit> Initialized => Header.Initialized;

        private void Start()
        {
            // Add List Listener
            rows.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(
                delegate (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
                {
                    if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                    {
                        ListViewWidthCal();
                        ListViewScrollCal();
                    }

                    for (int i = 0; i < rows.Count && i < 50; i++)
                    {
                        rows[i].RowIndex = i;
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
        }

        public void AddColumn(params string[] columnName)
        {
            Initialized.Subscribe(_ =>
            {
                for (int i = 0; i < columnName.Length; i++)
                {
                    Header.AddColumn(columnName[i]);
                }
            });
        }

        public void AddColumn(string columnName, float width, float fontSize)
        {
            Initialized.Subscribe(_ => 
            {
                Header.AddColumn(columnName, width, fontSize);
            });
        }

        /// <summary> ListViewRow 오브젝트를 생성하며 AddRow 작업을 인계합니다. </summary>
        /// <param name="rowElements"> <see cref="ListViewRow.AddRows"/>함수에서 지원하는 rowElements type들을 확인할 수 있습니다.</param>
        public void AddRow(params object[] rowElements)
        {
            Initialized.Subscribe(_ =>
            {
                GameObject listViewRowInstant = Instantiate(listViewRowPrefab, listItemParent.transform);
                ListViewRow listViewRow = listViewRowInstant.GetComponent<ListViewRow>();
                listViewRow.Init(this, rowElements);
                this.rows.Add(listViewRow);
            });
        }

        public void ClearRows()
        {
            Initialized.Subscribe(_ =>
            {
                for (int i = 0; i < rows.Count; i++)
                {
                    Destroy(rows[i].gameObject);
                }
                rows.Clear();
            });
        }

        public int GetRowIndex(ListViewRow listViewRow)
        {
            return rows.IndexOf(listViewRow);
        }

        /// <summary> 아이템 추가 등의 이유로 ListView의 Content의 높이를 조절합니다. </summary>
        private void ListViewScrollCal()
        {
            float totalHeight = 0;
            for (int i = 0; i < listItemParent.transform.childCount; i++)
            {
                RectTransform childRect = listItemParent.transform.GetChild(i) as RectTransform;
                if (childRect != null)
                {
                    totalHeight += childRect.rect.height;  // 자식의 높이를 더합니다.
                }
            }

            // listItemParent의 RectTransform의 sizeDelta를 조정합니다.
            RectTransform listItemParentRect = listItemParent.transform as RectTransform;
            if (listItemParentRect != null)
            {
                // 기존의 width는 유지하고, height만 totalHeight로 설정합니다.
                listItemParentRect.sizeDelta = new Vector2(listItemParentRect.sizeDelta.x, totalHeight);
            }
        }

        /// <summary> Column Width의 변경 등의 이유로 ListView Row의 모든 아이템의 Width를 재조절합니다. </summary>
        private void ListViewWidthCal()
        {
            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = 0; j < Header.ColumnCount; j++)
                {
                    float width = Header.GetColumnInfo(j).Width;
                    if (UseColumnResizer == true)
                        width += Header.ResizerWidth;
                    rows[i].ChangeItemWidth(j, width);
                }
            }
        }

        public void ListViewWidthCal(int columnIndex)
        {
            for (int i = 0; i < rows.Count; i++)
            {
                float width = Header.GetColumnInfo(columnIndex).Width;
                if (UseColumnResizer == true)
                    width += Header.ResizerWidth;
                rows[i].ChangeItemWidth(columnIndex, width);
            }
        }
    }
}
#endif