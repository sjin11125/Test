using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;

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
    void Start()
    {
        if (SceneManager.GetActiveScene().name=="Main")
        {
            isLoad = true;
        }
        //��ȭ�ε�
        //ĳ���� �ε�
        if (isLoad==true)
        {
            //isLoad = false;
            for (int i = 0; i < GameManager.Items.Length; i++)
            {
                GameManager.Items[i] = false;
            }
             if (SceneManager.GetActiveScene().name == "Main" && GameManager.BuildingList != null)       //���ξ����� �ε��ϱ�(�� ����)
             {
                //�ǹ��ε�
                Debug.Log(GameManager.BuildingList.Count);

                for (int i = 0; i < GameManager.BuildingList.Count; i++)
                {
                    if (GameManager.BuildingList[i].isLock != "T")
                        continue;

                    Building LoadBuilding = GameManager.BuildingList[i];           // ���� ������ �մ� ���� ����Ʈ�� ���� ������Ʈ
                    string BuildingName = LoadBuilding.Building_Image;        //���� ������ �ִ� ���� ����Ʈ���� ���� �̸� �θ���
                    Debug.Log(LoadBuilding.Placed);

                    GameObject BuildingPrefab = GameManager.BuildingPrefabData[BuildingName];           // �ش� �ǹ� ������
                    GameObject g = Instantiate(BuildingPrefab, new Vector3(LoadBuilding.BuildingPosition.x, LoadBuilding.BuildingPosition.y, 0), Quaternion.identity,buildings.transform) as GameObject;

                  //  Building PrefabBuilding = BuildingPrefab.GetComponent<Building>();      //�ش� �ǹ� �������� ���� ��ũ��Ʈ
                                                                                            //Component tempData = BuildingPrefab.GetComponent<Building>().GetType();
                                                                                            // PrefabBuilding = LoadBuilding;          //���������� ������ �Ͽ콺 ������Ʈ�� ���� ��ũ��Ʈ ����                                                                   
                                                                                            //�ش� �ǹ��� ������ Ŭ�� ���� �� ���� ��ũ��Ʈ ����
                    
                    //CopyComponent(LoadBuilding, g);
                    Building g_Building = g.GetComponent<Building>();
                    g_Building.SetValue(LoadBuilding);      //���� ������ �������� ���� ��ũ��Ʈ value ���� ������ �ִ� ��ũ��Ʈ value�� ����
                                                            //g.transform.SetParent(buildings.transform);     //buildings�� �θ�� ����

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

                    Debug.Log(LoadBuilding.Building_name);
                    g.name = LoadBuilding.Building_name;            //�̸� �缳��

                    g_Building.Type = BuildType.Load;
                    g_Building.Place_Initial(g_Building.Type);
                    Debug.Log(g.GetComponent<Building>().isFliped);
                   // g_Building.Rotation();
                   
                }
             }
             else if(SceneManager.GetActiveScene().name == "FriendMain")                            //ģ�� ���� ��
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
                    GameObject g = Instantiate(GameManager.BuildingPrefabData[BuildingName],new Vector3(float.Parse( LoadBuilding.BuildingPosiiton_x),float.Parse(LoadBuilding.BuildingPosiiton_y), 0),Quaternion.identity) as GameObject;

                    Building g_Building = g.GetComponent<Building>();
                    g_Building.SetValue(LoadBuilding);
                    //g.transform.position=new Vector3(LoadBuilding.BuildingPosition.x,LoadBuilding.BuildingPosition.y, 0);
                    Debug.Log(LoadBuilding.Building_name);
                    g.name = LoadBuilding.Building_name;            //�̸� �缳��

                    g_Building.Type = BuildType.Load;
                    g_Building.Place(g_Building.Type);
                    Debug.Log(g.GetComponent<Building>().isFliped);
                    // g_Building.Rotation();

                }
            }

            if (SceneManager.GetActiveScene().name == "Main" && GameManager.CharacterList != null)       //���ξ����� �ε��ϱ�(����)
            {
                for (int i = 0; i < GameManager.CharacterList.Count; i++)
                {
                    Card c = GameManager.CharacterList[i];
                    Debug.Log(c.cardImage);
                    if (c.isLock=="T")
                    {
                        Instantiate(GameManager.CharacterPrefab[c.cardImage], nunis.transform);
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