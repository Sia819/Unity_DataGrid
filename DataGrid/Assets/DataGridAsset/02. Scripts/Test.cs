using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] private ListView listView;

    // Start is called before the first frame update
    private void Start()
    {
        Button button = this.GetComponent<Button>();
        button.onClick.AddListener(AddRowTest);
    }

    private void AddColumnTest()
    {
        listView.AddColumn("1234");
    }
    
    private void AddRowTest()
    {
        listView.AddRow("1", "2", "3");
    }
}
