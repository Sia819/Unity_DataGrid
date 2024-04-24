using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
        listView.AddColumn("날짜",       width: 470f, fontSize: 50f);
        listView.AddColumn("이름",       width: 328f, fontSize: 50f);
        listView.AddColumn("소속",       width: 192f, fontSize: 50f);
        listView.AddColumn("사번",       width: 285f, fontSize: 50f);
        listView.AddColumn("부재",       width: 205f, fontSize: 50f);
        listView.AddColumn("도료",       width: 210f, fontSize: 50f);
        listView.AddColumn("팁사이즈",   width: 208f, fontSize: 50f);
        listView.AddColumn("토출압",     width: 224f, fontSize: 50f);
        listView.AddColumn("희석률",     width: 180f, fontSize: 50f);
        listView.AddColumn("도색차수",   width: 194f, fontSize: 50f);
        listView.AddColumn("성적서 조회", width: 336f, fontSize: 50f);
        listView.AddColumn("성적서 삭제", width: 336f, fontSize: 50f);

        
        UnityAction unityAction1 = new UnityAction(() => { Debug.Log("조회1"); });
        listView.AddRow("2024-04-25 22:11", "이지은", "관리팀", "20200003", "소부재", "EH2351", "524", "5.50bar", "20%", "2", ("조회", unityAction1), "삭제");


        //listView.AddRow("2024-04-25 22:11", "이지은", "관리팀", "20200003", "소부재", "EH2351", "524", "5.50bar", "20%", "2", "조회", "삭제");
        //listView.AddRow("2024-04-25 22:11", "이지은", "관리팀", "20200003", "소부재", "EH2351", "524", "5.50bar", "20%", "2", "조회", "삭제");
        //listView.AddRow("2024-04-25 22:11", "이지은", "관리팀", "20200003", "소부재", "EH2351", "524", "5.50bar", "20%", "2", "조회", "삭제");
        //listView.AddRow("2024-04-25 22:11", "이지은", "관리팀", "20200003", "소부재", "EH2351", "524", "5.50bar", "20%", "2", "조회", "삭제");
        //listView.AddRow("2024-04-25 22:11", "이지은", "관리팀", "20200003", "소부재", "EH2351", "524", "5.50bar", "20%", "2", "조회", "삭제");
        //listView.AddRow("2024-04-25 22:11", "이지은", "관리팀", "20200003", "소부재", "EH2351", "524", "5.50bar", "20%", "2", "조회", "삭제");
    }
}

