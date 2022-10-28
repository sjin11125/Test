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

    
    
    public void UpdateValue(Building update_building)
    {
        Debug.Log("UpdateValue");
        WWWForm form = new WWWForm();
        form.AddField("order", "updateValue");
        Debug.Log("building_image"+update_building.Building_Image);
        form.AddField("building_image", update_building.Building_Image);
        form.AddField("player_nickname", GameManager.NickName);
        form.AddField("buildingPosiiton_x", update_building.BuildingPosition.x.ToString());
        form.AddField("buildingPosiiton_y", update_building.BuildingPosition.y.ToString());
        form.AddField("isLock", update_building.isLock);
        form.AddField("building_name", update_building.Building_name);
        form.AddField("level", update_building.Level);
        form.AddField("isFlied", update_building.isFliped.ToString());
        form.AddField("id", update_building.Id.ToString());

        StartCoroutine(SavePost(form));
    }
    
    public void AddValue()
    {
        WWWForm form = new WWWForm();
        Building buildings = GetComponent<Building>();
        Debug.Log("�ǹ�����");
        form.AddField("order", "addValue");
        form.AddField("player_nickname", GameManager.NickName);
        form.AddField("building_image", buildings.Building_Image);
        form.AddField("buildingPosiiton_x", buildings.BuildingPosition.x.ToString());
        form.AddField("buildingPosiiton_y", buildings.BuildingPosition.y.ToString());
        form.AddField("isLock", buildings.isLock);
        form.AddField("building_name", buildings.Building_name);
        form.AddField("level", buildings.Level);
        form.AddField("isFlied",buildings.isFliped.ToString());
        form.AddField("id", buildings.Id.ToString());
        StartCoroutine(SavePost(form));
    }
    public void RemoveValue(string id)
    {
        WWWForm form1 = new WWWForm();
        form1.AddField("order", "removeValue");
        form1.AddField("player_nickname", GameManager.NickName);
        Debug.Log("ID: "+id);
        form1.AddField("id", id);
        StartCoroutine(Post(form1));

        return;
    } public void RemoveValue_str(string id)
    {
        WWWForm form1 = new WWWForm();
        form1.AddField("order", "removeValueStr");
        form1.AddField("player_nickname", GameManager.NickName);
        Debug.Log("ID: "+id);
        form1.AddField("id", id);
        StartCoroutine(Post(form1));

        return;
    }
    public void BuildingLoad()              //�α��� ���� �� �ǹ� �ҷ���
    {
        WWWForm form1 = new WWWForm();
        Debug.Log("�ǹ��ε�");
        isMe = true;                    //�� �ǹ� �ҷ��´�!!!!!!!!!!!!!!!!
        form1.AddField("order", "getFriendBuilding");
        form1.AddField("loadedFriend", GameManager.NickName);
        StartCoroutine(Post(form1));
    } 
    public void FriendBuildindLoad()
    {
        string FriendNickname=gameObject.transform.parent.name;
        GameManager.friend_nickname = FriendNickname;           
        WWWForm form1 = new WWWForm();
        isMe = false;                   //ģ�� �ǹ� �ҷ��ð����� �޷�
        form1.AddField("order", "getFriendBuilding");
        form1.AddField("loadedFriend", FriendNickname);
        StartCoroutine(Post(form1));
    }
   
    IEnumerator Post(WWWForm form)
    {
        Debug.Log("�ҷ�����");
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) // �ݵ�� using�� ����Ѵ�
            {
                yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);
            if (www.isDone)
            {
                
                Response(www.downloadHandler.text);
                
            }    //ģ�� �ǹ� �ҷ���
            else print("���� ������ �����ϴ�.");
            
        }
        

    }
    IEnumerator SavePost(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) // �ݵ�� using�� ����Ѵ�
        {
            yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);
           // if (www.isDone) Response(www.downloadHandler.text);         //ģ�� �ǹ� �ҷ���
                                                                        //else print("���� ������ �����ϴ�.");*/
        }

    }
    
    void Response(string json)                          //�ǹ� �� �ҷ�����
    {
        if (string.IsNullOrEmpty(json))
        {
            Debug.Log(json);
            return;
        }
        Debug.Log("josn:      "+json);
       
        if (isMe .Equals( false) )               //ģ�� �ǹ� �ҷ����°Ŷ��
        {
            if (json .Equals( "Null"))
            {
                SceneManager.LoadScene("FriendMain");
                return;
            }
            GameManager.FriendBuildingList = new List<Building>();
            Newtonsoft.Json.Linq.JArray j = Newtonsoft.Json.Linq.JArray.Parse(json);
            //Debug.Log("j.Count: "+j.Count);
            BuildingParse friendBuildings = new BuildingParse();
            for (int i = 0; i < j.Count; i++)
            {
                Debug.Log(i);
                friendBuildings = JsonUtility.FromJson<BuildingParse>(j[i].ToString());
                Building b = new Building();
                b.SetValueParse(friendBuildings);

                Debug.Log("X: " + friendBuildings.BuildingPosiiton_x);
                /*  new Building(friendBuildings.isLock, friendBuildings.Building_name, friendBuildings.Reward, friendBuildings.Info, 
                  friendBuildings.Building_Image, friendBuildings.Cost.ToString(), friendBuildings.Level.ToString(), friendBuildings.Tree.ToString(),
                   friendBuildings.Grass.ToString(), friendBuildings.Snow.ToString(), friendBuildings.Ice.ToString(), friendBuildings.isFliped.ToString(), 
                  friendBuildings.buildingPosiiton_x, friendBuildings.buildingPosiiton_y);*/
                GameManager.FriendBuildingList.Add(b);      //ģ���� �ǹ� ����Ʈ�� ����

            }
            Debug.Log(GameManager.FriendBuildingList.Count);
            GameManager.isLoading = true;
            SceneManager.LoadScene("FriendMain");
        }
        else                                    //�α������� �� �� �ǹ� �ҷ����°Ŷ��
        {
            if (json .Equals( "Null"))
            {
                SceneManager.LoadScene("Main");
                return;
            }
            GameManager.BuildingList = new List<Building>();
            Newtonsoft.Json.Linq.JArray j = Newtonsoft.Json.Linq.JArray.Parse(json);
            //Debug.Log("j.Count: "+j.Count);
            BuildingParse Buildings = new BuildingParse();
            for (int i = 0; i < j.Count; i++)
            {
                Buildings = JsonUtility.FromJson<BuildingParse>(j[i].ToString());
                Building b = new Building();
                b.SetValueParse(Buildings);
                
                /*  new Building(friendBuildings.isLock, friendBuildings.Building_name, friendBuildings.Reward, friendBuildings.Info, 
                  friendBuildings.Building_Image, friendBuildings.Cost.ToString(), friendBuildings.Level.ToString(), friendBuildings.Tree.ToString(),
                   friendBuildings.Grass.ToString(), friendBuildings.Snow.ToString(), friendBuildings.Ice.ToString(), friendBuildings.isFliped.ToString(), 
                  friendBuildings.buildingPosiiton_x, friendBuildings.buildingPosiiton_y);*/
                GameManager.BuildingList.Add(b);      //�� �ǹ� ����Ʈ�� ����

            }
            GameManager.isLoading = true;
            if (gameObject.GetComponent<LoadManager>() != null)
            {
                gameObject.GetComponent<LoadManager>().BuildingLoad();
            }
            else
                SceneManager.LoadScene("Main");
            
            
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