using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StrParse
{
    //-------------------------�Ľ�����------------------------------
    public string isLock;               //��� ����
    public string Building_name;            //�ǹ� �̸�
    public string Building_Image;          //���� �̹��� �̸�
    public string isFliped = "F";
    public string BuildingPosiiton_x;
    public string BuildingPosiiton_y;
    public string Id;
    //-----------------------------------------------------------

}
public class Str : MonoBehaviour
{
    public bool Placed = false;    //*

    public string isLock;               //��� ����
    public string Building_name;            //�ǹ� �̸�
    public string Building_Image;          //���� �̹��� �̸�
    public string isFliped = "F";
    public string BuildingPosiiton_x;
    public string BuildingPosiiton_y;
    public string Id;
    public string Cost;         //���
    public string Info;

    public Sprite Image;
    // Start is called before the first frame update
    GameObject Parent;

    public BuildType Type;

    public BuildingSave save;

    public int layer_y;   // �ǹ� ���̾�
    Transform[] child;

    public Transform Button_Pannel;    //*
    public Transform Rotation_Pannel;
    public Transform Remove_Pannel;

    public BoundsInt area;

    public Vector2 BuildingPosition;                //�ǹ� ��ġ
    public Str(string isLock, string str_name, string str_image,string info,string cost)
    {
        this.isLock = isLock;
        Building_name = str_name;
        Building_Image = str_image;
        Info = info;
        Cost = cost;
    }

    public Str()
    {
    }
    public void SetChaImage(Sprite image)
    {
        Image = image;
    }
    public void SetValue(Str getBuilding)
    {
        isLock = getBuilding.isLock;
        Building_name = getBuilding.Building_name;
        Building_Image = getBuilding.Building_Image;
        isFliped = getBuilding.isFliped;
        BuildingPosiiton_x = getBuilding.BuildingPosiiton_y;
        BuildingPosiiton_y = getBuilding.BuildingPosiiton_y;
        Id = getBuilding.Id;
    }
    public Str DeepCopy()
    {
        Str StrCopy = new Str();
        StrCopy.isLock= isLock;               //��� ����
        StrCopy.Building_name = Building_name;            //�ǹ� �̸�
        StrCopy.Building_Image= Building_Image;          //���� �̹��� �̸�
        StrCopy.isFliped = isFliped;
        StrCopy.BuildingPosiiton_x=BuildingPosiiton_x;
        StrCopy. BuildingPosiiton_y=BuildingPosiiton_y;
        StrCopy.Id = Id ;
        return StrCopy;
    }
    public void RefreshStrList()               //���� ����Ʈ ���ΰ�ħ
    {
        for (int i = 0; i < GameManager.StrList.Count; i++)
        {
            if (GameManager.StrList[i].Building_name == Building_name)
            {
                GameManager.StrList[i] = this.DeepCopy();
            }
        }
        GridBuildingSystem.isSave = true;
    }
    public void Rotation()          //�ǹ� ȸ��
    {
        bool isflip_bool;

        if (isFliped == "F")
            isflip_bool = false;
        else
            isflip_bool = true;

        SpriteRenderer spriterenderer = GetComponentInChildren<SpriteRenderer>();
        spriterenderer.flipX = isflip_bool;

      
                if (isFliped == "T")
                {
                    isFliped = "F";
                }
                else
                    isFliped = "T";
        RefreshStrList();//��ġ�� ����Ʈ ���ΰ�ħ
    }
    void Awake()
    {
        Parent = GameObject.Find("buildings");
    }
    void Start()
    {
        bool isflip_bool;

        if (isFliped == "F")
            isflip_bool = false;
        else
            isflip_bool = true;

        
        save = GetComponent<BuildingSave>();
        //TimeText = GameObject.Find("Canvas/TimeText"); //���ӿ�����Ʈ = ĵ������ �ִ� TimeText�� ����
        if (Type == BuildType.Make)
        {
            Building_Image = gameObject.name;       //�̸� ����
        }

        //Placed = false;

        child = GetComponentsInChildren<Transform>();


        //Text countdownText = GetComponent<Text>();

        layer_y = 10;
        child[1].GetComponent<SpriteRenderer>().sortingOrder = layer_y;


        
        if (isflip_bool == true)
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


        if (Placed == true)       // �ǹ� ��ġ�� Ȯ��
        {
            Button_Pannel.gameObject.SetActive(false);     // ��ġ�ϴ� ��ư �������
            Rotation_Pannel.gameObject.SetActive(false);        //ȸ�� ��ư �������
            Remove_Pannel.gameObject.SetActive(false);
        }
        else
        {

            Button_Pannel.gameObject.SetActive(true);
            Rotation_Pannel.gameObject.SetActive(true);
            Remove_Pannel.gameObject.SetActive(true);
        }
    }
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
    public void Remove(Str building)
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        //Debug.Log()
        GameManager.Money += int.Parse( Cost);          //�ڿ� �ǵ�����

        GridBuildingSystem.current.RemoveArea(areaTemp);
        if (Type == BuildType.Make)      //�������� ��� ��ġX �ٷ� ����
        {
            Destroy(gameObject);
        }
        else                                //��ġ�ϰ� ����
        {
            StrListRemove();
            save.RemoveValue(Id);
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
    }
    public void Place(BuildType buildtype)         //�ǹ� ��ġ
    {
        Debug.Log("Place()");

        Vector3 vec = transform.position;
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(vec);
        BoundsInt areaTemp = area;
        Debug.Log(areaTemp.position);
        //areaTemp.position = positionInt;
        //Debug.Log(areaTemp.position);
        Placed = true;      // ��ġ �ߴ�? ��

        GridBuildingSystem.current.TakeArea(areaTemp);      //Ÿ�� �� ����

        //currentTime = startingTime;
        //���� ������Ʈ �κ�
        BuildingPosition = transform.position;          //��ġ ����
        layer_y = (int)(-transform.position.y / 0.6);             //���̾� ����
        isLock = "T";           //��ġ�ߴ�

        if (layer_y == 0 || layer_y == 1)
        {
            layer_y = 2;
        }
        Str BuildingCurrent = gameObject.GetComponent<Str>();


        if (buildtype == BuildType.Make)                       //���� ����� �ǰ�?
        {

            Building_name = gameObject.name;
            Debug.Log("BuildingPosiiton_x: " + BuildingPosiiton_x);
            //GameManager.BuildingNumber[Building_Image]++; //�ش� �ǹ��� ���� �߰�
            Id = GameManager.IDGenerator();
            gameObject.name = Id;      //�̸� �缳��
            StrListAdd();      //���� ������ �ִ� �ǹ� ����Ʈ�� �߰�
            buildtype = BuildType.Empty;
            Debug.Log("���θ���");

        }
        else if (buildtype == BuildType.Load)                    //�ε��Ҷ�
        {
            buildtype = BuildType.Empty;
        }
        else if (buildtype == BuildType.Move)               //�̵��� ��
        {
            Debug.Log("Move");
            gameObject.name = GameManager.CurrentBuilding_Script.Id;
            Id = GameManager.CurrentBuilding_Script.Id;
            Building_name = GameManager.CurrentBuilding_Script.Building_name;
            isLock = "T";
            RefreshStrList();

            buildtype = BuildType.Empty;

            save.UpdateValue(this);
        }
        else
        {
            save.UpdateValue(this);
        }

        gameObject.transform.parent = Parent.transform;
        GridBuildingSystem.current.temp_gameObject = null;
    }
    public void StrListRemove()
    {
        for (int i = GameManager.StrList.Count - 1; i >= 0; i--)
        {
            if (GameManager.StrList[i].Building_name == Building_name)
            {
                Debug.Log("Remove: " + GameManager.StrList[i].Building_name);
                GameManager.StrList.RemoveAt(i);
                for (int p = 0; p < GameManager.StrList.Count; p++)
                {
                    Debug.Log("Current: " + GameManager.StrList[p].Building_name);
                }
                return;
            }

        }

        GridBuildingSystem.isSave = true;

    }
    public void StrListAdd()
    {
        GameManager.StrList.Add(this.DeepCopy());      //���� ������ �ִ� ���� ����Ʈ�� �߰�

        GameManager.StrArray = GameManager.StrList.ToArray();
        Debug.Log("GameManager.StrArray: " + GameManager.StrArray.Length);

        GameManager.CurrentStr = null;
        //

        save.AddValue(this);
        //GameManager.isUpdate = true;
    }
}




