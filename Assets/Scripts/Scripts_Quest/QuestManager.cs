using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;

[Serializable]
public class QuestInfo   //���� ��ũ��Ʈ�� ����� ����Ʈ ����
{
    public string quest,count;           //����Ʈ([�ڵ�:Ƚ��])

}
public class QuestManager : MonoBehaviour
{


    TextAsset csvData;
    //string[][] Questlist;             //����Ʈ ����Ʈ(�ϴ� 3���� �ϰ� ���߿� �߰���)
    List<string[]> Questlist;
    public Text[] QuestText;         //����Ʈ �ؽ�Ʈ��
    string[][] Quest;

    QuestInfo[] QuestArray;          //����Ʈ �����Ȳ �迭

    // Start is called before the first frame update
    void Start()                //������ �� ����Ʈ ��� �ҷ���
    {

        Questlist = new List<string[]>();

        Quest = new string[Questlist.Count][];

        csvData = Resources.Load<TextAsset>("Quest");
        string[] data = csvData.text.Split(new char[] { '\n' });    //���� �������� �ɰ�.

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] pro_data = data[i].Split(',');
            Debug.Log(pro_data[0]);

            Questlist.Add(pro_data); //����Ʈ ����Ʈ�� �ֱ�
        }

        Quest = Questlist.ToArray();

        if (GameManager.Quest == null)
        {
            Debug.Log("����Ʈ�� ��");
        }
        for (int i = 0; i < Questlist.Count; i++)
        {
            Debug.Log(Quest[i][0]);
            GameManager.Quest.Add(Quest[i][0], 0); //���ӸŴ����� �ִ� ����Ʈ ��ųʸ��� ����Ʈ �ڵ� �ֱ�
                                                   //���� ��ũ��Ʈ�� ����Ʈ ����Ʈ ������Ʈ
            QuestSave(Quest[i][0]);                             // ó���� ����Ʈ �ƹ��͵� �������ϱ� 0���� ����
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

        StartCoroutine(Post(form1));


    }
    public void QuestClick()            //����Ʈ ��ư Ŭ������ �� ����Ʈ ���� ��Ȳ �ҷ���
    {
        for (int i = 0; i < QuestText.Length; i++)
        {
            WWWForm form1 = new WWWForm();                                      //�����Ȳ �ҷ���
            form1.AddField("order", "questGet");
            form1.AddField("player_nickname", GameManager.NickName);
            form1.AddField("quest", Quest[i][1]);

            StartCoroutine(Post(form1));



            QuestText[i].text = Quest[i][1]+"    ("+ QuestArray[i].count+ "/"+Quest[i][2]+")";                    //����Ʈ ���� �߰���
        }
    }

    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(GameManager.URL, form)) // �ݵ�� using�� ����Ѵ�
        {
            yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);
            if (www.isDone) Response(www.downloadHandler.text);         //ģ�� �ǹ� �ҷ���
                                                                        //else print("���� ������ �����ϴ�.");*/
        }

    }
    void Response(string json)                          //����Ʈ �����Ȳ �ҷ�����
    {
        //List<QuestInfo> Questlist = new List<QuestInfo>();
        Debug.Log(json);
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
        QuestArray[QuestArray.Length] = questInfo;
       


    }
}
