using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public class FriendInfo
{
    public string f_nickname;      //�÷��̾� �г���
    //public string SheetsNum;     //�÷��̾� �ǹ� ���� ����ִ� �������� ��Ʈ id
    public string f_info;          //���¸޼���
    public string f_id;

    public FriendInfo(string nickname,string id,string info)
    {
        this.f_nickname = nickname;
        this.f_id = id;   
        this.f_info = info;   
        
    }
}
public class FriendManager : MonoBehaviour
{
    public GameObject Content;
    FriendInfo[] friends;
    const string URL = "https://script.google.com/macros/s/AKfycbzjunYJ8-acQqW3hNzf7wf5SkwKgGq3Tm9qNhGDFRiwBYbsBeLw5FhwMrifh4gZxhdY/exec";
    public FriendInfo Fr;

    public GameObject FriendPrefab;
    public void FriendWindowOpen()
    {
        Content.SetActive(true);
        GetFriendLsit();
    }
    public void GetFriendLsit()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "getFriend");
        form.AddField("id", "1234");
        form.AddField("nickname", "1234");
        form.AddField("info", "1234");
        StartCoroutine(Post(form));
    }

    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) // �ݵ�� using�� ����Ѵ�
        {
            yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);
            if (www.isDone) Response(www.downloadHandler.text);
            else print("���� ������ �����ϴ�.");
        }
    }

    void Response(string json)
    {
        if (string.IsNullOrEmpty(json)) return;

        Debug.Log(json);
        
        Newtonsoft.Json.Linq.JArray j= Newtonsoft.Json.Linq.JArray.Parse(json);
        FriendInfo[] friendInfos=new FriendInfo[j.Count];
        for (int i = 0; i < j.Count; i++)
        {
            friendInfos[i] = JsonUtility.FromJson<FriendInfo>(j[i].ToString());
            Debug.Log(friendInfos[i].f_nickname);
        }
        GameManager.Friends = friendInfos;
        FriendsList();              //ģ�� ��� ����

    }

    public void FriendsList()
    {
        for (int i = 0; i < GameManager.Friends.Length; i++)
        {
            GameObject friendprefab = Instantiate(FriendPrefab, Content.transform) as GameObject;  //ģ�� ������ ����
            Transform friendPrefabChilds = friendprefab.GetComponent<Transform>();
            friendPrefabChilds.name = GameManager.Friends[i].f_nickname;
            Text[] friendButtonText = friendprefab.GetComponentsInChildren<Text>();
            friendButtonText[0].text = GameManager.Friends[i].f_nickname;
            friendButtonText[1].text = GameManager.Friends[i].f_info;
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
       /* WWWForm form = new WWWForm();

        form.AddField("order", "info");
        form.AddField("id", "1234");
        form.AddField("nickname", "1234");
        form.AddField("info", "1234");



        StartCoroutine(Post(form));*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}