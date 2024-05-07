using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UniRx;

namespace UIExtension.ListView
{
    public class ListViewHeader : MonoBehaviour
    {
        [SerializeField] private GameObject columnButtonPrefab;
        [SerializeField] private GameObject columnResizerPrefab;

        public int ColumnCount => columns.Count;
        public float ResizerWidth
        {
            get
            {
                var result = (columnResizerPrefab.transform as RectTransform).rect.width;
                return result;
            }
        }

        private ListView parent;
        private List<ColumnInfo> columns = new List<ColumnInfo>();
        private ReactiveProperty<bool> awaked = new();
        private ReactiveProperty<bool> initialized = new();

        internal IObservable<Unit> Initialized => awaked.CombineLatest(initialized, (awake, init) => awake && init)
                                                  .Where(x => x)
                                                  .AsUnitObservable();


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
                    AddColumn(children.gameObject, children.name);
                }
            }
            awaked.Value = true;

        }

        public void Init(ListView parent, params string[] columnNames)
        {
            if (initialized.Value == false)
            {
                this.parent = parent;
                foreach (var columnName in columnNames)
                {
                    AddColumn(columnName);
                }
                initialized.Value = true;
            }
            else
            {
                Debug.LogWarning("두번 이상 ListViewHeader를 초기화하려 했습니다.");
            }
        }

        /// <summary> Add New Column </summary>
        public void AddColumn(string columnName, float width = 100f, float fontSize = 14f) // TODO : ColumnInfo -> Color 추가
        {
            ColumnInfo exist = columns.Find((columnInfo) => columnInfo.Name == columnName);
            if (exist != null)
            {
                Debug.LogWarning($"이미 존재하는 Column '{columnName}'을 추가하려 했습니다.");
                return;
            }
            GameObject colBtnIns = Instantiate(columnButtonPrefab, this.transform);
            AddColumn(colBtnIns, columnName, width, fontSize);
        }

        /// <summary> Add Exist Column </summary>
        private void AddColumn(GameObject column, string columnName, float width = 100f, float fontSize = 14f)
        {
            ColumnInfo columnInfo = column.GetComponent<ColumnInfo>();
            columnInfo.ListView = parent;
            columnInfo.Name = columnName;
            columnInfo.ColumnIndex = columns.Count;
            columnInfo.Width = width;
            columnInfo.FontSize = fontSize;
            columns.Add(columnInfo);
            if (parent.UseColumnResizer == true)
            {
                GameObject resizer = Instantiate(columnResizerPrefab, this.transform);
                ColumnResizer columnResizer = resizer.GetComponent<ColumnResizer>();
                columnResizer.leftColumn = column.transform as RectTransform;
            }
        }

        public ColumnInfo GetColumnInfo(int index)
        {
            if (index < 0 || index >= columns.Count)
            {
                Debug.LogWarning($"Column의 범위를 벗어나는 인덱스'{index}'에 접근했습니다!");
                return null;
            }
            return columns[index];
        }
    }
}