using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class BuildingSave : MonoBehaviour
{               //�ǹ��� �����ϴ� ��ũ��Ʈ
                //�����ϸ� ���� �������� ��Ʈ�� ����

    Buildingsave[] BTosave;
     string URL = GameManager.URL;
    public Buildingsave GD;
    public bool isMe;       //�� �ڽ��� �ǹ��� �ҷ����°�?
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void BuildingReq(BuildingDef buildingDef,Building tempBuilding=null)
    {
        WWWForm form = new WWWForm();

        switch (buildingDef)
        {
            case BuildingDef.updateValue:
            case BuildingDef.addValue:

                form.AddField("order", buildingDef.ToString());
                form.AddField("building_image", tempBuilding.Building_Image);
                form.AddField("player_nickname", GameManager.NickName);
                form.AddField("buildingPosiiton_x", tempBuilding.BuildingPosition.x.ToString());
                form.AddField("buildingPosiiton_y", tempBuilding.BuildingPosition.y.ToString());
                form.AddField("isLock", tempBuilding.isLock);
                form.AddField("building_name", tempBuilding.Building_name);
                form.AddField("level", tempBuilding.Level);
                form.AddField("isFlied", tempBuilding.isFliped.ToString());
                form.AddField("id", tempBuilding.Id.ToString());

                StartCoroutine(Post(form, buildingDef));        //SavePost
                break;

            case BuildingDef.removeValue:

                form.AddField("order", buildingDef.ToString());
                form.AddField("player_nickname", GameManager.NickName);

                form.AddField("id", tempBuilding.Id);
                StartCoroutine(Post(form, buildingDef));
                break;

            case BuildingDef.getFriendBuilding:
                break;

            case BuildingDef.getMyBuilding:
                isMe = true;                    //�� �ǹ� �ҷ��´�!!!!!!!!!!!!!!!!
                form.AddField("order", "getFriendBuilding");
                form.AddField("loadedFriend", GameManager.NickName);
                StartCoroutine(Post(form, buildingDef));
                break;

            default:
                break;
        }
    }
    
  
    public void FriendBuildindLoad()
    {
        string FriendNickname=gameObject.transform.parent.name;
        GameManager.friend_nickname = FriendNickname;           
        WWWForm form1 = new WWWForm();
        isMe = false;                   //ģ�� �ǹ� �ҷ��ð����� �޷�
        form1.AddField("order", "getFriendBuilding");
        form1.AddField("loadedFriend", FriendNickname);
        StartCoroutine(Post(form1, BuildingDef.getFriendBuilding));
    }
   
    IEnumerator Post(WWWForm form,BuildingDef buildingDef)
    {
        Debug.Log("�ҷ�����");
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) // �ݵ�� using�� ����Ѵ�
            {
                yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);
            if (www.isDone)
            {
                switch (buildingDef)
                {
                    case BuildingDef.updateValue:
                    case BuildingDef.addValue:
                    case BuildingDef.removeValue:
                        break;

                    case BuildingDef.getFriendBuilding:
                    case BuildingDef.getMyBuilding:
                        Response(www.downloadHandler.text, buildingDef);
                        break;

                    default:
                        break;
                }
            } 
            else print("���� ������ �����ϴ�.");
            }
        
    }
    void Response(string json, BuildingDef buildingDef)                          //�ǹ� �� �ҷ�����
    {
        if (string.IsNullOrEmpty(json))
        {
            Debug.Log(json);
            return;
        }
        Debug.Log("josn:      "+json);

        Newtonsoft.Json.Linq.JArray j = Newtonsoft.Json.Linq.JArray.Parse(json);

        switch (buildingDef)
        {
            case BuildingDef.getFriendBuilding:

                if (json.Equals("Null"))  //�ǹ��� ���ٸ�
                {
                    SceneManager.LoadScene("FriendMain");
                    return;
                }

                GameManager.FriendBuildingList = new List<Building>();
  

                BuildingParse friendBuildings = new BuildingParse();

                foreach (var item in j)
                {
                    Debug.Log(item);
                    friendBuildings = JsonUtility.FromJson<BuildingParse>(item.ToString());
                    Building b = new Building();
                    b.SetValueParse(friendBuildings);

                    GameManager.FriendBuildingList.Add(b);      //ģ���� �ǹ� ����Ʈ�� ����
                }

                Debug.Log(GameManager.FriendBuildingList.Count);
                GameManager.isLoading = true;
                SceneManager.LoadScene("FriendMain");
                break;

            case BuildingDef.getMyBuilding:
                if (json.Equals("Null"))
                {
                    SceneManager.LoadScene("Main");
                    return;
                }

                BuildingParse Buildings = new BuildingParse();
                foreach (var item in j)
                {
                    Buildings = JsonUtility.FromJson<BuildingParse>(item.ToString());
                    Building b = new Building();
                    b.SetValueParse(Buildings);

                    LoadManager.AddBuildingSubject.OnNext(b); //�� �ǹ� ����Ʈ�� ����

                }
                GameManager.isLoading = true;
                if (gameObject.GetComponent<LoadManager>() != null)
                {
                    gameObject.GetComponent<LoadManager>().BuildingLoad();
                }
                else
                    SceneManager.LoadScene("Main");
                break;

            default:
                break;
        }
  
    }
}

[Serializable]
public class Buildingsave
{
    public string order, result, msg, row_size,length;

    public string BuildingPosition_x;                //�ǹ� ��ġ(x��ǥ)
    public string BuildingPosition_y;                //�ǹ� ��ġ(y��ǥ)
    //-------------------------�Ľ�����------------------------------
    public string isLock;               //��� ����
    public string Building_name;            //�ǹ� �̸�
    public string Reward;               //ȹ���ڿ�
    public string Info;                 //�ǹ� ����
    public string Building_Image;          //���� �̹��� �̸� *
    public string Cost;        //�ǹ����
    public string ShinCost;  
    public string Level;       //�ǹ� ����
    public string isFlied;        //������������
                               //-----------------------------------------------------------
                               //public string[] Friends;
}