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
        
    }
    
    private void AddRowTest()
    {
        listView.UseColumnResizer = true;
        listView.AddColumn("��¥",       width: 470f, fontSize: 50f);
        listView.AddColumn("�̸�",       width: 328f, fontSize: 50f);
        listView.AddColumn("�Ҽ�",       width: 192f, fontSize: 50f);
        listView.AddColumn("���",       width: 285f, fontSize: 50f);
        listView.AddColumn("����",       width: 205f, fontSize: 50f);
        listView.AddColumn("����",       width: 210f, fontSize: 50f);
        listView.AddColumn("��������",   width: 208f, fontSize: 50f);
        listView.AddColumn("�����",     width: 224f, fontSize: 50f);
        listView.AddColumn("�񼮷�",     width: 180f, fontSize: 50f);
        listView.AddColumn("��������",   width: 194f, fontSize: 50f);
        listView.AddColumn("������ ��ȸ", width: 336f, fontSize: 50f);
        listView.AddColumn("������ ����", width: 336f, fontSize: 50f);

        listView.AddRow("2024-04-25 22:11", "������", "������", "20200003", "�Һ���", "EH2351", "524", "5.50bar", "20%", "2", "��ȸ", "����");
    }
}

