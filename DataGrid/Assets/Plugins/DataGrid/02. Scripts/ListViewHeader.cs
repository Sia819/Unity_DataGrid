#if DATAGRID_DEPENDENCY_INSTALLED
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UniRx;
using UnityEngine.Android;

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

        /// <summary>
        /// ListViewHeader가 유니티에 의해 완전히 초기화 된 시점입니다.
        /// 이 시점 이후에 ListViewHeader를 정상적으로 사용할 수 있습니다.
        /// (ListViewHeader 초기화 시점보다 명령이 우선될 수 없도록 관리다.)
        /// </summary>
        //internal IObservable<Unit> Initialized => awaked.CombineLatest(initialized, (awake, init) => awake && init)
        //                                          .Where(x => x)
        //                                          .AsUnitObservable();

        public IObservable<Unit> Initialized => awaked.CombineLatest(initialized, (awake, init) => awake && init)
                                                .Where(x => x)
                                                .AsUnitObservable()
                                                .Replay(1)
                                                .RefCount();

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

        /// <summary> 
        /// ListViewHeader 오브젝트를 초기화합니다. 만약 ListViewHeader 오브젝트가 유니티에서 초기화 전에 
        /// ListView가 ListViewHeader를 사용하려 하면 발생할 수 있는 실행 순서 오류를 방지합니다.
        /// (ListViewHeader 오브젝트 초기화보다 명령이 먼저일 수 있기 때문입니다.)
        /// </summary>
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

        /// <summary> 새로운 열을 만듭니다. </summary>
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

        public ColumnInfo GetColumnInfo(int index)
        {
            if (index < 0 || index >= columns.Count)
            {
                Debug.LogWarning($"Column의 범위를 벗어나는 인덱스'{index}'에 접근했습니다!");
                return null;
            }
            return columns[index];
        }

        /// <summary> 헤더에 추가된 모든 요소를 초기화 합니다. </summary>
        public void ClearColumns()
        {
            foreach (ColumnInfo column in columns)
            {
                Destroy(column.ColumnResizer);
                Destroy(column.gameObject);
            }
            columns.Clear();
        }

        /// <summary> 해당 열 이름과 같은 헤더의 Column을 제거합니다. </summary>
        public void RemoveColumn(string columnName)
        {
            ColumnInfo column = columns.Find((columnInfo) => columnInfo.Name == columnName);
            if (column == null)
            {
                Debug.LogWarning($"존재하지 않는 Column '{columnName}'을 제거하려 했습니다.");
                return;
            }
            Destroy(column.ColumnResizer);
            Destroy(column.gameObject);
            columns.Remove(column);
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
            if (parent.UseColumnResizer == true)
            {
                GameObject resizer = Instantiate(columnResizerPrefab, this.transform);
                ColumnResizer columnResizer = resizer.GetComponent<ColumnResizer>();
                columnResizer.leftColumn = column.transform as RectTransform;
                columnInfo.ColumnResizer = resizer;
            }
            columns.Add(columnInfo);
        }
    }
}
#endif