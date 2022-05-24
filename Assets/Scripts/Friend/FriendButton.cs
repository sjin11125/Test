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
    public GameObject Content;
    public GameObject FriendPrefab;
    public GameObject LoadingObejct;
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
        LoadingObejct.SetActive(true);
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

        StartCoroutine(EnrollPost(form1));
    }

    public void AddRecFriend()          //��û���� ģ�� �߰��ϱ� ��ư ������
    {
        string f_nickname = F_nickname.text;            //�߰��Ϸ��� ģ�� ��
        Button btn = GetComponent<Button>();
        btn.interactable = false;       //��ư ��������


        WWWForm form1 = new WWWForm();
        form1.AddField("order", "addFriend");
        string[] str = F_nickname.text.Split(':');
        form1.AddField("friend_nickname", str[0]);
        form1.AddField("player_nickname", GameManager.NickName);

        StartCoroutine(SearchPost(form1));
    }
    public void RemoveFriend()          //��û���� ģ�� �߰��ϱ� ��ư ������
    {
        string f_nickname = gameObject.transform.parent.gameObject.name;            //�߰��Ϸ��� ģ�� ��


        WWWForm form1 = new WWWForm();
        form1.AddField("order", "RemoveFriend");
        string[] str = F_nickname.text.Split(':');
        form1.AddField("friend_nickname", F_nickname.text);
        form1.AddField("player_nickname", GameManager.NickName);

        StartCoroutine(RemovePost(form1));
    }
    IEnumerator RemovePost(WWWForm form)
    {
        Debug.Log("RemovePost");
        using (UnityWebRequest www = UnityWebRequest.Post(GameManager.URL, form)) // �ݵ�� using�� ����Ѵ�
        {
            yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);
            if (www.isDone)
            {
                //SearchResponse(www.downloadHandler.text);
                Destroy(gameObject.transform.parent.gameObject);
                Debug.Log("�����մ�");
            }
            else print("���� ������ �����ϴ�.");
        }

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
        LoadingObejct.SetActive(true);
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
        if (json == "")
        {
            LoadingObejct.SetActive(false);
            return;
        }
        FriendInfo friendInfo =JsonUtility.FromJson<FriendInfo>(json);
        GameObject Search = Instantiate(SearchFriendPrefab, SearchFriendContents.transform)as GameObject;
        Text[] SearchText=Search.GetComponentsInChildren<Text>();
        SearchText[0].text = friendInfo.f_nickname;
        SearchText[1].text = friendInfo.f_info;

        Image[] friendImage = Search.GetComponentsInChildren<Image>();
        for (int j = 0; j < GameManager.AllNuniArray.Length; j++)               //ģ�� ���� ����
        {
            if (GameManager.AllNuniArray[j].Image.name != friendInfo.f_image)
                continue;
            friendImage[1].sprite = GameManager.AllNuniArray[j].Image;
        }
        for (int i = 0; i < GameManager.Friends.Length; i++)
        {
            if (friendInfo.f_nickname== GameManager.Friends[i].f_nickname)      //ģ����Ͽ� ������
            {

                Button[] btn = Search.GetComponentsInChildren<Button>();
                btn[1].interactable = false;               //Ŭ�����ϰ�
            }

        }
        LoadingObejct.SetActive(false);

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
    void RequireResponse(string json)
    {
        if (json == "")
        {
            LoadingObejct.SetActive(false);
            return;
        }
        Debug.Log(json);
        
        Transform[] child = Content.GetComponentsInChildren<Transform>();           //�ϴ� �ʱ�ȭ
        for (int k = 1; k < child.Length; k++)
        {
            Destroy(child[k].gameObject);
        }

        Newtonsoft.Json.Linq.JArray j = Newtonsoft.Json.Linq.JArray.Parse(json);
        FriendInfo[] friendInfos = new FriendInfo[j.Count];
        for (int i = 0; i < j.Count; i++)
        {
            friendInfos[i] = JsonUtility.FromJson<FriendInfo>(j[i].ToString());
            Debug.Log(friendInfos[i].f_nickname);
        }


        for (int i = 0; i < friendInfos.Length; i++)
        {
            GameObject friendprefab = Instantiate(FriendPrefab, Content.transform) as GameObject;  //ģ�� ������ ����
            friendprefab.tag = "addFriend";
            Transform friendPrefabChilds = friendprefab.GetComponent<Transform>();
            friendPrefabChilds.name = friendInfos[i].f_nickname;
            Text[] friendButtonText = friendprefab.GetComponentsInChildren<Text>();
            friendButtonText[0].text = friendInfos[i].f_nickname;
            friendButtonText[1].text = friendInfos[i].f_info;


            Image[] friendImage = friendPrefabChilds.GetComponentsInChildren<Image>();
            for (int o = 0; o < GameManager.AllNuniArray.Length; o++)
            {
                if (GameManager.AllNuniArray[o].Image.name != friendInfos[i].f_image)
                    continue;
                friendImage[1].sprite = GameManager.AllNuniArray[o].Image;
            }


        }
        LoadingObejct.SetActive(false);
    }



}
