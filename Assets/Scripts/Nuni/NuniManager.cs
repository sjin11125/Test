using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NuniManager : MonoBehaviour                    //���� �����ϰ� ���� ��ũ��Ʈ���� ������ �ִ� ���� �θ� ��
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator RewardStart()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "questTime");
        form.AddField("player_nickname", GameManager.NickName);
        yield return StartCoroutine(RewardPost(form));
    }

    IEnumerator RewardPost(WWWForm form)
    {
        Debug.Log("RewardPost");
        using (UnityWebRequest www = UnityWebRequest.Post(GameManager.URL, form)) // �ݵ�� using�� ����Ѵ�
        {
            yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);
            if (www.isDone) Reward_response(www.downloadHandler.text);
            else print("���� ������ �����ϴ�.");
        }

    }

    void Reward_response(string json)
    {
        Debug.Log("��¥: "+json);
        string time = json;
        if (time!= DateTime.Now.ToString("yyyy.MM.dd"))     //���ó�¥�� �ƴϳ� �ϰ���Ȯ ����
        {
            Debug.Log("���������� ��Ȯ�ߴ� ��¥: " + time);
            Debug.Log("���ó�¥: "+DateTime.Now.ToString("yyyy.MM.dd"));
            GameManager.isReward=true;
        }
        else
        {
            GameManager.isReward = false;               //���ó�¥�� ��Ȯ �Ұ���
        }
        Debug.Log("��Ȯ���ɿ���: "+ GameManager.isReward);
    }
    public IEnumerator NuniStart()          //������ �� ���� ��ũ��Ʈ���� ���� ��� �ҷ���
    {
        Debug.Log("NuniStart");
        WWWForm form1 = new WWWForm();
        form1.AddField("order", "nuniGet");
        form1.AddField("player_nickname", GameManager.NickName);




        yield return StartCoroutine(NuniPost(form1));                        //���� ��ũ��Ʈ�� �ʱ�ȭ�ߴ��� ��������� ���

    }
    IEnumerator NuniPost(WWWForm form)
    {
        Debug.Log("NuniPost");
        using (UnityWebRequest www = UnityWebRequest.Post(GameManager.URL, form)) // �ݵ�� using�� ����Ѵ�
        {
            yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);
            if (www.isDone) Response_Nuni(www.downloadHandler.text);
            else print("���� ������ �����ϴ�.");
        }

    }
    void Response_Nuni(string json)                          
    {
        //List<QuestInfo> Questlist = new List<QuestInfo>();
        Debug.Log(json);
        if (json == "null")
        {
            return;
        }
        if (string.IsNullOrEmpty(json))
        {
            Debug.Log(json);
            return;
        }
        
        string[] Nunis = json.Split(',');//���ӸŴ����� �ִ� ��� ���Ϲ迭���� �ش� ���� ã�Ƽ� ������ �ִ� ���� �迭�� �ֱ�
        Debug.Log(Nunis.Length);
        
        for (int j = 0; j < Nunis.Length; j++)
        {
            
            string[] Nunis_nuni = Nunis[j].Split(':');
            for (int i = 0; i < GameManager.AllNuniArray.Length; i++)
            {
                if (GameManager.AllNuniArray[i].cardName == Nunis_nuni[0])
                {
                    Card nuni = new Card();
                       Debug.Log(Nunis_nuni[0]);
                    Debug.Log(GameManager.AllNuniArray[i].cardName);
                    nuni.SetValue( GameManager.AllNuniArray[i]);
                    //Debug.Log("���ϴ� ���� "+Nunis_nuni[1]);
                    if (Nunis_nuni[1] == "T")
                    {
                        nuni.isLock = "T";

                    }
                    else
                        nuni.isLock = "F";
                    GameManager.CharacterList.Add(nuni);
                    break;
                  
                }
            }
           

        }
        Debug.Log("������ �� ������ " + GameManager.CharacterList.Count);
        for (int k = 0; k < GameManager.CharacterList.Count; k++)
        {

            Debug.Log("�� ��: " + GameManager.CharacterList[k].isLock);
        }
    }

}
