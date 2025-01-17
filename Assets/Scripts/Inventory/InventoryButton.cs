using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class InventoryButton : MonoBehaviour
{
    // Start is called before the first frame update
    public Image X_Image;     //건물 회수 버튼

  public  Building this_building;         //이 버튼에 해당하는 건물
    public Card this_nuni;         //이 버튼에 해당하는 건물
    GridBuildingSystem gridBuildingSystem;

    public GameObject buildings;
    public GameObject nunis;

    private GameObject settigPanel;

    void Start()
    {
        if (gameObject.tag.Equals("Inven_Building"))
        {
            buildings = GameObject.Find("buildings");

            for (int i = 0; i < GameManager.BuildingList.Count; i++)
            {
                if (this.gameObject.name .Equals( GameManager.BuildingList[i].Id))
                {
                    this_building = GameManager.BuildingList[i];
                    gridBuildingSystem = buildings.GetComponentInChildren<GridBuildingSystem>();
                }
            }
            if (this_building.isLock .Equals( "F"))
            {
                X_Image.gameObject.SetActive(true);
            }
            else
            {
                X_Image.gameObject.SetActive(false);
            }

        }
        else if(gameObject.tag .Equals( "Inven_Nuni"))
        {
            nunis= GameObject.Find("nunis"); 
            gridBuildingSystem = gameObject.transform.parent.parent.GetComponent<GridBuildingSystem>();

            if (this_nuni.isLock .Equals( "F"))
            {
                X_Image.gameObject.SetActive(true);
            }
            else
            {
                X_Image.gameObject.SetActive(false);
            }
        }

        settigPanel = GameObject.FindGameObjectWithTag("SettingPanel");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void nuni_Click()
    {
        if (this_nuni.isLock.Equals("T") )     //누니가 배치된 상태
        {
            this_nuni.isLock = "F";         //배치 안된 상태로 바꾸기
            Transform[] nuni_child = nunis.GetComponentsInChildren<Transform>();
            X_Image.gameObject.SetActive(true);
            for (int i = 0; i < nuni_child.Length; i++)                     //누니 목록에서 해당 누니 찾아서 없애기
            {
                if (nuni_child[i].gameObject.name .Equals( this_nuni.cardImage+"(Clone)"))
                {
                    
                    Card nuni_childs = nuni_child[i].gameObject.GetComponent<Card>();
                    if (nuni_childs.isLock.Equals("T"))
                    {
                        nuni_childs.isLock = "F";
                        Destroy(nuni_child[i].gameObject);
                        Debug.Log(nuni_child[i].gameObject.name);
                        StartCoroutine(NuniSave(this_nuni));          //구글 스크립트에 업데이트
                    }
                   
                    return;
                }
            }
          

        }
        else                                    //누니가 배치 안된 상태
        {
            this_nuni.isLock = "T";         //배치 된 상태로 바꾸기

            X_Image.gameObject.SetActive(false);
            for (int i = 0; i < GameManager.CharacterList.Count; i++)           //Instatntiate 해주기
            {
                if (this_nuni.cardName.Equals( GameManager.CharacterList[i].cardName))
                {
                    GameManager.CharacterList[i].isLock = "T";
                    GameObject nuni = Instantiate(GameManager.CharacterPrefab[this_nuni.cardImage], nunis.transform) as GameObject;
                   
                    for (int j = 0; j < GameManager.AllNuniArray.Length; j++)
                    {
                        if (GameManager.AllNuniArray[j].cardImage != this_nuni.cardImage)
                            continue;

                      Card Value=  nuni.GetComponent<Card>();
                        Value.SetValue(GameManager.AllNuniArray[j]);
                    }
                    
                    nuni.GetComponent<Card>().isLock = "T";
                    StartCoroutine(NuniSave(this_nuni));          //구글 스크립트에 업데이트
                    return;
                }

                
            }

        }
        settigPanel.GetComponent<AudioController>().Sound[0].Play();
    }
    IEnumerator NuniSave(Card nuni)                //누니 구글 스크립트에 저장
    {

        WWWForm form1 = new WWWForm();
        form1.AddField("order", "nuniUpdate");
        form1.AddField("player_nickname", GameManager.NickName);
        form1.AddField("nuni", nuni.cardName +":"+this_nuni.isLock);



        yield return StartCoroutine(Post(form1));                        //구글 스크립트로 초기화했는지 물어볼때까지 대기


    }
    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(GameManager.URL, form)) // 반드시 using을 써야한다
        {
            yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);
           // if (www.isDone) NuniResponse(www.downloadHandler.text);
            //else print("웹의 응답이 없습니다.");*/
        }

    }


    public void Click()         //건축물 버튼 클릭했을 때
    {
       

        for (int i = 0; i < GameManager.BuildingList.Count; i++)
        {
            if (this.gameObject.name .Equals( GameManager.BuildingList[i].Id))
            {
                this_building = GameManager.BuildingList[i];
                
            }
        }  

      /*  for (int i = 0; i < GameManager.StrList.Count; i++)
        {
            if (this.gameObject.name .Equals( GameManager.StrList[i].Id)
            {
                this_building = GameManager.StrList[i];
                
            }
        }*/

        if (gridBuildingSystem.temp_gameObject!=null)
        {
            Building c = gridBuildingSystem.temp_gameObject.GetComponent<Building>();


            gridBuildingSystem.prevArea2 = c.area;
            gridBuildingSystem.ClearArea2();
            //gridBuildingSystem.CanTakeArea(c.area);
            Destroy(gridBuildingSystem.temp_gameObject);

            
        }

        if (GameManager.CurrentBuilding_Button ==null )      //그 전에 클릭했던 버튼이 없을 때
        {
            GameManager.CurrentBuilding_Button = this;
        }
        else
        {
            if (GameManager.CurrentBuilding_Button.this_building.Id!=this.this_building.Id&& GameManager.CurrentBuilding_Button.this_building.isLock .Equals( "T"))
            {
                
            
                GameManager.CurrentBuilding_Button.this_building.isLock = "F";
                GameManager.CurrentBuilding_Button.X_Image.gameObject.SetActive(true);
                GameManager.CurrentBuilding_Button = this;
            }

        }
        Transform[] building_child = buildings.GetComponentsInChildren<Transform>();
     
        if (this_building.isLock.Equals("T"))      //현재 배치된 상태인가
        {
         
            if (GameManager.CurrentBuilding != null)
            {
                Building c = GameManager.CurrentBuilding.GetComponent<Building>();


                gridBuildingSystem.prevArea2 = c.area;
                gridBuildingSystem.ClearArea2();
                //gridBuildingSystem.CanTakeArea(c.area);
            }
            for (int i = 0; i < building_child.Length; i++)
            {
                if (building_child[i].name .Equals( this_building.Id))
                {
                    //buildingprefab = building_child[i].gameObject;
                    GameManager.CurrentBuilding = building_child[i].gameObject;

        
                    Building b = GameManager.CurrentBuilding.GetComponent<Building>();
                    gridBuildingSystem.prevArea2 = b.area;
                    gridBuildingSystem.RemoveArea(b.area);
                    gridBuildingSystem.CanTakeArea(b.area);
                    //b.Remove(GameManager.CurrentBuilding.GetComponent<Building>());
                    Building c = building_child[i].gameObject.GetComponent<Building>();
                    gridBuildingSystem.RemoveArea(c.area);
                    Destroy(building_child[i].gameObject);

                }
            }
            GameManager.isEdit = false;
           
            //GameManager.CurrentBuilding = null;
            GameManager.CurrentBuilding_Script = GameManager.CurrentBuilding.GetComponent<Building>();
            this_building.isLock = "F";         //배치 안된 상태로 바꾸기
            X_Image.gameObject.SetActive(true);
           





            for (int i = 0; i < building_child.Length; i++)
            {
                if (building_child[i].gameObject.name .Equals(GameManager.CurrentBuilding.name))
                {
                    Building building_childs = building_child[i].gameObject.GetComponent<Building>();
                    Destroy(building_childs);
                }
            }

            for (int i = 1; i < building_child.Length; i++)
            {
                if (building_child[i].gameObject.name .Equals( gameObject.name))
                {
                    Building building_childs = building_child[i].gameObject.GetComponent<Building>();
                    building_childs.isLock = "F";
                    building_childs.BuildingPosiiton_x = "0";
                    building_childs.BuildingPosiiton_y = "0";

                    building_childs.save.UpdateValue(building_childs);
                    Destroy(building_child[i].gameObject);
                }
                
            }
            GameManager.CurrentBuilding = null;
            GameManager.CurrentBuilding_Button = null;
            gridBuildingSystem.GridLayerNoSetting();                //메인 타일 안보이게
        }
        else if(this_building.isLock .Equals( "F"))                     //현재 배치된 상태가 아닌가
        {
            X_Image.gameObject.SetActive(false);
            this_building.isLock = "T";         //배치 된 상태로 바꾸기
            GameManager.InvenButton =this.GetComponent<Button>();
            if (GameManager.CurrentBuilding!=null)
            {
                Building c = GameManager.CurrentBuilding.GetComponent<Building>();

             
                gridBuildingSystem.prevArea2 = c.area;
                gridBuildingSystem.ClearArea2();
                gridBuildingSystem.CanTakeArea(c.area);
            }

            for (int i = 0; i < GameManager.BuildingArray.Length; i++)
            {
                if (GameManager.BuildingArray[i].Building_Image.Equals( this_building.Building_Image))
                {
                    GameManager.CurrentBuilding =GameManager.BuildingPrefabData[this_building.Building_Image];
                    Building c = GameManager.CurrentBuilding.GetComponent<Building>();
                    
                    c.SetValue(this_building);

                    Building b = GameManager.CurrentBuilding.GetComponent<Building>();
                    gridBuildingSystem.prevArea2 = b.area;
                    gridBuildingSystem.ClearArea2();
                    gridBuildingSystem.CanTakeArea(b.area);
                    break;
                }
            }
            for (int i = 0; i < GameManager.StrArray.Length; i++)
            {
              if (GameManager.StrArray[i].Building_Image .Equals( this_building.Building_Image))
                {
                    GameManager.CurrentBuilding = GameManager.BuildingPrefabData[this_building.Building_Image];
                    Building c = GameManager.CurrentBuilding.GetComponent<Building>();

                    c.SetValue(this_building);

                    Building b = GameManager.CurrentBuilding.GetComponent<Building>();
                  
                    gridBuildingSystem.prevArea2 = b.area;
                    gridBuildingSystem.ClearArea2();
                    gridBuildingSystem.CanTakeArea(b.area);
                    break;
                }
            }
            GameManager.CurrentBuilding_Script = this_building;

            //GameManager.CurrentBuilding.name = this_building.Building_Image;
            gridBuildingSystem.GridLayerSetting();          //메인 타일 보이게
            GameManager.isInvenEdit = true;
            //gridBuildingSystem.Inven_Move(GameManager.CurrentBuilding.transform);
            GameManager.CurrentBuilding_Button = this;

        }
        settigPanel.GetComponent<AudioController>().Sound[0].Play();
    }
    
}
