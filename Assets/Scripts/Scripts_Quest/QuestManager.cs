using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;
using Random = System.Random;

[Serializable]
public class QuestInfo   //���� ��ũ��Ʈ�� ����� ����Ʈ ����
{
    public string quest,count,title;           //����Ʈ�ڵ�, Ƚ��

}
public class QuestManager : MonoBehaviour
{


    TextAsset csvData;             //����Ʈ ����Ʈ(�ϴ� 3���� �ϰ� ���߿� �߰���)
    List<QuestInfo> Questlist;
    public Text[] QuestText;         //����Ʈ �ؽ�Ʈ��
    QuestInfo[] Quest;

    QuestInfo[] QuestArray=new QuestInfo[3];          //����Ʈ �����Ȳ �迭
    List<QuestInfo> GetQuestList = new List<QuestInfo>();
    bool isStart = false;

    string isReset;           //���� �ʱ�ȭ ����
    // Start is called before the first frame update
    public IEnumerator QuestStart()                //������ �� ���� ��ũ��Ʈ���� ���� �ʱ�ȭ �ƴ��� Ȯ���ϰ� ����Ʈ ��� �ҷ���
    {
        //���� �ʱ�ȭ �ߴ��� Ȯ�� �������� �ʱ�ȭ�ϰ� ������ �׳� �����Ȳ �ҷ���
        WWWForm form1 = new WWWForm();
        form1.AddField("order", "questTime");
        form1.AddField("player_nickname", GameManager.NickName);




       yield return StartCoroutine(TimePost(form1));                        //���� ��ũ��Ʈ�� �ʱ�ȭ�ߴ��� ��������� ���


    }
    void Start()
    {
        if (GameManager.QParse==false)
        {
            GameManager.QParse = true;

            Questlist = new List<QuestInfo>();                       //�׷� �������� ������ ����Ʈ ��� �� ����Ʈ �ʱ�ȭ���ְ� ���� ��ũ��Ʈ���� ������Ʈ

            Quest = new QuestInfo[3];

            csvData = Resources.Load<TextAsset>("Quest");
            string[] data = csvData.text.Split(new char[] { '\n' });    //���� �������� �ɰ�. 

            for (int i = 1; i < data.Length - 1; i++)
            {
                string[] pro_data = data[i].Split(',');
                QuestInfo qeustOne = new QuestInfo();
                qeustOne.quest = pro_data[0];
                qeustOne.count = pro_data[2];
                qeustOne.title = pro_data[1];
                Questlist.Add(qeustOne); //����Ʈ ����Ʈ�� �ֱ�
            }
            GameManager.Quest = Questlist.ToArray();
        }
      
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void QuestSave(string quest)
    {
        WWWForm form1 = new WWWForm();
        form1.AddField("order", "questSave");
        form1.AddField("player_nickname", GameManager.NickName);
        form1.AddField("quest", quest);
        form1.AddField("isReset", isReset);
        form1.AddField("time", DateTime.Now.ToString("yyyy.MM.dd"));

        StartCoroutine(Post(form1));


    }
    public void QuestClick()            //����Ʈ ��ư Ŭ������ �� ����Ʈ ���� ��Ȳ �ҷ���
    {
        for (int i = 0; i < 3; i++)
        {
            WWWForm form1 = new WWWForm();                                      //�����Ȳ �ҷ���
            form1.AddField("order", "questGet");
            form1.AddField("player_nickname", GameManager.NickName);
            form1.AddField("quest", GameManager.Quest[i].quest);

            StartCoroutine(Post(form1));
        }
       
    }

    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(GameManager.URL, form)) // �ݵ�� using�� ����Ѵ�
        {
            yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);
            if (www.isDone) Response(www.downloadHandler.text);  
                                                                        //else print("���� ������ �����ϴ�.");*/
        }

    }
    IEnumerator TimePost(WWWForm form)
    {
        Debug.Log("TimePost");
        using (UnityWebRequest www = UnityWebRequest.Post(GameManager.URL, form)) // �ݵ�� using�� ����Ѵ�
        {
            yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);
            if (www.isDone) Response_Time(www.downloadHandler.text);
            else print("���� ������ �����ϴ�.");
        }

    }
    void Response_Time(string json)                          //����Ʈ �ʱ�ȭ Ȯ��
    {
        Debug.Log(json);
       

        if (string.IsNullOrEmpty(json))
        {
            Debug.Log(json);
            return;
        }

        if (json == DateTime.Now.ToString("yyyy.MM.dd"))                         //�ҷ��� ��¥�� ���� ��¥�� �ʱ�ȭ ���� true
        {
            //GameManager.isReset = true;
            isReset = "true";
            GameManager.Quest = Questlist.ToArray();
            QuestClick();    //�ʱ�ȭ ������ ���� ��ũ��Ʈ���� �����Ȳ�� �ҷ����� ���� ��ũ��Ʈ�� ���ó�¥ �־�

            return;
        }
        else                                                                    //�ҷ��� ��¥�� ������ �ƴϰ�(�ʱ�ȭ ���ߴٸ�) ���� �����ߴٸ� false
        {
            isReset = "false";
            // GameManager.isReset = false;


          
            GameManager.Quest = Questlist.ToArray();

            if (GameManager.Quest == null)
            {
                Debug.Log("����Ʈ�� ��");
            }

            for (int i = 0; i < 3; i++)                   //����Ʈ ���
            {
                QuestSave(GameManager.Quest[UnityEngine.Random.Range(0,14)].quest);                             // ó���� ����Ʈ �ƹ��͵� �������ϱ� 0���� ����
            }
        }
        isStart = true;


    }
        void Response(string json)                          //����Ʈ �����Ȳ �ҷ�����
    {
        //List<QuestInfo> Questlist = new List<QuestInfo>();
        Debug.Log("Quest: "+json);
        if (json=="null")
        {
            return;
        }
        if (string.IsNullOrEmpty(json))
        {
            Debug.Log(json);
            return;
        }
        
        QuestInfo questInfo = JsonUtility.FromJson<QuestInfo>(json);
        
        GetQuestList.Add(questInfo);
        Debug.Log(GetQuestList.Count);
        GameManager.QuestProgress=GetQuestList.ToArray();               //���ӸŴ����� �ִ� ����Ʈ �����Ȳ �迭�� �ֱ�


        for (int i = 0; i < GameManager.QuestProgress.Length; i++)              //���ӸŴ����� �ִ� ����Ʈ �����Ȳ �ҷ��ͼ� ui�� ����
        {
            QuestText[i].text = GameManager.QuestProgress[i].quest + "   (" + GameManager.QuestProgress[i].count + "/" + GameManager.Quest[i].count + ")";
        }
        //
    }

    public void QuestExit()
    {
        GetQuestList.Clear();               //����Ʈ �����Ȳ ����Ʈ �ʱ�ȭ
    }
}
