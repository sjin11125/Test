using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class VisitorBook
{
    public string f_nickname;      //ģ��
    public string f_massage;        //ģ���� ���� �޼���
    public VisitorBook(string nickname, string message)
    {
        this.f_nickname = nickname;
        this.f_massage = message;

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
        VBWindow.SetActive(true);           
        VisitorBookList();              //���� �ֳ� Ȯ��
    }
    // Start is called before the first frame update
     public void VisitorBookList()  //���� �ҷ���
    {

    }

    public void VisitorBookWrite()          //���� ����        (������ ��ư�� �ֱ�)
    {
        Debug.Log("���� ����");
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
            //Debug.Log(www.downloadHandler.text);
            
        }
    }
}
