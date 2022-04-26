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
        WWWForm form = new WWWForm();
        form.AddField("order", "updateValue");
        form.AddField("building_image", update_building.Building_Image);
        form.AddField("buildingPosiiton_x", update_building.BuildingPosition.x.ToString());
        form.AddField("buildingPosiiton_y", update_building.BuildingPosition.y.ToString());
        form.AddField("isLock", update_building.isLock);
        form.AddField("building_name", update_building.Building_name);
        form.AddField("cost", update_building.Cost);
        form.AddField("shinCost", update_building.ShinCost);
        form.AddField("level", update_building.Level);
        form.AddField("isFlied", update_building.isFliped.ToString());
        StartCoroutine(Post(form));
    }
    public void AddValue()
    {
        WWWForm form = new WWWForm();
        Building buildings = GetComponent<Building>();
        Debug.Log("�ǹ�����");
        form.AddField("order", "addValue");
        form.AddField("building_image", buildings.Building_Image);
        form.AddField("buildingPosiiton_x", buildings.BuildingPosition.x.ToString());
        form.AddField("buildingPosiiton_y", buildings.BuildingPosition.y.ToString());
        form.AddField("isLock", buildings.isLock);
        form.AddField("building_name", buildings.Building_name);
        form.AddField("cost", buildings.Cost);
        form.AddField("shinCost", buildings.ShinCost);
        form.AddField("level", buildings.Level);
        form.AddField("isFliped",buildings.isFliped.ToString());
        StartCoroutine(Post(form));
    }
    public void RemoveValue(string b_name)
    {
        WWWForm form1 = new WWWForm();
        form1.AddField("order", "removeValue");
        form1.AddField("remove_building", b_name);
        StartCoroutine(Post(form1));

        return;
    }
    public void BuildingLoad()              //�α��� ���� �� �ǹ� �ҷ���
    {
        WWWForm form1 = new WWWForm();
        isMe = true;                    //�� �ǹ� �ҷ��´�!!!!!!!!!!!!!!!!
        form1.AddField("order", "getFriendBuilding");
        form1.AddField("loadedFriend", GameManager.NickName);
        StartCoroutine(Post(form1));
    }
    public void FriendBuildindLoad()
    {
        string FriendNickname=gameObject.transform.parent.name;
        WWWForm form1 = new WWWForm();
        isMe = false;                   //ģ�� �ǹ� �ҷ��ð����� �޷�
        form1.AddField("order", "getFriendBuilding");
        form1.AddField("loadedFriend", FriendNickname);
        StartCoroutine(Post(form1));
    }
    IEnumerator Post(WWWForm form)
    {
            using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) // �ݵ�� using�� ����Ѵ�
            {
                yield return www.SendWebRequest();
                //Debug.Log(www.downloadHandler.text);
                if (www.isDone) Response(www.downloadHandler.text);         //ģ�� �ǹ� �ҷ���
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
        Debug.Log(json);
       
        if (isMe == false)                //ģ�� �ǹ� �ҷ����°Ŷ��
        {
            if (json == "Null")
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
            SceneManager.LoadScene("FriendMain");
        }
        else                                    //�α������� �� �� �ǹ� �ҷ����°Ŷ��
        {
            if (json == "Null")
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
                Debug.Log(i);
                Buildings = JsonUtility.FromJson<BuildingParse>(j[i].ToString());
                Building b = new Building();
                b.SetValueParse(Buildings);

                Debug.Log("X: " + Buildings.BuildingPosiiton_x);
                /*  new Building(friendBuildings.isLock, friendBuildings.Building_name, friendBuildings.Reward, friendBuildings.Info, 
                  friendBuildings.Building_Image, friendBuildings.Cost.ToString(), friendBuildings.Level.ToString(), friendBuildings.Tree.ToString(),
                   friendBuildings.Grass.ToString(), friendBuildings.Snow.ToString(), friendBuildings.Ice.ToString(), friendBuildings.isFliped.ToString(), 
                  friendBuildings.buildingPosiiton_x, friendBuildings.buildingPosiiton_y);*/
                GameManager.BuildingList.Add(b);      //�� �ǹ� ����Ʈ�� ����

            }
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