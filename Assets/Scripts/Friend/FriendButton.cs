using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class FriendButton : MonoBehaviour
{
    public InputField FriendNickname;
    Button SearchButton;

    public GameObject SearchFriendPrefab;
    public GameObject SearchFriendContents;

    public Text F_nickname;
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.tag=="FriendSearch")
        {
            SearchButton = gameObject.GetComponent<Button>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterFriend()               //ģ���� ������ ����
    {
        string friendNickname = gameObject.name;


    }

    public void RequireFriend()         //���� ģ�� ��û ����
    {
        WWWForm form1 = new WWWForm();
        form1.AddField("order", "requireFriend");
        form1.AddField("player_nickname", GameManager.NickName);

        StartCoroutine(RequirePost(form1));
    }
    public void EnrollFriend()          //ģ�� �߰��ϱ� ��ư ������
    {
        string f_nickname = F_nickname.text;            //�߰��Ϸ��� ģ�� ��

        WWWForm form1 = new WWWForm();
        form1.AddField("order", "EnrollFriend");
        form1.AddField("friend_nickname", F_nickname.text);
        form1.AddField("player_nickname", GameManager.NickName);

        StartCoroutine(SearchPost(form1));
    }
    IEnumerator EnrollPost(WWWForm form)
    {
        Debug.Log("EnrollPost");
        using (UnityWebRequest www = UnityWebRequest.Post(GameManager.URL, form)) // �ݵ�� using�� ����Ѵ�
        {
            yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);
            if (www.isDone)
            {
                //SearchResponse(www.downloadHandler.text);
                Debug.Log("�����մ�");
            }
            else print("���� ������ �����ϴ�.");
        }

    }
    public void SearchFriend()              //ģ�� �˻� ��ư ������
    {
       // SearchButton.OnSubmit();
       Transform[] ContentsChild= SearchFriendContents.GetComponentsInChildren<Transform>();        //�� �����
        for (int i = 1; i < ContentsChild.Length; i++)
        {
            Destroy(ContentsChild[i].gameObject);
        }


        WWWForm form1 = new WWWForm();
        form1.AddField("order", "SearchFriend");
        form1.AddField("friend_nickname", FriendNickname.text);

        StartCoroutine(SearchPost(form1));                        
    }

    IEnumerator SearchPost(WWWForm form)
    {
        Debug.Log("SearchPost");
        using (UnityWebRequest www = UnityWebRequest.Post(GameManager.URL, form)) // �ݵ�� using�� ����Ѵ�
        {
            yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);
            if (www.isDone)
            {
                SearchResponse(www.downloadHandler.text);
                Debug.Log("�����մ�");
            }
            else print("���� ������ �����ϴ�.");
        }

    }
    void SearchResponse(string json)
    {
        Debug.Log(json);
        FriendInfo friendInfo =JsonUtility.FromJson<FriendInfo>(json);
        GameObject Search = Instantiate(SearchFriendPrefab, SearchFriendContents.transform)as GameObject;
        Text[] SearchText=Search.GetComponentsInChildren<Text>();
        SearchText[0].text = friendInfo.f_nickname;
        SearchText[1].text = friendInfo.f_info;
    }


    IEnumerator RequirePost(WWWForm form)               //���� ģ�� ��û
    {
        Debug.Log("RequirePost");
        using (UnityWebRequest www = UnityWebRequest.Post(GameManager.URL, form)) // �ݵ�� using�� ����Ѵ�
        {
            yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);
            if (www.isDone)
            {
                RequireResponse(www.downloadHandler.text);
                Debug.Log("�����մ�");
            }
            else print("���� ������ �����ϴ�.");
        }

    }
    void RequireResponse(string json)                   //��û�� ģ�� ��� �߰�
    {
        Debug.Log(json);
        Newtonsoft.Json.Linq.JArray j = Newtonsoft.Json.Linq.JArray.Parse(json);
        FriendInfo[] friendInfos = new FriendInfo[j.Count];
        for (int i = 0; i < j.Count; i++)
        {
            friendInfos[i] = JsonUtility.FromJson<FriendInfo>(j[i].ToString());
            Debug.Log(friendInfos[i].f_nickname);
        }

        RequestFriendsList(friendInfos);              //��û�� ģ�� ��� ����
    }
    public void RequestFriendsList(FriendInfo[] friendInfos)
    {
        for (int i = 0; i < friendInfos.Length; i++)
        {
            GameObject Search =  Instantiate(SearchFriendPrefab, SearchFriendContents.transform) as GameObject;  //ģ�� ������ ����
            //GameObject Search = friendprefab.GetComponent<Transform>();
            Search.name= friendInfos[i].f_nickname;

            Text[] SearchText=Search.GetComponentsInChildren<Text>();

            SearchText[0].text = friendInfos[i].f_nickname;
            SearchText[1].text = friendInfos[i].f_info;
        }
    }
    public void AddFriend()         //��û�� ģ�� ���� ��ư ������
    {
        
        WWWForm form1 = new WWWForm();
        form1.AddField("order", "addFriend");
        form1.AddField("player_nickname", GameManager.NickName);
        form1.AddField("friend_nickname", gameObject.transform.parent.name);

        StartCoroutine(AddPost(form1));

    }
    IEnumerator AddPost(WWWForm form)               //��û ���� ģ�� �޾��ֱ�
    {
        Debug.Log("RequirePost");
        using (UnityWebRequest www = UnityWebRequest.Post(GameManager.URL, form)) // �ݵ�� using�� ����Ѵ�
        {
            yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);
        }

    }
}
