using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    public Image hooverImage;
    public Image activeImage;
    public Image normalImage;
    public List<Sprite> normalImages;

    [HideInInspector]
    public Image spriteImage;

    public bool Selected { get; set; }
    public int SquareIndex { get; set; }
    public bool SquareOccupied { get; set; }

    public GameObject keepTimer;
    public GameObject activeObj;

    [HideInInspector]
    public int keepCount, trashCount;

    public string currentColor;
    public string currentShape;
    public GameObject KeepShape;

    void Start()
    {
        Selected = false;
        SquareOccupied = false;
        currentColor = null;
        currentShape = null;

        GameObject contectShape = GameObject.FindGameObjectWithTag("Shape");
        if (contectShape != null)
        {
            GameObject squareImage = contectShape.transform.GetChild(0).gameObject;
            spriteImage = squareImage.GetComponent<Image>();
        }
    }

    void Update()
    {
        if (keepTimer.activeSelf ==false)
        {
            keepCount = 0;
        }
        if (keepTimer.activeSelf == false)
        {
            trashCount = 0;
        }
    }

    //temp function remove it
    public bool CanWeUseThisSquare()
    {
        return hooverImage.gameObject.activeSelf;
    }

    public void PlaceShapeOnBoard()//�׸��� ��ũ��Ʈ CheckIfShapeCanBePlaced���� ���
    {
        ActivateSquare();
    }

    public void ActivateSquare()
    {
        hooverImage.gameObject.SetActive(false);//���õǰ��ִ��߿��ߴ� ���ѻ�����
        activeImage.gameObject.SetActive(true);//���õ� �� �ѱ�

        Selected = true; //���õ�
        SquareOccupied = true; //�����

        if (activeImage.gameObject.activeSelf == true)
        {
            activeImage.GetComponent<Image>().sprite = spriteImage.sprite;//������ ��������Ʈ ����
        }
    }

    public void Deactivate()
    {
        activeImage.gameObject.SetActive(false);
        activeImage.GetComponent<Image>().sprite = null;//��������� ���� �ȴ����س��� �̰Ż�Ǿ���ɵ�?
        currentColor = null;
        currentShape = null;
    }

    public void NonKeep()//keep ���� ������ �ֵ鿡 ���
    {
        gameObject.SetActive(false);
    }

    public void ClearOccupied()
    {
        Selected = false;
        SquareOccupied = false;
    }

    public void SetImage(bool setFirstImage)
    {
        normalImage.GetComponent<Image>().sprite = setFirstImage ? normalImages[1] : normalImages[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)//�浹ó��
    {
        if(SquareOccupied == false)//������� �ƴϸ�
        {
            Selected = true;//���õȰɷιٲ�
            hooverImage.gameObject.SetActive(true);//���ѻ�
            GameObject ShapeStorageObj = GameObject.FindGameObjectWithTag("ShapeStorage");
            if (ShapeStorageObj != null)
            {
                currentColor = ShapeStorageObj.GetComponent<ShapeStorage>().shapeColor;
                currentShape = ShapeStorageObj.GetComponent<ShapeStorage>().shapeShape;
            }
        }
        else if(collision.GetComponent<ShapeSquare>() != null)//�������� �������
        {
            collision.GetComponent<ShapeSquare>().SetOccupied();//������ �������Ʈ
        }
    }

    private void OnTriggerStay2D(Collider2D collision)//�浹��
    {
        Selected = true;//���õȰɷιٲ�

        if (SquareOccupied == false)//������� �ƴϸ�
        {            
            hooverImage.gameObject.SetActive(true);
        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().SetOccupied();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)//�浹���
    {
        if (SquareOccupied == false)//������� �ƴϸ�
        {
            Selected = false;//���þȵȰɷ���
            hooverImage.gameObject.SetActive(false);//���ѻ� ��
            currentColor = null;
            currentShape = null;
        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().UnSetOccupied();//�������Ʈ��
        }
    }

    GameObject twentyNine;
    int keepSquareIndex;
    //public GameObject prefab;
    public void ColorTransfer() //�׸��彺ũ��Ʈ UseKeep�� ����
    {
        GameObject ItemControllerObj = GameObject.FindGameObjectWithTag("ItemController");//��Ʈ�ѷ����� ������ �ε����� ���� ��ġ ����
        if (ItemControllerObj != null)
        {
            keepSquareIndex = ItemControllerObj.GetComponent<ItemController>().keepItemIndex;
        }

        GameObject contectGrid = GameObject.FindGameObjectWithTag("Grid");
        if (contectGrid != null)
        {
            twentyNine = contectGrid.transform.GetChild(keepSquareIndex).gameObject; //29��������Ʈ����
        }

        if (Input.GetMouseButtonDown(0)) //��Ŭ�Ҷ�
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);//������ ��
            if (hit.collider != null)
            {
                if (hit.collider.gameObject == twentyNine && keepCount == 0)// && activeObj.activeSelf==true)//���� ������ 29��������Ʈ�� �°� �ѹ� ������ ��Ƽ�������Ʈ�� ������
                {
                    keepOnclick();
                    /*
                    spriteImage.sprite = activeImage.GetComponent<Image>().sprite;//�����ص� ������ shape�� ��
                    GameObject useKeepShape = GameObject.FindGameObjectWithTag("ShapeStorage");//keep���ִ� �������������� ���� shape�� �ش�
                    if (useKeepShape != null)
                    {
                        useKeepShape.GetComponent<ShapeStorage>().shapeColor = currentColor;
                        useKeepShape.GetComponent<ShapeStorage>().shapeShape = currentShape;
                    }*/
                    currentColor = null;//�ְ��� ����
                    currentShape = null;
                    keepTimer.SetActive(true);                  
                    activeObj.SetActive(false);
                    keepCount++;
                }
            }
        }
    }

    public void TrashCan()
    {
        activeImage.sprite = normalImage.sprite;

        if (trashCount == 0)
        {  
             keepTimer.SetActive(true);
             activeObj.SetActive(false);
             trashCount++;
        }
    }
    
    public void keepOnclick()
    {
        GameObject keepInstance = Instantiate(KeepShape) as GameObject;
        keepInstance.transform.SetParent(this.transform, false);
        Vector3 pos = new Vector3(0, 0, 0);
        keepInstance.transform.localPosition = pos;

        GameObject useKeepShape = GameObject.FindGameObjectWithTag("KeepShape");//keep���ִ� �������������� ���� shape�� �ش�
        if (useKeepShape != null)
        {
            useKeepShape.GetComponent<CreateKeepShape>().keepColor = currentColor;
            useKeepShape.GetComponent<CreateKeepShape>().keepShape = currentShape;
            useKeepShape.GetComponent<Image>().sprite = activeImage.sprite;
        }
        
    }
}
