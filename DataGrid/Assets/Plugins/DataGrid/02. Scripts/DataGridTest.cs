#if DATAGRID_DEPENDENCY_INSTALLED
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UIExtension.ListView;

public class DataGridTest : MonoBehaviour
{
    [SerializeField] private ListView listView;

    // Start is called before the first frame update
    private void Start()
    {
        Button button = this.GetComponent<Button>();
        button.onClick.AddListener(AddRowTest);

        listView.UseColumnResizer = true;
        listView.AddColumn("날짜", width: 470f, fontSize: 50f);
        listView.AddColumn("이름", width: 328f, fontSize: 50f);
        listView.AddColumn("소속", width: 192f, fontSize: 50f);
        listView.AddColumn("사번", width: 285f, fontSize: 50f);
        listView.AddColumn("확인", width: 336f, fontSize: 50f);
        listView.AddColumn("삭제", width: 336f, fontSize: 50f);
    }

    private void AddRowTest()
    {
        UnityAction unityAction1 = new UnityAction(() => { Debug.Log("확인1"); });
        UnityAction unityAction2 = new UnityAction(() => { Debug.Log("삭제1"); });

        listView.AddRow("2024-05-07 22:11", "김철수", "관리팀", "20240001", ("확인", unityAction1), ("삭제", unityAction2));
    }
}
#endif