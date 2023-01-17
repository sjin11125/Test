using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;

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
    public VisitorBook VB;

    public GameObject VBPrefab;             //���� ��� ������

    public InputField VBInput;
    public GameObject LoadingNuni;      //�ε� ���� ������
    public Button CloseBtn;

    public void Start()
    {
        LoadingNuni = Instantiate(GameManager.Instance.TopCanvas);


    }
 
   

    void Response(string json)                          
    {
       
        if (string.IsNullOrEmpty(json))
        {
         
            return;
        }
        if (json.Equals("null"))                          //���Ͽ� �ƹ��͵� ����
        {
            Destroy(LoadingNuni);
            return;
        }

        Newtonsoft.Json.Linq.JArray j = Newtonsoft.Json.Linq.JArray.Parse(json);
        Debug.Log("j����: "+j.Count);
      
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
            Debug.Log("friendBuildings.f_image: "+ friendBuildings.f_image);
            for (int k = 0; k < GameManager.AllNuniArray.Length; k++)
            {
                if (GameManager.AllNuniArray[k].cardImage != friendBuildings.f_image)
                    continue;
                else
                {
                    Images[1].sprite = GameManager.AllNuniArray[k].Image;
                    
                }
               
            }
        }
        Debug.Log("The End");
        Destroy(LoadingNuni);
    }
}
