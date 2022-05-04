using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    public GameObject activeObj;

    public string keepCurrentColor;
    public string currentShape;
    public Sprite keepImage;
    public static bool UseKeepBool = false;

    private float clickTime;
    private float minClickTime = 0.8f;
    private bool isClick;
    GameObject rainbowObj;
    GameObject ChangeShapeObj;
    GameObject squareImage;

    public bool shinActive;

    void Start()
    {
        Selected = false;
        SquareOccupied = false;
        keepCurrentColor = null;
        currentShape = null;
        shinActive = false;

        GameObject contectShape = GameObject.FindGameObjectWithTag("Shape");
        if (contectShape != null)
        {
            squareImage = contectShape.transform.GetChild(0).gameObject;
            spriteImage = squareImage.GetComponent<Image>();
        }
        GameObject GetRainbow = GameObject.FindGameObjectWithTag("ItemController");//��Ʈ�ѷ� �ټ���° �ڽ���
        if(GetRainbow != null)
        {
            rainbowObj = GetRainbow.transform.GetChild(4).gameObject;//���κ��� ������ ������Ʈ�� �޾�
            ChangeShapeObj = GetRainbow.transform.GetChild(5).gameObject;
        }
    }

    private void Update()
    {
        Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray2D ray = new Ray2D(wp, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (Input.GetMouseButtonDown(0))
        {
            isClick = true;
            if (gameObject.transform.GetChild(2).gameObject.activeSelf)//�������� ���Ŀ� ��Ŭ��
            {
                if (clickTime >= minClickTime)//��Ŭ���̾�����
                {
                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject == this.gameObject)
                        {
                            if(GridScript.RainbowItemTurn <= 0 && RainbowItem.rainbowActive)
                            {
                                rainbowObj.GetComponent<RainbowItem>().RainbowItemUse(currentShape);//���κ��� ������ �Լ� ȣ��
                                RainbowItem.squareColorObj = this.gameObject;
                                print(currentShape);
                            }
                            else if (GridScript.ChangeShapeItem <= 0 && ChangeShapeItem.changeActive)
                            {
                                ChangeShapeObj.GetComponent<ChangeShapeItem>().RainbowItemUse(keepCurrentColor);//�÷��ٲٴ� ������ �Լ� ȣ��
                                ChangeShapeItem.squareObj = this.gameObject;
                                print(keepCurrentColor);
                            }
                        }
                    }
                }
            }
            clickTime = 0;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isClick = false;               
        }

        if (isClick)
        {
            clickTime += Time.deltaTime;
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
        if (squareImage.transform.GetChild(0).gameObject.activeSelf)//shin�� ����������
        {
            activeImage.transform.GetChild(0).gameObject.SetActive(true);
            shinActive = true;
            print("hi");
        }

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
        keepCurrentColor = null;
        currentShape = null;

        activeImage.transform.GetChild(0).gameObject.SetActive(false);
        shinActive = false;
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
        if (SquareOccupied == false)//������� �ƴϸ�
        {
            Selected = true;//���õȰɷιٲ�
            hooverImage.gameObject.SetActive(true);//���ѻ�
            
            GameObject ShapeStorageObj = GameObject.FindGameObjectWithTag("ShapeStorage");
            if (ShapeStorageObj != null)//���⼭ �׻� ���� shape�� ������ �޴´�
            {                             
                keepCurrentColor = ShapeStorageObj.GetComponent<ShapeStorage>().shapeColor;
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
            keepCurrentColor = null;
            currentShape = null;
        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().UnSetOccupied();//�������Ʈ��
        }
        
        GameObject GridObj = GameObject.FindGameObjectWithTag("Grid");
        if (GridObj != null && UseKeepBool == true)//ŵ������ ��� ��
        {
            keepCurrentColor = GridObj.GetComponent<GridScript>().KeepColor;//������Ȯ���ؼ� keep�� ����ߴ��Ŷ��
            currentShape = GridObj.GetComponent<GridScript>().KeepShape;
        }
    }

    public void UseSquareKeep()//ŵ �����հ� ������ ������ �Լ�
    {
        UseKeepBool = true;
        
        hooverImage.gameObject.SetActive(false);//���õǰ��ִ��߿��ߴ� ���ѻ�����
        activeImage.gameObject.SetActive(true);//���õ� �� �ѱ�

        Selected = true; //���õ�
        SquareOccupied = true; //�����
        gameObject.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = keepImage;        
    }
}
