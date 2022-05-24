using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class VisitorBook
{
    public string f_nickname;      //ģ��
    public string f_message;        //ģ���� ���� �޼���
    public string f_time;        //ģ���� ���� �ð�
    public string f_image;        //ģ������
    public VisitorBook(string nickname, string message,string time)
    {
        this.f_nickname = nickname;
        this.f_message = message;
        this.f_time = time;
    }
}
public class VisitorBookManager : MonoBehaviour
{
    public GameObject VBWindow;
    public GameObject Content;
    //FriendInfo[] ;
    string URL = GameManager.URL;
    public VisitorBook VB;

    public GameObject VBPrefab;             //���� ��� ������

    public InputField VBInput;
    

    public void VBWindowOpen()              //���� â �������� ��
    {
        //VBWindow.SetActive(true);
        
    }

    public void Start()
    {
        VBWindowOpen();
        if (SceneManager.GetActiveScene().name.Equals("FriendMain"))    //ģ�� ���̳�
        {
            VBInput.gameObject.SetActive(true);
            if (gameObject.tag.Equals("VisitorBook"))
            {
                FriendVisitorBookList();              //ģ�� ���� �ֳ� Ȯ��
            }
            
        }
        else                                                        //�� ���̳�
        {
          
            VBInput.gameObject.SetActive(false);
            if (gameObject.tag .Equals( "VisitorBook"))
            {
                VisitorBookList();              //���� �ֳ� Ȯ��
            }
        }
    }
    // Start is called before the first frame update
    public void VisitorBookList()  //�� ���� �ҷ���
    {

        WWWForm form = new WWWForm();
        form.AddField("order", "getMessage");
        form.AddField("player_nickname", GameManager.NickName);
        //form.AddField("message", VBInput.text);
        StartCoroutine(GetPost(form));
    }

    public void FriendVisitorBookList()         //ģ�� ���� �ҷ��� 
    {

        WWWForm form = new WWWForm();
        form.AddField("order", "getMessageFriend");
        form.AddField("friend_nickname", GameManager.friend_nickname);
        //form.AddField("message", VBInput.text);
        StartCoroutine(GetPost(form));
    }

    public void VisitorBookWrite()          //���� ����        (������ ��ư�� �ֱ�)
    {
   
        WWWForm form = new WWWForm();
        form.AddField("order", "enrollMessage");
        form.AddField("player_nickname", GameManager.NickName);
        form.AddField("friend_nickname", GameManager.friend_nickname);
        form.AddField("message", VBInput.text);
        StartCoroutine(Post(form));
    }

    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) // �ݵ�� using�� ����Ѵ�
        {
            yield return www.SendWebRequest();


        }
        GameObject VB = Instantiate(VBPrefab, Content.transform) as GameObject;

        Text[] VBtext = VB.GetComponentsInChildren<Text>();

        VBtext[0].text = GameManager.NickName;
        VBtext[1].text = VBInput.text;
        VBtext[2].text = DateTime.Now.ToString("yyyy.MM.dd");
        //GameManager.FriendBuildingList.Add(b);      //ģ���� �ǹ� ����Ʈ�� ����
    }
    IEnumerator GetPost(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) // �ݵ�� using�� ����Ѵ�
        {
            yield return www.SendWebRequest();
        
            if (www.isDone) Response(www.downloadHandler.text);         //���� �ҷ���

        }
    }

    void Response(string json)                          
    {
       
        if (string.IsNullOrEmpty(json))
        {
         
            return;
        }
        if (json .Equals( "null")   )                          //���Ͽ� �ƹ��͵� ����
            return;

        Newtonsoft.Json.Linq.JArray j = Newtonsoft.Json.Linq.JArray.Parse(json);

      
        for (int i = 0; i < j.Count; i++)
        {
 
            VisitorBook friendBuildings;
            friendBuildings = JsonUtility.FromJson<VisitorBook>(j[i].ToString());

            GameObject VB = Instantiate(VBPrefab, Content.transform)as GameObject;

            Text[] VBtext = VB.GetComponentsInChildren<Text>();

            VBtext[0].text =friendBuildings.f_nickname;
            VBtext[1].text = friendBuildings.f_message;
            VBtext[2].text = friendBuildings.f_time;

            Image[] Images= VB.GetComponentsInChildren<Image>();
            for (int k = 0; k < GameManager.AllNuniArray.Length; k++)
            {
                if (GameManager.AllNuniArray[k].Image.name != friendBuildings.f_image)
                    continue;
                else
                {
                    Images[1].sprite = GameManager.AllNuniArray[k].Image;
                    return;
                }
               
            }
        }
        

    }
}
