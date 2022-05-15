using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameExitController : MonoBehaviour
{
    public void Awake()
    {
        //GameLoad();
    }
    public void GameSave()
    {/*
        PlayerPrefs.SetInt("Money", GameManager.Money);//��
        PlayerPrefs.SetInt("ShinMoney", GameManager.ShinMoney);//��
        PlayerPrefs.Save();
        print("save");*/

        WWWForm form2 = new WWWForm();
        Debug.Log("�ڿ�����");
        //isMe = true;                    //�ڿ� �ҷ�����
        form2.AddField("order", "setMoney");
        form2.AddField("player_nickname", GameManager.NickName);
        form2.AddField("money", GameManager.Money.ToString()+"|"+GameManager.ShinMoney.ToString());

        StartCoroutine(MoneyPost(form2));
    }
    IEnumerator MoneyPost(WWWForm form)
    {
        Debug.Log("�����϶�");
        using (UnityWebRequest www = UnityWebRequest.Post(GameManager.URL, form)) // �ݵ�� using�� ����Ѵ�
        {
            yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);
           /* if (www.isDone)
            {

                MoneyResponse(www.downloadHandler.text);

            }
            else print("���� ������ �����ϴ�.");*/
        }

    }

   /* void MoneyResponse(string json)                          //�ڿ� �� �ҷ�����
    {
        if (string.IsNullOrEmpty(json))
        {
            Debug.Log(json);
            return;
        }
        Debug.Log("���絷:      " + json);
        string[] moneys = json.Split('|');

        GameManager.Money = int.Parse(moneys[0]);
        GameManager.ShinMoney = int.Parse(moneys[1]);

    }*/
    public void GameLoad()
    {
        GameManager.ShinMoney = 10000;
        print("load");
    }
    public void GameExit()
    {
        GameSave();
        print("exit");
        Application.Quit();
    }
}
