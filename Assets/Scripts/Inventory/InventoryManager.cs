using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class InventoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject inventory_prefab;     //�κ��丮 Ȱ��ȭ�� ��ư ������
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
                Inventory_Building_Open();
            }).AddTo(this);
        
       
            InvenNuniBtn.OnClickAsObservable().Subscribe(_ =>
            {
                Inventory_Nuni_Open();
            }).AddTo(this);
        
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

                Debug.Log(item.Value.Id);
                GameObject inven = Instantiate(inventory_prefab, Content) as GameObject;         //�κ� ��ư ������ ����


                InventoryButton inventoryBtn = inven.GetComponent<InventoryButton>();

                inventoryBtn.SetButtonImage(GameManager.GetDogamChaImage(item.Value.Building_Image));   //��ư �̹��� ����
                inventoryBtn.temp_building=item.Value;

                Button Button = inven.GetComponent<Button>();
                


                Button.OnClickAsObservable().Subscribe(_ =>                     //�κ��丮 �ǹ� Ŭ�� ����
                {
                    if (inventoryBtn.temp_building.isLock=="T")         //�ش� �ǹ��� ��ġ�Ǿ�����
                    {
                        inventoryBtn.temp_building.Type = BuildType.Load;
                        GridBuildingSystem.OnEditMode.OnNext(inventoryBtn.temp_building);  //�Ǽ���� ON (Ÿ�� �ʱ�ȭ)
                        LoadManager.Instance.RemoveBuilding(inventoryBtn.temp_building.Id); //�ش� ������ ����

                        inventoryBtn.temp_building.isLock = "F";                            //��ġ�ȵ� ���·� �ٲٱ�
                    }
                    else                               //�ش� �ǹ��� ��ġ�ȵǾ�������
                    {
                      Building ActiveBuilding=  LoadManager.Instance.InstatiateBuilding(inventoryBtn.temp_building);
                        ActiveBuilding.Type = BuildType.Load;

                        GridBuildingSystem.OnEditMode.OnNext(ActiveBuilding);  //�Ǽ���� ON
                        item.Value.isLock = "T";                //��ġ�� ���·� �ٲٱ�
                    }
                    

                   
                }).AddTo(this);             


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
        Inventory_Exit();

        for (int i = 0; i < GameManager.CharacterList.Count; i++)
        {

            GameObject inven = Instantiate(inventory_nuni_prefab, Content) as GameObject;         //�κ� ��ư ������ ����

            //inven.name = GameManager.CharacterList[i].cardImage;
            inven.tag = "Inven_Nuni";            //�κ� ��ư �±� ����

            Image ButtonImage = inven.GetComponent<Image>();


          
            ButtonImage.sprite = GameManager.GetCharacterImage(GameManager.CharacterList[i].cardImage);

            inven.GetComponent<InventoryButton>().this_nuni = GameManager.CharacterList[i];
       


        }
    }
}
