using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreateKeepShape : MonoBehaviour, IPointerDownHandler,IBeginDragHandler,IEndDragHandler,IDragHandler,IDropHandler //, IDragHandler
{
    private Vector3 _startPosition;
    private Canvas canvas;
    public bool drop = false;
    private RectTransform rectTransform;

    public string keepColor;
    public string keepShape;
    public GameObject hitKeepObj;
    GameObject KeepObj;
    int HitChildCount;
    int keepIndex;

    private void Awake()
    {       
        _startPosition = new Vector3(-377f, -660.5f, 0);
        gameObject.tag = "KeepShape";
        rectTransform = GetComponent<RectTransform>();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();

        GameObject ItemControllerObj = GameObject.FindGameObjectWithTag("ItemController");//��Ʈ�ѷ����� ������ �ε����� ���� ��ġ ����
        if (ItemControllerObj != null)
        {
            keepIndex = ItemControllerObj.GetComponent<ItemController>().keepItemIndex;
        }

        KeepObj = GameObject.FindGameObjectWithTag("Grid").transform.GetChild(keepIndex).gameObject;
        if (KeepObj != null)
        {
            gameObject.GetComponent<Image>().sprite = KeepObj.GetComponent<GridSquare>().activeImage.sprite;
            keepColor = KeepObj.GetComponent<GridSquare>().keepCurrentColor;
            keepShape = KeepObj.GetComponent<GridSquare>().currentShape;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GridSquare")
        {
            if (collision.gameObject.GetComponent<GridSquare>().SquareOccupied == false)//���������
            {
                hitKeepObj = collision.gameObject;
                hitKeepObj.GetComponent<GridSquare>().keepImage = gameObject.GetComponent<Image>().sprite;//��������Ʈ�� ��� ����� ����
                HitChildCount = hitKeepObj.transform.GetSiblingIndex();//�׸��忡�� ���° �ε����� �΋H����               
                drop = true;
            }
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
        drop = false;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        gameObject.transform.localPosition = _startPosition;

        if (drop == true)//��������
        {
            hitKeepObj.GetComponent<GridSquare>().UseSquareKeep();//��� ������Ʈ�� �Ҵ�
            GridScript.KeepItemTurn = 30;
            KeepObj.SetActive(true);
            Destroy(this.gameObject);//�� �������� �����ȴ�
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
    }
    public void OnDrop(PointerEventData eventData)
    {

        
    }
}
