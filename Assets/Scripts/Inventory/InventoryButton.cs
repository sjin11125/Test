using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryButton : MonoBehaviour
{
    // Start is called before the first frame update
    public Button X_Image;     //�ǹ� ȸ�� ��ư

    Building this_building;         //�� ��ư�� �ش��ϴ� �ǹ�
    GridBuildingSystem gridBuildingSystem;

    public GameObject buildings;
    void Start()
    {
        buildings = GameObject.Find("buildings");
        for (int i = 0; i < GameManager.BuildingList.Count; i++)
        {
            if (this.gameObject.name== GameManager.BuildingList[i].Building_name)
            {
                this_building = GameManager.BuildingList[i];
                gridBuildingSystem=gameObject.transform.parent.parent.GetComponent<GridBuildingSystem>();
            }
        }
        Debug.Log(this_building.isLock);
        if (this_building.isLock=="F")
        {
            X_Image.gameObject.SetActive(false);
        }
        else
        {
            X_Image.gameObject.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Click()         //��ư Ŭ������ ��
    {
        if (this_building.isLock=="T")      //���� ��ġ�� �����ΰ�
        {
            this_building.isLock = "F";         //��ġ �ȵ� ���·� �ٲٱ�
            X_Image.gameObject.SetActive(true);

            Transform[] building_child=buildings.GetComponentsInChildren<Transform>();

            for (int i = 1; i < building_child.Length; i++)
            {
                if (building_child[i].gameObject.name==gameObject.name)
                {
                    Building building_childs= building_child[i].gameObject.GetComponent<Building>();
                    building_childs.isLock = "F";
                    building_childs.BuildingPosiiton_x = "0";
                    building_childs.BuildingPosiiton_y = "0";

                    building_childs.save.UpdateValue(building_childs);
                    Destroy(building_child[i].gameObject);
                }
                
            }
        }
        else                                    //���� ��ġ�� ���°� �ƴѰ�
        {
            X_Image.gameObject.SetActive(false);
            Debug.Log("image: " + this_building.Building_Image);
            GameObject buildingprefab = GameManager.BuildingPrefabData[this_building.Building_Image];

            GameManager.CurrentBuilding  = buildingprefab;
            Building b = buildingprefab.GetComponent<Building>();
            Building c = GameManager.CurrentBuilding.GetComponent<Building>();
            c = b.GetComponent<Building>().DeepCopy();
            c.SetValue(b);
            GameManager.CurrentBuilding_Script = this_building;
            //GameManager.CurrentBuilding.name = this_building.Building_Image;
            GameManager.isEdit = true;
            //gridBuildingSystem.Inven_Move(GameManager.CurrentBuilding.transform);


        }
    }


   /* public void Place()         //�ǹ� ��ư Ŭ������ ��(��ġ)
    {
        X_Button.SetActive(false);
    }
    public void X_Place()       //�ǹ� ȸ��
    {
        X_Button.SetActive(true);
    }*/
}
