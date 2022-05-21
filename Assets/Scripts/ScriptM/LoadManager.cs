using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;
using UnityEngine.Networking;
using System;

public class LoadManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool isLoad = false;

    public GameObject buildings;
    public GameObject nunis;

    bool isLoaded;      //�ǹ� �� �ҷ��Դ���

    public GameObject RewardPannel;     //�ϰ����� �ǳ�
    //public GameObject 
    Component CopyComponent(Component original, GameObject destination)
    {
        System.Type type = original.GetType();

        Component copy = destination.AddComponent(type);
        Debug.Log(copy.GetType());
        // Copied fields can be restricted with BindingFlags
        FieldInfo[] fields = type.GetFields();
        foreach (FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }
    IEnumerator MoneyPost(WWWForm form)
    {
        Debug.Log("�ҷ�����");
        using (UnityWebRequest www = UnityWebRequest.Post(GameManager.URL, form)) // �ݵ�� using�� ����Ѵ�
        {
            yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);
            if (www.isDone)
            {

                MoneyResponse(www.downloadHandler.text);

            }  
            else print("���� ������ �����ϴ�.");
        }

    }
    void MoneyResponse(string json)                          //�ڿ� �� �ҷ�����
    {
        if (string.IsNullOrEmpty(json))
        {
            Debug.Log(json);
            return;
        }
        Debug.Log("���絷:      " + json);
        string[] moneys = json.Split('@');
        Debug.Log(moneys[0] +"    "+ moneys[1]);//
        GameManager.Money =int.Parse(moneys[0].ToString());
        GameManager.ShinMoney= int.Parse(moneys[1].ToString());

        Debug.Log("��: " + GameManager.Money);
        Debug.Log("�߱���: " + GameManager.ShinMoney);

        StartCoroutine(RewardStart());  //�ϰ���Ȯ ��������
    }
    IEnumerator Post(WWWForm form)
    {
        Debug.Log("�ҷ�����");
        using (UnityWebRequest www = UnityWebRequest.Post(GameManager.URL, form)) // �ݵ�� using�� ����Ѵ�
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
   
   
    void Response(string json)                          //�ǹ� �� �ҷ�����
    {
        if (string.IsNullOrEmpty(json))
        {
            Debug.Log(json);
            return;
        }
        Debug.Log("josn:      " + json);

        if (json == "Null")
        {
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

            /*  new Building(friendBuildings.isLock, friendBuildings.Building_name, friendBuildings.Reward, friendBuildings.Info, 
              friendBuildings.Building_Image, friendBuildings.Cost.ToString(), friendBuildings.Level.ToString(), friendBuildings.Tree.ToString(),
               friendBuildings.Grass.ToString(), friendBuildings.Snow.ToString(), friendBuildings.Ice.ToString(), friendBuildings.isFliped.ToString(), 
              friendBuildings.buildingPosiiton_x, friendBuildings.buildingPosiiton_y);*/
            GameManager.BuildingList.Add(b);      //�� �ǹ� ����Ʈ�� ����

        }
        Debug.Log("GameManager.BuildingList[0]" + GameManager.BuildingList[0].BuildingPosiiton_x);

        Debug.Log("GameManager.BuildingList[0]" + GameManager.BuildingList[0].BuildingPosiiton_x);
        isLoaded = true;
    }
    public IEnumerator RewardStart()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "questTime");
        form.AddField("player_nickname", GameManager.NickName);
        yield return StartCoroutine(RewardPost(form));
    }

    IEnumerator RewardPost(WWWForm form)
    {
        Debug.Log("RewardPost");
        using (UnityWebRequest www = UnityWebRequest.Post(GameManager.URL, form)) // �ݵ�� using�� ����Ѵ�
        {
            yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);
            if (www.isDone) Reward_response(www.downloadHandler.text);
            else print("���� ������ �����ϴ�.");
        }

    }

    void Reward_response(string json)
    {
        Debug.Log("��¥: " + json);
        string time = json;
        if (time != DateTime.Now.ToString("yyyy.MM.dd"))     //���ó�¥�� �ƴϳ� �ϰ���Ȯ ����
        {
            Debug.Log("���������� ��Ȯ�ߴ� ��¥: " + time);
            Debug.Log("���ó�¥: " + DateTime.Now.ToString("yyyy.MM.dd"));
            GameManager.isReward = true;
        }
        else
        {
            GameManager.isReward = false;               //���ó�¥�� ��Ȯ �Ұ���
        }
        Debug.Log("��Ȯ���ɿ���: " + GameManager.isReward);
    }
    //��ȭ�ε�
    //ĳ���� �ε�
    void Start()
    {
        isLoaded = false;
        GameManager.items = 0;          //������ �ʱ�ȭ

        if (SceneManager.GetActiveScene().name == "Main")
        {

            WWWForm form1 = new WWWForm();
            Debug.Log("�ǹ��ε�");
            //isMe = true;                    //�� �ǹ� �ҷ��´�!!!!!!!!!!!!!!!!
            form1.AddField("order", "getFriendBuilding");
            form1.AddField("loadedFriend", GameManager.NickName);

            isLoad = true;
            /* WWWForm form2 = new WWWForm();
             Debug.Log("��ġ���ε�");
             //isMe = true;                    //�� ��ġ�� �ҷ��´�!!!!!!!!!!!!!!!!
             form2.AddField("order", "getFriendStr");
             form2.AddField("loadedFriend", GameManager.NickName);
             StartCoroutine(Post_Str(form2)); */


            StartCoroutine(Post(form1));

            WWWForm form2 = new WWWForm();
            Debug.Log("�ڿ��ε�");
            //isMe = true;                    //�ڿ� �ҷ�����
            form2.AddField("order", "getMoney");
            form2.AddField("player_nickname", GameManager.NickName);

            StartCoroutine(MoneyPost(form2));

            if (TutorialsManager.itemIndex>13)
            {
                Camera.main.GetComponent<Transform>().position = new Vector3(0, 0, -10);
            }

        }

        Debug.Log("���ϰ���: "+GameManager.CharacterList.Count);

        if (SceneManager.GetActiveScene().name == "Main" && GameManager.CharacterList != null)       //���ξ����� �ε��ϱ�(����)
        {
            /*for (int j = 0; j < GameManager.CharacterList.Count; j++)
            {
                Debug.Log(GameManager.CharacterList[j].name);
            }*/
            Debug.Log("GameManager.: " + GameManager.CharacterList.Count);
            for (int i = 0; i < GameManager.CharacterList.Count; i++)
            {
                Debug.Log("not t.: " + i);
                Card c = GameManager.CharacterList[i];
                if (c.isLock == "T")
                {
                    GameObject nuni = Instantiate(GameManager.CharacterPrefab[c.cardImage], nunis.transform);
                    Card nuni_card = nuni.GetComponent<Card>();
                    nuni_card.SetValue(c);
                }
                else
                {
                    Debug.Log("not t.: " + c.cardName + "   " + c.isLock);
                }
            }

        }
    }



    // Update is called once per frame
    void Update()
    {

        if (GameManager.isReward == true)          //�ϰ���Ȯ �Ҽ��ִ�?
        {
            GameManager.isReward = false;
            int MyReward = 0;
            for (int i = 0; i < GameManager.BuildingList.Count; i++)
            {
                for (int j = 0; j < GameManager.BuildingArray.Length; j++)
                {
                    if (GameManager.BuildingList[i].Building_Image == GameManager.BuildingArray[j].Building_Image)
                        MyReward += GameManager.BuildingArray[j].Reward[GameManager.BuildingList[i].Level - 1];
                }
            }
            GameManager.Money += MyReward;


            WWWForm form1 = new WWWForm();
            form1.AddField("order", "questSave");
            form1.AddField("player_nickname", GameManager.NickName);
            form1.AddField("time", DateTime.Now.ToString("yyyy.MM.dd"));

            StartCoroutine(Post(form1));//���� ��Ʈ�� ���ó�¥ ������Ʈ ���ֱ�

            Debug.Log("����: "+MyReward);
            RewardPannel.SetActive(true);
            Text[] rewardText = RewardPannel.GetComponentsInChildren<Text>();
            rewardText[1].text = MyReward.ToString();
        }
        if (isLoad == true)
        {
            isLoad = false;
            //isLoad = false;
            for (int i = 0; i < GameManager.Items.Length; i++)
            {
                GameManager.Items[i] = false;
            }
            if (SceneManager.GetActiveScene().name == "Main" && GameManager.BuildingList != null)       //���ξ����� �ε��ϱ�(�� ����)
            {
                //�ǹ��ε�
                Debug.Log("GameManager.BuildingList.Count: " + GameManager.BuildingList.Count);

                for (int i = 0; i < GameManager.BuildingList.Count; i++)
                {
                    if (GameManager.BuildingList[i].isLock == "F")          //��ġ�ȵǾ��ִ�?
                        continue;

                    Building LoadBuilding = GameManager.BuildingList[i];           // ���� ������ �մ� ���� ����Ʈ�� ���� ������Ʈ
                    string BuildingName = LoadBuilding.Building_Image;        //���� ������ �ִ� ���� ����Ʈ���� ���� �̸� �θ���
                    Debug.Log(LoadBuilding.Placed);
                    Debug.Log("BuildingName: " + BuildingName);
                    GameObject BuildingPrefab = GameManager.BuildingPrefabData[BuildingName];           // �ش� �ǹ� ������
                    GameObject g = Instantiate(BuildingPrefab, new Vector3(LoadBuilding.BuildingPosition.x, LoadBuilding.BuildingPosition.y, 0), Quaternion.identity, buildings.transform) as GameObject;

                    //  Building PrefabBuilding = BuildingPrefab.GetComponent<Building>();      //�ش� �ǹ� �������� ���� ��ũ��Ʈ
                    //Component tempData = BuildingPrefab.GetComponent<Building>().GetType();
                    // PrefabBuilding = LoadBuilding;          //���������� ������ �Ͽ콺 ������Ʈ�� ���� ��ũ��Ʈ ����                                                                   
                    //�ش� �ǹ��� ������ Ŭ�� ���� �� ���� ��ũ��Ʈ ����

                    //CopyComponent(LoadBuilding, g);
                    Building g_Building = g.GetComponent<Building>();
                    g_Building.SetValue(LoadBuilding);      //���� ������ �������� ���� ��ũ��Ʈ value ���� ������ �ִ� ��ũ��Ʈ value�� ����
                    Debug.Log("IDIDIDIDID:  " + LoadBuilding.BuildingPosiiton_x);                                      //g.transform.SetParent(buildings.transform);     //buildings�� �θ�� ����

                    //Debug.Log("gm_Building.Building_Image: " + GameManager.BuildingArray[0].Building_Image);
                    for (int j = 0; j < GameManager.BuildingArray.Length; j++)
                    {
                        if (g_Building.Building_Image == GameManager.BuildingArray[j].Building_Image)
                        {
                            g_Building.Reward = GameManager.BuildingArray[j].Reward;
                            g_Building.Cost = GameManager.BuildingArray[j].Cost;
                            g_Building.ShinCost = GameManager.BuildingArray[j].ShinCost;
                        }

                    }
                    for (int j = 0; j < GameManager.StrArray.Length; j++)
                    {
                        if (g_Building.Building_Image == GameManager.StrArray[j].Building_Image)
                        {
                            g_Building.Reward = GameManager.StrArray[j].Reward;
                            g_Building.Cost = GameManager.StrArray[j].Cost;
                            g_Building.ShinCost = GameManager.StrArray[j].ShinCost;
                        }

                    }
                    Debug.Log("ididkjflsnmfld:      " + g_Building.Building_name);
                    g.name = g_Building.Id;          //�̸� �缳��

                    g_Building.Type = BuildType.Load;
                    g_Building.Place_Initial(g_Building.Type);
                    GameManager.IDs.Add(g_Building.Id);
                    Debug.Log(g.GetComponent<Building>().isFliped);
                    // g_Building.Rotation();

                }
            }
          
            else if (SceneManager.GetActiveScene().name == "FriendMain")                            //ģ�� ���� ��
            {
                for (int i = 0; i < GameManager.FriendBuildingList.Count; i++)
                {
                    Building LoadBuilding = GameManager.FriendBuildingList[i];           // ���� ������ �մ� ���� ����Ʈ�� ���� ������Ʈ
                    string BuildingName = LoadBuilding.Building_Image;        //���� ������ �ִ� ���� ����Ʈ���� ���� �̸� �θ���
                    Debug.Log(BuildingName);

                    foreach (var item in GameManager.BuildingPrefabData)
                    {
                        Debug.Log(item.Key);
                    }
                    Debug.Log(LoadBuilding.BuildingPosiiton_x);
                    Debug.Log(BuildingName);
                    GameObject g = Instantiate(GameManager.BuildingPrefabData[BuildingName], new Vector3(float.Parse(LoadBuilding.BuildingPosiiton_x), float.Parse(LoadBuilding.BuildingPosiiton_y), 0), Quaternion.identity) as GameObject;

                    Building g_Building = g.GetComponent<Building>();
                    g_Building.SetValue(LoadBuilding);
                    //g.transform.position=new Vector3(LoadBuilding.BuildingPosition.x,LoadBuilding.BuildingPosition.y, 0);
                    Debug.Log(LoadBuilding.Building_name);
                    g.name = LoadBuilding.Id;            //�̸� �缳��

                    g_Building.Type = BuildType.Load;
                    g_Building.Place(g_Building.Type);
                    Debug.Log(g.GetComponent<Building>().isFliped);
                    // g_Building.Rotation();

                }
            }

        }

    }
}