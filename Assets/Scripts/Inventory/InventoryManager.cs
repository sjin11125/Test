using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject inventory_prefab;     //�κ��丮 ��ư ������
    public Transform Content;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Inventory_Building_Open()            //�ǹ� �κ� ��ư ������ ��
    {
        Debug.Log(GameManager.BuildingList.Count);
        for (int i = 0; i < GameManager.BuildingList.Count; i++)
        {
            
            GameObject inven=Instantiate(inventory_prefab,Content) as GameObject;         //�κ� ��ư ������ ����

            inven.gameObject.name = GameManager.BuildingList[i].Building_name;

            Image ButtonImage = inven.GetComponent<Image>();
            foreach (var item in GameManager.BuildingPrefabData)
            {
                Debug.Log("prefab name: " +item.Key);
            }


            Image PrefabImage;// = GameManager.GetDogamChaImage(GameManager.BuildingList[i].Building_Image);
            ButtonImage.sprite = GameManager.GetDogamChaImage(GameManager.BuildingList[i].Building_Image);

            if (GameManager.BuildingList[i].isLock == "T")
            {
                Button Button = inven.GetComponent<Button>();
                Button.interactable= false;              //�̹� ��ġ�Ǿ� ������ ��ư Ŭ�� ���ϰ� X ��
            }

        }
        
    }
}
