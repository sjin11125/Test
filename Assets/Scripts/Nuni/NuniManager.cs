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
    
    public IEnumerator NuniStart()          //������ �� ���� ��ũ��Ʈ���� ���� ��� �ҷ���
    {
        WWWForm form1 = new WWWForm();
        form1.AddField("order", "nuniGet");
        form1.AddField("player_nickname", GameManager.NickName);




        yield return StartCoroutine(NuniPost(form1));                        //���� ��ũ��Ʈ�� �ʱ�ȭ�ߴ��� ��������� ���

    }
    IEnumerator NuniPost(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(GameManager.URL, form)) // �ݵ�� using�� ����Ѵ�
        {
            yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);
           /* if (www.isDone) ResponseNuni(www.downloadHandler.text);
            else print("���� ������ �����ϴ�.");*/
        }

    }
    /*void ResponseNuni(string json)                          
    {
       
        if (json .Equals( "null"))
        {
            return;
        }
        if (string.IsNullOrEmpty(json))
        {
            return;
        }
        
        string[] Nunis = json.Split(',');//���ӸŴ����� �ִ� ��� ���Ϲ迭���� �ش� ���� ã�Ƽ� ������ �ִ� ���� �迭�� �ֱ�

        
        for (int j = 0; j < Nunis.Length; j++)
        {
            
            string[] Nunis_nuni = Nunis[j].Split(':');
            for (int i = 0; i < GameManager.AllNuniArray.Length; i++)
            {
                if (GameManager.AllNuniArray[i].cardName .Equals( Nunis_nuni[0]))
                {
                    Card nuni = new Card();
                    nuni.SetValue( GameManager.AllNuniArray[i]);
                    if (Nunis_nuni[1] .Equals( "T"))
                    {
                        nuni.isLock = "T";

                    }
                    else
                        nuni.isLock = "F";
                    GameManager.Instance.CharacterList.Add(nuni);
                    break;
                  
                }
            }
           

        }
 
        SceneManager.LoadScene("Main");
    }*/

}
