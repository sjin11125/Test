using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool isLoad = false;
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
            /* if (SceneManager.GetActiveScene().name == "Main" && GameManager.BuildingArray != null)       //���ξ����� �ε��ϱ�
             {
                 Debug.Log("hello");
                 //�ǹ��ε�
                 Debug.Log(GameManager.BuildingArray.Length);

                 for (int i = 0; i < GameManager.BuildingArray.Length; i++)
                 {
                     Building LoadBuilding = GameManager.BuildingArray[i].GetComponent<Building>();           // ���� ������ �մ� ���� ����Ʈ�� ���� ������Ʈ
                     string BuildingName = LoadBuilding.Building_Image;        //���� ������ �ִ� ���� ����Ʈ���� ���� �̸� �θ���
                     Debug.Log(LoadBuilding.BuildingPosition);

                     GameObject BuildingPrefab = GameManager.BuildingPrefabData[BuildingName];           // �ش� �ǹ� ������
                    GameObject g= Instantiate(BuildingPrefab, new Vector3(LoadBuilding.BuildingPosition.x, LoadBuilding.BuildingPosition.y, 0), Quaternion.identity);

                     Building PrefabBuilding = BuildingPrefab.GetComponent<Building>();      //�ش� �ǹ� �������� ���� ��ũ��Ʈ
                                                                                             //Component tempData = BuildingPrefab.GetComponent<Building>().GetType();


                     foreach (var item in LoadBuilding.GetType().GetFields())
                     {
                         item.SetValue(PrefabBuilding, item.GetValue(LoadBuilding));
                     }
                     PrefabBuilding = LoadBuilding.DeepCopy();
                     Debug.Log("level: " + PrefabBuilding.level);
                     PrefabBuilding.Placed = true;
                     Debug.Log(PrefabBuilding.Placed);
                     Debug.Log(g.GetComponent<Building>().Placed);

                     ///----------���������� ������ �Ͽ콺 ������Ʈ�� ���� ��ũ��Ʈ �����ؾ���----------------
                     // Debug.Log(PrefabBuilding.Placed);
                     GameObject[] BuildingLevels = PrefabBuilding.Buildings();

                     if (PrefabBuilding.level == 2)
                     {
                         BuildingLevels[1].SetActive(true);
                     }
                     else if (PrefabBuilding.level == 3)
                     {
                         BuildingLevels[1].SetActive(true);
                         BuildingLevels[2].SetActive(true);
                     }
                     else
                     {
                         BuildingLevels[0].SetActive(true);
                     }

                 }
             }*/
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