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
    public string Friends;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    
    public void UpdateValue(Building update_building)
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "updateValue");
        form.AddField("buildingPosiiton_x", update_building.BuildingPosition.x.ToString());
        form.AddField("buildingPosiiton_y", update_building.BuildingPosition.y.ToString());
        form.AddField("isLock", update_building.isLock);
        form.AddField("building_name", update_building.Building_name);
        form.AddField("cost", update_building.Cost);
        form.AddField("level", update_building.Level);
        form.AddField("tree", update_building.Tree);
        form.AddField("ice", update_building.Ice);
        form.AddField("grass", update_building.Grass);
        form.AddField("snow", update_building.Snow);
        form.AddField("isFlied", update_building.isFliped.ToString());
        StartCoroutine(Post(form));
    }
    public void AddValue()
    {
        WWWForm form = new WWWForm();
        Building buildings = GetComponent<Building>();
        Debug.Log("�ǹ�����");
        form.AddField("order", "addValue");
        form.AddField("buildingPosiiton_x", buildings.BuildingPosition.x.ToString());
        form.AddField("buildingPosiiton_y", buildings.BuildingPosition.y.ToString());
        form.AddField("isLock", buildings.isLock);
        form.AddField("building_name", buildings.Building_name);
        form.AddField("cost", buildings.Cost);
        form.AddField("level", buildings.Level);
        form.AddField("tree", buildings.Tree);
        form.AddField("ice", buildings.Ice);
        form.AddField("grass", buildings.Grass);
        form.AddField("snow", buildings.Snow);
        form.AddField("isFlied",buildings.isFliped.ToString());
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
    public void FriendBuildindLoad()
    {
        string FriendNickname=gameObject.name;
        WWWForm form1 = new WWWForm();
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
    void Response(string json)
    {
        if (string.IsNullOrEmpty(json)) return;
        Debug.Log(json);

        GameManager.FriendBuildingList = new List<Building>();
        Newtonsoft.Json.Linq.JArray j = Newtonsoft.Json.Linq.JArray.Parse(json);
        //Debug.Log("j.Count: "+j.Count);
        BuildingParse friendBuildings = new BuildingParse();
        for (int i = 0; i < j.Count; i++)
        {
            Debug.Log(i);
            friendBuildings = JsonUtility.FromJson<BuildingParse>(j[i].ToString());
            Building b=new Building();
            b.SetValueParse(friendBuildings);


              /*  new Building(friendBuildings.isLock, friendBuildings.Building_name, friendBuildings.Reward, friendBuildings.Info, 
                friendBuildings.Building_Image, friendBuildings.Cost.ToString(), friendBuildings.Level.ToString(), friendBuildings.Tree.ToString(),
                 friendBuildings.Grass.ToString(), friendBuildings.Snow.ToString(), friendBuildings.Ice.ToString(), friendBuildings.isFliped.ToString(), 
                friendBuildings.buildingPosiiton_x, friendBuildings.buildingPosiiton_y);*/
            GameManager.FriendBuildingList.Add(b);      //ģ���� �ǹ� ����Ʈ�� ����

        }
        Debug.Log(GameManager.FriendBuildingList.Count);
        SceneManager.LoadScene("FriendMain");
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
    public string Level;       //�ǹ� ����
    public string Tree;        //����
    public string Ice;        //����
    public string Grass;        //Ǯ
    public string Snow;        //��
    public string isFlied;        //������������
                               //-----------------------------------------------------------
                               //public string[] Friends;
}