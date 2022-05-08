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
            for (int i = 0; i < GameManager.AllNuniArray.Length; i++)
            {
                if (GameManager.AllNuniArray[i].cardName ==Nunis[j])
                {
                    Card nuni = GameManager.AllNuniArray[i];
                    nuni.isLock = "T";
                    GameManager.CharacterList.Add(nuni);

                }
            }
        }
    }

}
