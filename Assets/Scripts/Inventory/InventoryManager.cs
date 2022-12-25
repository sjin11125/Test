using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class InventoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject inventory_prefab;     //�κ��丮 ��ư ������
    public GameObject inventory_nuni_prefab;     //�κ��丮 ��ư ������
    public Transform Content;

    public Button InvenBuildingBtn;
    public Button InvenNuniBtn;

    public GridBuildingSystem gridBuildingSystem;
    void Start()
    {
        if (LoadManager.Instance == null)
            return;
       
        InvenBuildingBtn.OnClickAsObservable().Subscribe(_ =>
        {
            Inventory_Exit();
        }).AddTo(this);
        InvenNuniBtn.OnClickAsObservable().Subscribe(_ =>
        {
            Inventory_Exit();
        }).AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isUpdate.Equals(true))
        {
            GameManager.isUpdate = false;
            Inventory_Building_Open();
        }
    }
    public void Inventory_Exit()
    {
        Transform[] Content_Child = Content.GetComponentsInChildren<Transform>();
        for (int i = 1; i < Content_Child.Length; i++)
        {
            Destroy(Content_Child[i].gameObject);
        }
    }
    public void Inventory_Building_Open()            //�ǹ� �κ� ��ư ������ ��
    {
        Inventory_Exit();           //���� �ִ� ��� �� �����
        foreach (var item in LoadManager.Instance.MyBuildings)
        {

        
            bool isStr = false;
            for (int j = 0; j < GameManager.StrArray.Length; j++)
            {
                if (item.Value.Building_Image .Equals( GameManager.StrArray[j].Building_Image) )      //��ġ���ΰ�
                {
                    isStr = true;
                }
            }
            if (item.Value.Id != "ii1y1"&&isStr.Equals(false) )         //�м��� �ƴϰ� ��ġ���� �ƴ϶��
            {
                
              
                GameObject inven = Instantiate(inventory_prefab, Content) as GameObject;         //�κ� ��ư ������ ����

                inven.gameObject.name = item.Value.Id;
                inven.gameObject.tag = "Inven_Building";            //�κ� ��ư �±� ����



                Image ButtonImage = inven.GetComponent<Image>();


                Image PrefabImage;// = GameManager.GetDogamChaImage(GameManager.BuildingList[i].Building_Image);
                ButtonImage.sprite = GameManager.GetDogamChaImage(item.Value.Building_Image);

                if (item.Value.isLock .Equals( "T"))
                {
                    Button Button = inven.GetComponent<Button>();
                    //Button.enabled= false;              //�̹� ��ġ�Ǿ� ������ ��ư Ŭ�� ���ϰ� X ��

                }
            }
        }
    }
    public void Inventory_Structure_Open()            //��ġ�� �κ� ��ư ������ ��
    {
        Inventory_Exit();           //���� �ִ� ��� �� �����
        foreach (var item in LoadManager.Instance.MyBuildings)
        {

            bool isStr = false;
            for (int j = 0; j < GameManager.StrArray.Length; j++)
            {
                if (item.Value.Building_Image .Equals( GameManager.StrArray[j].Building_Image))       //��ġ���ΰ�
                {
                    isStr = true;
                }
            }
            if (item.Value.Id != "ii1y1"&&isStr.Equals(true))          //�м��� �ƴϰ� ��ġ���̶��
            {

                GameObject inven = Instantiate(inventory_prefab, Content) as GameObject;         //�κ� ��ư ������ ����
     
                inven.gameObject.name = item.Value.Id;
                inven.gameObject.tag = "Inven_Building";            //�κ� ��ư �±� ����



                Image ButtonImage = inven.GetComponent<Image>();


                ButtonImage.sprite = GameManager.GetDogamChaImage(item.Value.Building_Image);

                if (item.Value.isLock .Equals( "T"))
                {
                    Button Button = inven.GetComponent<Button>();
                    //Button.enabled= false;              //�̹� ��ġ�Ǿ� ������ ��ư Ŭ�� ���ϰ� X ��

                }
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

            inven.name = GameManager.CharacterList[i].cardImage;
            inven.tag = "Inven_Nuni";            //�κ� ��ư �±� ����

            Image ButtonImage = inven.GetComponent<Image>();


          
            ButtonImage.sprite = GameManager.GetCharacterImage(GameManager.CharacterList[i].cardImage);

            inven.GetComponent<InventoryButton>().this_nuni = GameManager.CharacterList[i];
       


        }
    }
}
