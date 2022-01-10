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

    public GameObject activeObj;

    public string currentColor;
    public string currentShape;
    public Sprite keepImage;
    public static bool UseKeepBool = false;

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
        if (SquareOccupied == false)//������� �ƴϸ�
        {
            Selected = true;//���õȰɷιٲ�
            hooverImage.gameObject.SetActive(true);//���ѻ�
            
            GameObject ShapeStorageObj = GameObject.FindGameObjectWithTag("ShapeStorage");
            if (ShapeStorageObj != null)//���⼭ �׻� ���� shape�� ������ �޴´�
            {                             
                currentColor = ShapeStorageObj.GetComponent<ShapeStorage>().shapeColor;
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
            currentColor = null;
            currentShape = null;
        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().UnSetOccupied();//�������Ʈ��
        }

        GameObject GridObj = GameObject.FindGameObjectWithTag("Grid");
        if (GridObj != null && UseKeepBool == true)
        {
            currentColor = GridObj.GetComponent<GridScript>().KeepColor;//������Ȯ���ؼ� keep�� ����ߴ��Ŷ�� 
            currentShape = GridObj.GetComponent<GridScript>().KeepShape;
        }
    }

    public void TrashCan()
    {
        activeImage.sprite = normalImage.sprite;
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
