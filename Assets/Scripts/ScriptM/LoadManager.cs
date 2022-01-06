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
            isLoad = false;
            for (int i = 0; i < GameManager.Items.Length; i++)
            {
                GameManager.Items[i] = false;
            }
             if (SceneManager.GetActiveScene().name == "Main" && GameManager.BuildingArray != null)       //���ξ����� �ε��ϱ�
             {
                //�ǹ��ε�

                for (int i = 0; i < GameManager.BuildingArray.Length; i++)
                {
                    Building LoadBuilding = GameManager.BuildingArray[i];           // ���� ������ �մ� ���� ����Ʈ�� ���� ������Ʈ
                    string BuildingName = LoadBuilding.Building_Image;        //���� ������ �ִ� ���� ����Ʈ���� ���� �̸� �θ���
                    Debug.Log(LoadBuilding.Placed);

                    GameObject BuildingPrefab = GameManager.BuildingPrefabData[BuildingName];           // �ش� �ǹ� ������
                    GameObject g = Instantiate(BuildingPrefab, new Vector3(LoadBuilding.BuildingPosition.x, LoadBuilding.BuildingPosition.y, 0), Quaternion.identity) as GameObject;

                  //  Building PrefabBuilding = BuildingPrefab.GetComponent<Building>();      //�ش� �ǹ� �������� ���� ��ũ��Ʈ
                                                                                            //Component tempData = BuildingPrefab.GetComponent<Building>().GetType();
                                                                                            // PrefabBuilding = LoadBuilding;          //���������� ������ �Ͽ콺 ������Ʈ�� ���� ��ũ��Ʈ ����                                                                   
                                                                                            //�ش� �ǹ��� ������ Ŭ�� ���� �� ���� ��ũ��Ʈ ����
                    
                    //CopyComponent(LoadBuilding, g);
                    Building g_Building = g.GetComponent<Building>();
                    g_Building.SetValue(LoadBuilding);      //���� ������ �������� ���� ��ũ��Ʈ value ���� ������ �ִ� ��ũ��Ʈ value�� ����
                    //g.transform.SetParent(buildings.transform);     //buildings�� �θ�� ����
                    Debug.Log(LoadBuilding.Building_name);
                    g.name = LoadBuilding.Building_name;            //�̸� �缳��

                    g_Building.Type = BuildType.Load;
                    g_Building.Place(g_Building.Type);
                    Debug.Log(g.GetComponent<Building>().isFliped);
                   // g_Building.Rotation();
                   
                }
             }

            if (SceneManager.GetActiveScene().name == "Main" && GameManager.CharacterList != null)       //���ξ����� �ε��ϱ�
            {
                for (int i = 0; i < GameManager.CharacterList.Count; i++)
                {
                    Card c = GameManager.CharacterList[i];
                    Debug.Log(c.cardImage);

                    Instantiate(GameManager.CharacterPrefab[c.cardImage]);
                }

            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }

}