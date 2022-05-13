
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


[System.Serializable]
public class GoogleData
{
    public string order, result, msg, value,nickname,state, profile_image;
}


public class GoogleSheetManager : MonoBehaviour
{
    string URL = GameManager.URL;
    public GoogleData GD;
    public InputField IDInput, PassInput, NicknameInput;
    string id, pass, nickname,statemessage;
    public QuestManager QuestManager;
    public NuniManager NuniManager;
    public BuildingSave MyBuildingLoad;

    public GameObject WarningPannel;

    private void Awake()
    {
        if (id != null)
        {
            id = PlayerPrefs.GetString("Id");
            pass = PlayerPrefs.GetString("Pass");
            nickname = PlayerPrefs.GetString("Nickname");
            Login();
        }
        else
        {
            GameManager.Money = 1000;
            GameManager.ShinMoney = 0;
        }
    }

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
            WarningPannel.SetActive(true);
            Text t = WarningPannel.GetComponentInChildren<Text>();
            t.text = "���̵� �Ǵ� ��й�ȣ �Ǵ� �г����� ����ֽ��ϴ�";
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("order", "register");
        form.AddField("id", id);
        form.AddField("pass", pass);
        form.AddField("player_nickname", nickname);

        StartCoroutine(SignPost(form));
    }


    public void Login()
    {
        if (!SetIDPass())
        {
            WarningPannel.SetActive(true);
            Text t = WarningPannel.GetComponentInChildren<Text>();
            t.text = "���̵� �Ǵ� ��й�ȣ�� ����ֽ��ϴ�";
           // print("���̵� �Ǵ� ��й�ȣ�� ����ֽ��ϴ�");
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("order", "login");
        form.AddField("id", id);
        form.AddField("pass", pass);

        StartCoroutine(Post(form));

        PlayerPrefs.SetString("Id", id);
        PlayerPrefs.SetString("Pass", pass);
        PlayerPrefs.SetString("Nickname", nickname);       
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



    IEnumerator SignPost(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(GameManager.URL, form)) // �ݵ�� using�� ����Ѵ�
        {
            yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);

            GameManager.NickName = nickname;
            GameManager.Id = id;
            Response(www.downloadHandler.text);
            //StartCoroutine(Quest());
            //SceneManager.LoadScene("Main");
        }
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
        WarningPannel.SetActive(true);

        Text t = WarningPannel.GetComponentInChildren<Text>();
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
            t.text=GD.msg;
            return;
        }
        else if (GD.result == "NickNameERROR")
        {
            t.text = "�г����� �ߺ��˴ϴ�.";
        }
        if (GD.result == "OK")
        {
            if (GD.msg == "ȸ������ �Ϸ�")
            {
                t.text = "ȸ������ �Ϸ�!"+ nickname + "(" + id + ")�� ȯ���մϴ�!! " +
                    "\n��ø� ��ٷ� �ּ���.";
            }
            else
            {
                nickname = GD.nickname;
               GameManager.StateMessage= GD.state;
                t.text = "�α��� �Ϸ�!"+ nickname + "(" + id + ")�� ȯ���մϴ�!! " +
                    "\n��ø� ��ٷ� �ּ��� ";
            }

            GameManager.NickName = nickname;
            GameManager.Id = id;

            Debug.Log(GameManager.AllNuniArray.Length);
            for (int i = 0; i < GameManager.AllNuniArray.Length; i++)
            {
                if (GameManager.AllNuniArray[i].Image.name != GD.profile_image)
                    continue;
                GameManager.ProfileImage = GameManager.AllNuniArray[i].Image;
            }
             
            StartCoroutine(Quest());
            


               

            return;
        }
        if (GD.order == "getValue")
        {
            NicknameInput.text = GD.value;
        }

        Debug.Log("");
    }
    IEnumerator Quest()
    {
       // gameObject.GetComponent<BuildingSave>().BuildingLoad();         //�� �ǹ� �ҷ���
        //yield return StartCoroutine( QuestManager.QuestStart()); //����Ʈ ������ ������ ���
        yield return StartCoroutine(NuniManager.NuniStart()); //���� ������ ������ ���
        yield return StartCoroutine(NuniManager.RewardStart()); //���� ������ ������ ���
        MyBuildingLoad.BuildingLoad();

    }
}
