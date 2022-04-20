
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


[System.Serializable]
public class GoogleData
{
    public string order, result, msg, value;
}


public class GoogleSheetManager : MonoBehaviour
{
    string URL = GameManager.URL;
        public GoogleData GD;
    public InputField IDInput, PassInput, NicknameInput;
    string id, pass,nickname;
    public QuestManager QuestManager;


    bool SetIDPass()
    {
        id = IDInput.text.Trim();
        pass = PassInput.text.Trim();
        if (id == "" || pass == "") return false;
        else return true;
    }

    bool SetSignPass()
    {
        id = IDInput.text.Trim();
        pass = PassInput.text.Trim();
        nickname = NicknameInput.text.Trim();
        if (id == "" || pass == "" || nickname == "") return false;
        else return true;
    }
    public void Register()
    {
        if (!SetSignPass())
        {
            print("���̵� or ��� or �г����� ����ֽ��ϴ�");
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("order", "register");
        form.AddField("id", id);
        form.AddField("pass", pass);
        form.AddField("player_nickname", nickname);

        StartCoroutine(Post(form));
    }


    public void Login()
    {
        if (!SetIDPass())
        {
            print("���̵� �Ǵ� ��й�ȣ�� ����ֽ��ϴ�");
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("order", "login");
        form.AddField("id", id);
        form.AddField("pass", pass);

        StartCoroutine(Post(form));
    }


    void OnApplicationQuit()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "logout");

        StartCoroutine(Post(form));
    }


    public void SetValue()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "setValue");
        form.AddField("value", NicknameInput.text);

        StartCoroutine(Post(form));
    }


    public void GetValue()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "getValue");

        StartCoroutine(Post(form));
    }





    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(GameManager.URL, form)) // �ݵ�� using�� ����Ѵ�
        {
            yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);
            if (www.isDone)
            {
                Response(www.downloadHandler.text);
                Debug.Log("�����մ�");
            }
            else print("���� ������ �����ϴ�.");
        }
    }


    void Response(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            Debug.Log(json);
            return;
        }
        Debug.Log(json);
        GD = JsonUtility.FromJson<GoogleData>(json);
        //System.Text.Encoding.UTF8.GetString(GD, 3, GD.Length - 3);
        

        if (GD.result == "ERROR")
        {
            print(GD.order + "�� ������ �� �����ϴ�. ���� �޽��� : " + GD.msg);
            return;
        }
        else if(GD.result == "NickNameERROR")
        {
            print("�г����� �ߺ��˴ϴ�.");
        }
        if (GD.result == "OK")
        {
            if (GD.msg=="ȸ������ �Ϸ�")
            {
                Debug.Log("ȸ������ �Ϸ�!");
            }
            else
            {
                nickname= GD.msg;
                Debug.Log("�α��� �Ϸ�!");
            }
            print(nickname + "(" + id + ")�� ȯ���մϴ�!! ");

            GameManager.NickName = nickname;
            GameManager.Id = id;

            QuestManager.QuestStart();                                  //����Ʈ ����

            gameObject.GetComponent<BuildingSave>().BuildingLoad();         //�� �ǹ� �ҷ���
                                                                            //����Ʈ �����Ȳ �ҷ�

            
            //MyBuildingLoad.BuildingLoad();          

            return;
        }
        if (GD.order == "getValue")
        {
            NicknameInput.text = GD.value;
        }

        Debug.Log("");
    }
}
