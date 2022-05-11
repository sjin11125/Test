using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;
using UnityEngine.Networking;

public class LoadManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool isLoad = false;

    public GameObject buildings;
    public GameObject nunis;
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
    IEnumerator Post(WWWForm form)
    {
        Debug.Log("불러오라");
        using (UnityWebRequest www = UnityWebRequest.Post(GameManager.URL, form)) // 반드시 using을 써야한다
        {
            yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);
            if (www.isDone)
            {

                Response(www.downloadHandler.text);

            }    //친구 건물 불러옴
            else print("웹의 응답이 없습니다.");
        }

    }
    void Response(string json)                          //건물 값 불러오기
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

            Debug.Log("Id: " + Buildings.Id);
            /*  new Building(friendBuildings.isLock, friendBuildings.Building_name, friendBuildings.Reward, friendBuildings.Info, 
              friendBuildings.Building_Image, friendBuildings.Cost.ToString(), friendBuildings.Level.ToString(), friendBuildings.Tree.ToString(),
               friendBuildings.Grass.ToString(), friendBuildings.Snow.ToString(), friendBuildings.Ice.ToString(), friendBuildings.isFliped.ToString(), 
              friendBuildings.buildingPosiiton_x, friendBuildings.buildingPosiiton_y);*/
            GameManager.BuildingList.Add(b);      //내 건물 리스트에 삽입

        }
        Debug.Log("GameManager.BuildingList[0]" + GameManager.BuildingList[0].BuildingPosiiton_x);
        
        Debug.Log("GameManager.BuildingList[0]" + GameManager.BuildingList[0].BuildingPosiiton_x);

    }
    //재화로드
    //캐릭터 로드
    void Start()
    {
        if (SceneManager.GetActiveScene().name=="Main")
        {
            isLoad = true;
            WWWForm form1 = new WWWForm();
            Debug.Log("건물로딩");
            //isMe = true;                    //내 건물 불러온다!!!!!!!!!!!!!!!!
            form1.AddField("order", "getFriendBuilding");
            form1.AddField("loadedFriend", GameManager.NickName);
            StartCoroutine(Post(form1));
        }
        
        
        if (isLoad==true)
        {
            //isLoad = false;
            for (int i = 0; i < GameManager.Items.Length; i++)
            {
                GameManager.Items[i] = false;
            }
             if (SceneManager.GetActiveScene().name == "Main" && GameManager.BuildingList != null)       //메인씬에서 로드하기(내 마을)
             {
                //건물로드
                Debug.Log(GameManager.BuildingList.Count);

                for (int i = 0; i < GameManager.BuildingList.Count; i++)
                {
                    if (GameManager.BuildingList[i].isLock == "F")          //배치안되어있니?
                        continue;

                    Building LoadBuilding = GameManager.BuildingList[i];           // 현재 가지고 잇는 빌딩 리스트의 빌딩 컴포넌트
                    string BuildingName = LoadBuilding.Building_Image;        //현재 가지고 있는 빌딩 리스트에서 빌딩 이름 부르기
                    Debug.Log(LoadBuilding.Placed);
                    Debug.Log("BuildingName: "+ BuildingName);
                    GameObject BuildingPrefab = GameManager.BuildingPrefabData[BuildingName];           // 해당 건물 프리팹
                    GameObject g = Instantiate(BuildingPrefab, new Vector3(LoadBuilding.BuildingPosition.x, LoadBuilding.BuildingPosition.y, 0), Quaternion.identity,buildings.transform) as GameObject;

                  //  Building PrefabBuilding = BuildingPrefab.GetComponent<Building>();      //해당 건물 프리팹의 빌딩 스크립트
                                                                                            //Component tempData = BuildingPrefab.GetComponent<Building>().GetType();
                                                                                            // PrefabBuilding = LoadBuilding;          //프리팹으로 생성된 하우스 오브젝트의 빌딩 스크립트 대입                                                                   
                                                                                            //해당 건물의 프리팹 클론 생성 후 빌딩 스크립트 복제
                    
                    //CopyComponent(LoadBuilding, g);
                    Building g_Building = g.GetComponent<Building>();
                    g_Building.SetValue(LoadBuilding);      //새로 생성된 프리팹의 빌딩 스크립트 value 값을 기존에 있던 스크립트 value값 설정
                    Debug.Log("IDIDIDIDID:  "+ LoadBuilding.BuildingPosiiton_x);                                      //g.transform.SetParent(buildings.transform);     //buildings를 부모로 설정

                    //Debug.Log("gm_Building.Building_Image: " + GameManager.BuildingArray[0].Building_Image);
                    for (int j = 0; j < GameManager.BuildingArray.Length; j++)
                    {
                        if (g_Building.Building_Image== GameManager.BuildingArray[j].Building_Image)
                        {
                            g_Building.Reward = GameManager.BuildingArray[j].Reward;
                            g_Building.Cost = GameManager.BuildingArray[j].Cost;
                            g_Building.ShinCost = GameManager.BuildingArray[j].ShinCost;
                        }
                       
                    }
                    Debug.Log("ididkjflsnmfld:      "+g_Building.Building_name);
                    g.name = g_Building.Id;          //이름 재설정

                    g_Building.Type = BuildType.Load;
                    g_Building.Place_Initial(g_Building.Type);
                    GameManager.IDs.Add(g_Building.Id);
                    Debug.Log(g.GetComponent<Building>().isFliped);
                   // g_Building.Rotation();
                   
                }
             }
             else if(SceneManager.GetActiveScene().name == "FriendMain")                            //친구 마을 씬
            {
                for (int i = 0; i < GameManager.FriendBuildingList.Count; i++)
                {
                    Building LoadBuilding = GameManager.FriendBuildingList[i];           // 현재 가지고 잇는 빌딩 리스트의 빌딩 컴포넌트
                    string BuildingName = LoadBuilding.Building_Image;        //현재 가지고 있는 빌딩 리스트에서 빌딩 이름 부르기
                    Debug.Log(BuildingName);

                    foreach (var item in GameManager.BuildingPrefabData)
                    {
                        Debug.Log(item.Key);
                    }
                    Debug.Log(LoadBuilding.BuildingPosiiton_x);
                    Debug.Log(BuildingName);
                    GameObject g = Instantiate(GameManager.BuildingPrefabData[BuildingName],new Vector3(float.Parse( LoadBuilding.BuildingPosiiton_x),float.Parse(LoadBuilding.BuildingPosiiton_y), 0),Quaternion.identity) as GameObject;

                    Building g_Building = g.GetComponent<Building>();
                    g_Building.SetValue(LoadBuilding);
                    //g.transform.position=new Vector3(LoadBuilding.BuildingPosition.x,LoadBuilding.BuildingPosition.y, 0);
                    Debug.Log(LoadBuilding.Building_name);
                    g.name = LoadBuilding.Id;            //이름 재설정

                    g_Building.Type = BuildType.Load;
                    g_Building.Place(g_Building.Type);
                    Debug.Log(g.GetComponent<Building>().isFliped);
                    // g_Building.Rotation();

                }
            }

            if (SceneManager.GetActiveScene().name == "Main" && GameManager.CharacterList != null)       //메인씬에서 로드하기(누니)
            {
                Debug.Log("GameManager.: " + GameManager.CharacterList.Count);
                for (int i = 0; i < GameManager.CharacterList.Count; i++)
                {
                    Debug.Log("not t.: " + i);
                    Card c = GameManager.CharacterList[i];
                    if (c.isLock=="T")
                    {
                       GameObject nuni= Instantiate(GameManager.CharacterPrefab[c.cardImage], nunis.transform);
                        Card nuni_card = nuni.GetComponent<Card>();
                        nuni_card.SetValue(c);
                    }
                    else
                    {  
                        Debug.Log("not t.: " + c.cardName+"   "  + c.isLock);
                    }
                }
            }

            
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }

}