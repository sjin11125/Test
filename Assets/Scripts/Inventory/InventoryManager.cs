using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject inventory_prefab;     //�κ��丮 ��ư ������
    public GameObject inventory_nuni_prefab;     //�κ��丮 ��ư ������
    public Transform Content;

    public GridBuildingSystem gridBuildingSystem;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Inventory_Building_Open()            //�ǹ� �κ� ��ư ������ ��
    {
        Transform[] Content_Child= Content.GetComponentsInChildren<Transform>();
        for (int i = 1; i < Content_Child.Length; i++)
        {
            Destroy(Content_Child[i].gameObject);
        }

        for (int i = 0; i < GameManager.BuildingList.Count; i++)
        {
            
            GameObject inven=Instantiate(inventory_prefab,Content) as GameObject;         //�κ� ��ư ������ ����

            inven.gameObject.name = GameManager.BuildingList[i].Building_name;
            inven.gameObject.tag = "Inven_Building";            //�κ� ��ư �±� ����

            

            Image ButtonImage = inven.GetComponent<Image>();
            Debug.Log("building image: "+GameManager.BuildingList[i].Building_Image);
         

            Image PrefabImage;// = GameManager.GetDogamChaImage(GameManager.BuildingList[i].Building_Image);
            ButtonImage.sprite = GameManager.GetDogamChaImage(GameManager.BuildingList[i].Building_Image);

            if (GameManager.BuildingList[i].isLock == "T")
            {
                Button Button = inven.GetComponent<Button>();
                //Button.enabled= false;              //�̹� ��ġ�Ǿ� ������ ��ư Ŭ�� ���ϰ� X ��

            }

        }
    }
    public void Inventory_Nuni_Open()            //���� �κ� ��ư ������ ��
    {
        Transform[] Content_Child = Content.GetComponentsInChildren<Transform>();       //��ư �� ����
        for (int i = 1; i < Content_Child.Length; i++)
        {
            Destroy(Content_Child[i].gameObject);
        }

        for (int i = 0; i < GameManager.CharacterList.Count; i++)
        {

            GameObject inven = Instantiate(inventory_nuni_prefab, Content) as GameObject;         //�κ� ��ư ������ ����

            inven.gameObject.name = GameManager.CharacterList[i].cardImage;
            inven.gameObject.tag = "Inven_Nuni";            //�κ� ��ư �±� ����

            Image ButtonImage = inven.GetComponent<Image>();
            Debug.Log("building image: " + GameManager.CharacterList[i].cardImage);


            Image PrefabImage;// = GameManager.GetDogamChaImage(GameManager.BuildingList[i].Building_Image);
            ButtonImage.sprite = GameManager.GetCharacterImage(GameManager.CharacterList[i].cardImage);

            if (GameManager.CharacterList[i].isLock == "T")
            {
                Button Button = inven.GetComponent<Button>();
                //Button.enabled= false;              //�̹� ��ġ�Ǿ� ������ ��ư Ŭ�� ���ϰ� X ��

            }

        }
    }
}
