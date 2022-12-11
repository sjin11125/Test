using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class NBuilding : Building
{
    public void RefreshBuildingList()               //���� ����Ʈ ���ΰ�ħ
    {
        for (int i = 0; i < GameManager.BuildingList.Count; i++)
        {
            if (GameManager.BuildingList[i].Id.Equals(Id))
            {
                GameManager.BuildingList[i] = this.DeepCopy();
            }
        }
        GridBuildingSystem.isSave = true;

    }
    public void Rotation()          //�ǹ� ȸ��
    {
        bool isflip_bool;

        if (isFliped.Equals("F"))
            isflip_bool = false;
        else
            isflip_bool = true;


        for (int i = 0; i < buildings.Length; i++)
        {
            if (buildings[i] != null)
            {
                SpriteRenderer[] spriterenderer = buildings[i].GetComponentsInChildren<SpriteRenderer>();
                Transform[] transform = buildings[i].GetComponentsInChildren<Transform>();


                for (int j = 0; j < spriterenderer.Length; j++)
                {
                    spriterenderer[j].flipX = isflip_bool;
                }
                for (int k = 0; k < transform.Length; k++)
                {
                    transform[k].localPosition = new Vector3(-transform[k].localPosition.x, transform[k].localPosition.y, 0);
                }

                if (isFliped.Equals("T"))
                    isFliped = "F";
                else
                    isFliped = "T";
            }
        }
        RefreshBuildingList();          //�ǹ� ����Ʈ ���ΰ�ħ
    }

    void Awake()
    {
        Parent = GameObject.Find("buildings");
    }
    void Start()
    {
        bool isflip_bool;

        if (isFliped.Equals("F"))
            isflip_bool = false;
        else
            isflip_bool = true;

        buildings = new GameObject[2];
        currentTime = (int)startingTime;
        save = GetComponent<BuildingSave>();
        //TimeText = GameObject.Find("Canvas/TimeText"); //���ӿ�����Ʈ = ĵ������ �ִ� TimeText�� ����
        if (Type.Equals(BuildType.Make))
        {
            Building_Image = gameObject.name;       //�̸� ����
        }

        //Placed = false;

        // child = GetComponentsInChildren<Transform>();

        // Debug.Log(child[6].name);
        //Coin_Button= child[6];
        //Button_Pannel = child[2];

        Coin_Button.gameObject.SetActive(false);

        //Text countdownText = GetComponent<Text>();

        //layer_y = 10;
        //child[1].GetComponent<SpriteRenderer>().sortingOrder = layer_y;


        //-------------���� �� �ǹ�--------------------
        GameObject Level1building, Level2building, Level3building;
        if (Level <= 3)
        {
            //GameObject UPPannel = Instantiate(UpgradePannel);
            Level1building = gameObject.transform.Find("building").gameObject;
            if (gameObject.transform.Find("building2").gameObject != null)
            {

            }
            Level2building = gameObject.transform.Find("building2").gameObject;
            //Level3building = gameObject.transform.Find("building3").gameObject;
            buildings[0] = Level1building;
            buildings[1] = Level2building;
            // buildings[2] = Level3building;
        }

        switch (Level)
        {
            case 1:
                buildings[0].SetActive(true);

                child = GetComponentsInChildren<Transform>();

                buildings[0].GetComponent<SortingGroup>().sortingOrder = -(int)transform.position.y;

                break;
            case 2:
                buildings[0].SetActive(true);
                //buildings[1].SetActive(true);
                //buildings[2].SetActive(false);
                buildings[0].GetComponent<SpriteRenderer>().sprite = buildings_image[Level - 2];
                buildings[0].GetComponent<SortingGroup>().sortingOrder = -(int)transform.position.y;

                buildings[1].GetComponentInChildren<SortingGroup>().sortingOrder = (-buildings[0].GetComponent<SortingGroup>().sortingOrder) + 1;
                Debug.Log(" buildings[0]:  " + buildings[0].transform.parent.gameObject.name);
                Debug.Log(" buildings[0] layer:  " + buildings[0].GetComponent<SortingGroup>().sortingOrder);

                break;
            case 3:
                buildings[0].SetActive(false);
                buildings[1].SetActive(true);
                // buildings[2].SetActive(true);
                //buildings[2].GetComponent<SpriteRenderer>().sortingOrder = layer_y;
                break;
            default:
                break;
        }
        if (isflip_bool.Equals(true))
        {
            Rotation();
        }
    }

    void Update()
    {
        // layer_y = 1;             //���̾� ����



        // text.text = currentTime.ToString("0.0");
        //TimeText.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0.30f, 1.4f, 0)); //Timer��ġ

        //���� �߰��ؾ��� ���� �ǹ��� �������� �ð��� �ߵ��� �ϱ� (�̰Ŵ� ���߿�)
        //�ǹ��� �����Ǹ� �ð��� �����Ǿ�� �� (�̰͵� ���߿�)


        // �ð��� �帣�� ���� ��� ����ǵ��� �ϱ�


        // �������� ������ ��ȭ + 
        // current Time�� �����ð� ������ �������� �� ��Ȯ ������ ����


        if (Placed.Equals(true))       // �ǹ� ��ġ�� Ȯ��
        {
            Button_Pannel.gameObject.SetActive(false);     // ��ġ�ϴ� ��ư �������
            Rotation_Pannel.gameObject.SetActive(false);        //ȸ�� ��ư �������
            UpgradePannel.gameObject.SetActive(false);
            Remove_Pannel.gameObject.SetActive(false);
            /*if (Building_Image != "bunsu_level(Clone)")
            {
               ( if (isCoin .Equals( false)      //���� ���� �ȸԾ�����
                {
                    Coin();     //��ȭ �����ǰ�
                }*
            }*/
        }
        else                            //Ȯ�� �ƴ� ��
        {

            Button_Pannel.gameObject.SetActive(true);               //Ȯ�� �г� �߰�
            Rotation_Pannel.gameObject.SetActive(true);               //ȸ�� �г� �߰�
            if (Building_Image != "bunsu_level(Clone)")
            {

                if (Type != BuildType.Make)
                {
                    UpgradePannel.gameObject.SetActive(true);

                }
            }
            if (Building_Image != "village_level(Clone)")
            {
                Remove_Pannel.gameObject.SetActive(true);
            }
        }




    }
    public void Coin() //��ȭ�κ�
    {

        //float currentTime_1 = currentTime;
        //currentTime_1 -= 1 * Time.deltaTime;
        currentTime -= 1 * Time.deltaTime;
        //currentTime = (int)currentTime_1;

        if ((int)currentTime <= 0)
        {
            currentTime = 0;
        }

        if (((int)currentTime % 5).Equals(0) && (int)currentTime != startingTime && isCountCoin.Equals(false))     //�����ǰ� 5�� ���� ��ȭ���� (�ǹ����� �ٸ���!)
        {
            isCountCoin = true;
            //CountCoin += 1;

            Coin_Button.gameObject.SetActive(true);
        }
        else if ((int)currentTime % 5 != 0)
        {
            isCountCoin = false;
        }

        // ��ȭ�� ������ current Time �ʱ�ȭ or 0�� �Ǹ� �̹��� MAX coin���� ��ȯ �� ��Ȯ�ϸ� currentTime = startingTime




    }

    public void Coin_OK()       //��ȭ ��ư ������ �Լ�
    {
        //currentTime =  startingTime;
        isCoin = true;      //���� �Ծ���
        GameManager.Money += Reward[Level - 1];
        CanvasManger.AchieveMoney += Reward[Level - 1];

        currentTime = (int)startingTime;

        isCoin = true;

        if (currentTime.Equals(0))
        {
            //�����ʿ�
            currentTime = (int)startingTime;
            //Max �̹����� �ٲ�
        }
        Coin_Button.gameObject.SetActive(false);
    }







    #region Build Methods
    public bool CanBePlaced()           //�ǹ��� ������ �� �ִ��� üũ
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);     //������ġ
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;


        if (GridBuildingSystem.current.CanTakeArea(areaTemp))
        {
            return true;
        }
        return false;
    }
    public void Sell_Pannel()
    {

        RemovePannel.gameObject.SetActive(true);
        RemovePannel.transform.parent = GameObject.Find("O").transform;
        RemovePannel.GetComponent<RectTransform>().localPosition = new Vector3(1, 1, 0);
        RemovePannel.GetComponent<ChaButtonScript>().DowngradeBuilding = this;

    }
    public void Remove(Building building)
    {

        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        GridBuildingSystem.current.RemoveArea(areaTemp);





        if (Type.Equals(BuildType.Make))     //�������� ��� ��ġX �ٷ� ����
        {
            GameManager.Money += building.Cost[building.Level - 1];          //�ڿ� �ǵ�����
            CanvasManger.AchieveMoney += building.Cost[building.Level - 1];
            GameManager.ShinMoney += building.ShinCost[building.Level - 1];
            CanvasManger.AchieveShinMoney += building.ShinCost[building.Level - 1];
            Destroy(gameObject);
        }
        else                                //��ġ�ϰ� ����
        {
            GameManager.Money += building.Cost[building.Level - 1] / 10;          //�ڿ� �ǵ�����
            CanvasManger.AchieveMoney += building.Cost[building.Level - 1] / 10;
            GameManager.ShinMoney += building.ShinCost[building.Level - 1] / 3;
            CanvasManger.AchieveShinMoney += building.ShinCost[building.Level - 1] / 3;

            BuildingListRemove();
            save.BuildingReq(BuildingDef.removeValue, this);
            Destroy(gameObject);
        }
        GameManager.isUpdate = true;

    }
    public void Place_Initial(BuildType buildtype)
    {
        Vector3 vec = new Vector3(float.Parse(BuildingPosiiton_x), float.Parse(BuildingPosiiton_y), 0);
        area.position = GridBuildingSystem.current.gridLayout.WorldToCell(vec);
        BoundsInt areaTemp = area;
        //areaTemp.position = positionInt;
        Placed = true;      // ��ġ �ߴ�? ��
        GridBuildingSystem.current.TakeArea(areaTemp);      //Ÿ�� �� ����
        transform.position = vec;
        //Debug.Log("index: "+ (-((int)transform.position.y - (int)transform.position.x)));
        /* buildings[0].GetComponent<SortingGroup>().sortingOrder = -((int)transform.position.y- (int)transform.position.x);
         if (Level .Equals( 2)
         {
             buildings[1].GetComponentInChildren<SortingGroup>().sortingOrder = -((int)transform.position.y-(int)transform.position.x);
         }*/
    }
    public void Place(BuildType buildtype)         //�ǹ� ��ġ
    {

        Vector3 vec = transform.position;
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(vec);
        BoundsInt areaTemp = area;
        //areaTemp.position = positionInt;
        //Debug.Log(areaTemp.position);
        Placed = true;      // ��ġ �ߴ�? ��
        Debug.Log(buildings.Length);
        buildings[0].GetComponent<SortingGroup>().sortingOrder = -(int)transform.position.y;
        /*if (Level.Equals(2)
        {
            buildings[1].GetComponentInChildren<SortingGroup>().sortingOrder = -(int)transform.position.y;
        }*/
        GridBuildingSystem.current.TakeArea(areaTemp);      //Ÿ�� �� ����

        //currentTime = startingTime;
        //���� ������Ʈ �κ�
        BuildingPosition = transform.position;          //��ġ ����
        layer_y = (int)-transform.position.y;      //���̾� ����
        isLock = "T";           //��ġ�ߴ�

        /* for (int i = 0; i < buildings.Length; i++)
         {
             if (buildings[i] != null)
             {
                 buildings[i].GetComponent<SpriteRenderer>().sortingOrder = layer_y;
             }
         }*/

        if (buildtype.Equals(BuildType.Make))                     //���� ����� �ǰ�?
        {

            Building_name = gameObject.name;
            Debug.Log("BuildingPosiiton_x: " + BuildingPosiiton_x);
            GameManager.BuildingNumber[Building_Image]++; //�ش� �ǹ��� ���� �߰�
            Id = GameManager.IDGenerator();
            gameObject.name = Id;      //�̸� �缳��
            BuildingListAdd();      //���� ������ �ִ� �ǹ� ����Ʈ�� �߰�
            buildtype = BuildType.Empty;
            Debug.Log("���θ���");

        }
        else if (buildtype.Equals(BuildType.Load))                  //�ε��Ҷ�
        {
            buildtype = BuildType.Empty;
        }
        else if (buildtype.Equals(BuildType.Move))            //�̵��� ��
        {
            Debug.Log("Move");
            gameObject.name = GameManager.CurrentBuilding_Script.Id;
            Id = GameManager.CurrentBuilding_Script.Id;
            Building_name = GameManager.CurrentBuilding_Script.Building_name;
            isLock = "T";
            RefreshBuildingList();

            buildtype = BuildType.Empty;

            // save.UpdateValue(this);
            save.BuildingReq(BuildingDef.updateValue, this);
        }
        else
        {
            // save.UpdateValue(this);
            save.BuildingReq(BuildingDef.updateValue, this);
        }

        gameObject.transform.parent = Parent.transform;
        GridBuildingSystem.current.temp_gameObject = null;
    }
    public void BuildingListRemove()
    {
        for (int i = GameManager.BuildingList.Count - 1; i >= 0; i--)
        {
            if (GameManager.BuildingList[i].Building_name.Equals(Building_name))
            {
                Debug.Log("Remove: " + GameManager.BuildingList[i].Building_name);
                GameManager.BuildingList.RemoveAt(i);
                for (int p = 0; p < GameManager.BuildingList.Count; p++)
                {
                    Debug.Log("Current: " + GameManager.BuildingList[p].Building_name);
                }
                return;
            }

        }

        GridBuildingSystem.isSave = true;

    }
    public void BuildingListAdd()
    {
        GameManager.BuildingList.Add(this.DeepCopy());      //���� ������ �ִ� ���� ����Ʈ�� �߰�

        //GameManager.BuildingArray = GameManager.BuildingList.ToArray();
        Debug.Log("GameManager.BuildingArray: " + GameManager.BuildingArray.Length);

        GameManager.CurrentBuilding = null;
        //

        save.BuildingReq(BuildingDef.addValue, this);
        //GameManager.isUpdate = true;
    }
    #endregion
    // Update is called once per frame


    public bool Upgrade()
    { //GameObject Level1building, Level2building, Level3building;
        Debug.Log("�� ���� �̹���: " + Building_Image);
        if (Level < 2)
        {
            if (Building_Image == "building_level(Clone)" ||
                   Building_Image == "village_level(Clone)" ||
                   Building_Image == "flower_level(Clone)")
            {
                Debug.Log("�ش� �ǹ�����");
                for (int i = 0; i < GameManager.CharacterList.Count; i++)
                {
                    if (GameManager.CharacterList[i].cardName == "����������")
                    {
                        Debug.Log("�ش� �����̽�");
                        isUp = true;

                    }
                }
            }
            //GameObject UPPannel = Instantiate(UpgradePannel);
            if (Building_Image == "syrup_level(Clone)" ||
             Building_Image == "fashion_level(Clone)" ||
             Building_Image == "school_level(Clone)")
            {
                Debug.Log("�ش� �ǹ�����22");
                for (int i = 0; i < GameManager.CharacterList.Count; i++)
                {
                    if (GameManager.CharacterList[i].cardName == "����Ʈ����")
                    {
                        Debug.Log("�ش� �����̽�222");
                        isUp = true;

                    }
                }
            }
            if (isUp == true)
            {


                UpgradePannel2.GetComponent<ChaButtonScript>().Upgrade(buildings, Level, this);
                UpgradePannel2.gameObject.SetActive(true);


                Text[] upgradeText = UpgradePannel2.GetComponentsInChildren<Text>();
                Debug.Log("���׷��̵� ���̵�: " + Id);




                for (int i = 0; i < GameManager.BuildingList.Count; i++)
                {
                    for (int j = 0; j < GameManager.BuildingArray.Length; j++)
                    {
                        if (Building_Image == GameManager.BuildingArray[j].Building_Image)
                        {

                            upgradeText[3].text = GameManager.BuildingArray[j].Reward[Level - 1].ToString();     //���� �� ȹ�� ��ȭ
                            Debug.Log("������: " + GameManager.BuildingArray[j].Reward[Level - 1]);
                            upgradeText[4].text = GameManager.BuildingArray[j].Reward[Level].ToString();                       //���� �� ȹ�� ��ȭ
                            Debug.Log("������: " + GameManager.BuildingArray[j].Reward[Level - 1]);
                            upgradeText[6].text = "����: " + GameManager.BuildingArray[j].Cost[Level].ToString() + ",   ������ ����: " + GameManager.BuildingArray[j].ShinCost[Level].ToString() + " �� �Ҹ�˴ϴ�.";
                            return true;

                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }

        }
        return isUp;
    }




}
