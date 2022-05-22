using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public class FriendInfo
{
    public string f_nickname;      //플레이어 닉네임
    //public string SheetsNum;     //플레이어 건물 정보 들어있는 스프레드 시트 id
    public string f_info;          //상태메세지
    public string f_id;
    public string f_image;

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
    //FriendInfo[] ;
     string URL = GameManager.URL;
    public FriendInfo Fr;

    public GameObject FriendPrefab;
    public void FriendWindowOpen()
    {
        Content.SetActive(true);
        GetFriendLsit();
    }
    public void GetFriendLsit()         //친구 정보 불러오기
    {


        WWWForm form = new WWWForm();
        form.AddField("order", "getFriend");
        form.AddField("id", "1234");
        form.AddField("player_nickname", GameManager.NickName);
        form.AddField("info", "1234");
        StartCoroutine(ListPost(form));
    }
    public void GetRecFriendLsit()         //추천친구 정보 불러오기
    {


        WWWForm form = new WWWForm();
        form.AddField("order", "RecoommendFriend");
        form.AddField("id", "1234");
        form.AddField("player_nickname", GameManager.NickName);
        StartCoroutine(Post(form));
    }

    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) // 반드시 using을 써야한다
        {
            yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);
            if (www.isDone) Response(www.downloadHandler.text);
            else print("웹의 응답이 없습니다.");
        }
    }
    IEnumerator ListPost(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) // 반드시 using을 써야한다
        {
            yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);
            if (www.isDone) ListResponse(www.downloadHandler.text);
            else print("웹의 응답이 없습니다.");
        }
    }
    void ListResponse(string json)
    {
        if (string.IsNullOrEmpty(json)) return;

        Debug.Log(json);

        Newtonsoft.Json.Linq.JArray j = Newtonsoft.Json.Linq.JArray.Parse(json);
        FriendInfo[] friendInfos = new FriendInfo[j.Count];
        for (int i = 0; i < j.Count; i++)
        {
            friendInfos[i] = JsonUtility.FromJson<FriendInfo>(j[i].ToString());
            if (friendInfos[i].f_nickname=="")      //친구가 없다
            {
                return;
            }
            Debug.Log(friendInfos[i].f_nickname);
        }
        GameManager.Friends = friendInfos;

        Transform[] child = Content.GetComponentsInChildren<Transform>();           //일단 초기화
        for (int k = 1; k < child.Length; k++)
        {
            Destroy(child[k].gameObject);
        }
        for (int i = 0; i < GameManager.Friends.Length; i++)
        {
            string[] friend = GameManager.Friends[i].f_nickname.Split(':');
            Debug.Log(friend.Length);
            if (friend.Length>=2)
            {
                continue;
            }
          
          
            GameObject friendprefab = Instantiate(FriendPrefab, Content.transform) as GameObject;  //친구 프리팹 생성
            Transform friendPrefabChilds = friendprefab.GetComponent<Transform>();
            friendPrefabChilds.name = GameManager.Friends[i].f_nickname;
            Text[] friendButtonText = friendprefab.GetComponentsInChildren<Text>();
            friendButtonText[0].text = friend[0];
            friendButtonText[1].text = GameManager.Friends[i].f_info;
        }           //친구 목록 세팅

    }
    void Response(string json)
    {
        if (string.IsNullOrEmpty(json)) return;

        Debug.Log(json);

        Transform[] child = Content.GetComponentsInChildren<Transform>();           //일단 초기화
        for (int k = 1; k < child.Length; k++)
        {
            Destroy(child[k].gameObject);
        }

        Newtonsoft.Json.Linq.JArray j= Newtonsoft.Json.Linq.JArray.Parse(json);
        FriendInfo[] friendInfos=new FriendInfo[j.Count];
        for (int i = 0; i < j.Count; i++)
        {
            friendInfos[i] = JsonUtility.FromJson<FriendInfo>(j[i].ToString());
            Debug.Log(friendInfos[i].f_nickname);
        }
        GameManager.Friends = friendInfos;
        FriendsList();              //친구 목록 세팅

    }

    public void FriendsList()
    {
        Transform[] child = Content.GetComponentsInChildren<Transform>();           //일단 초기화
        for (int k = 1; k < child.Length; k++)
        {
            Destroy(child[k].gameObject);
        }
        for (int i = 0; i < GameManager.Friends.Length; i++)
        {
            GameObject friendprefab = Instantiate(FriendPrefab, Content.transform) as GameObject;  //친구 프리팹 생성
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
