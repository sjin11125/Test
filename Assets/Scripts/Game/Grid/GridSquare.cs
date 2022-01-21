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
    public float minClickTime = 0.8f;
    private bool isClick;
    GameObject rainbowObj;
    bool rainBowUse = false;
    public int colorK;
    void Start()
    {
        GridScript.TrashItemTurn = 20;
        GridScript.KeepItemTurn = 30;
        Selected = false;
        SquareOccupied = false;
        keepCurrentColor = null;
        currentShape = null;

        GameObject contectShape = GameObject.FindGameObjectWithTag("Shape");
        if (contectShape != null)
        {
            GameObject squareImage = contectShape.transform.GetChild(0).gameObject;
            spriteImage = squareImage.GetComponent<Image>();
        }
        GameObject GetRainbow = GameObject.FindGameObjectWithTag("ItemController");//��Ʈ�ѷ� �ټ���° �ڽ���
        if(GetRainbow != null)
        {
            rainbowObj = GetRainbow.transform.GetChild(5).gameObject;//���κ��� ������ ������Ʈ�� �޾�
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            isClick = true;
            rainBowUse = false;
            Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray = new Ray2D(wp, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    rainBowUse = true;
                          
                    //�ٸ� ���������� �ٲ������°� �ᵵ��
                }
                else
                {
                    rainBowUse = false;
                    //rainbowObj.GetComponent<RainbowItem>().UseThisItemEx();
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isClick = false;
            if (clickTime >= minClickTime)//��Ŭ���̾�����
            {
                if (!rainbowObj.GetComponent<RainbowItem>().myChlid[1].activeSelf && activeImage.gameObject.activeSelf)//������ ������Ʈ��밡���̰� ������ ������
                {
                    if (rainBowUse == true)
                    {
                        rainbowObj.GetComponent<RainbowItem>().RainbowItemUse(keepCurrentColor);//���κ��� ������ �Լ� ȣ��
                        rainbowObj.GetComponent<RainbowItem>().myChlid[1].SetActive(true);//����̹��� �ѱ�
                        //rainbowObj.GetComponent<RainbowItem>().UseThisItem(this.gameObject);
                        print(keepCurrentColor);
                        clickTime = 0;
                    }         
                }
            }
            else
            {
                if (rainBowUse == true)
                {
                    if(colorK == 6)
                    {
                        colorK = 0;
                    }
                    activeImage.sprite = rainbowObj.GetComponent<RainbowItem>().RainbowItemChange(colorK);//���� �������
                    print(colorK);
                    colorK++;
                }
            }
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
        UseKeepBool = false;
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
        if (GridObj != null && UseKeepBool == true)
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
