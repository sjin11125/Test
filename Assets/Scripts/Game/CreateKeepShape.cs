using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreateKeepShape : MonoBehaviour, IPointerDownHandler,IBeginDragHandler,IEndDragHandler,IDragHandler,IDropHandler //, IDragHandler
{
    private RectTransform _transform;
    private Vector3 _startPosition;
    private Canvas canvas;
    public bool drop = false;
    private RectTransform rectTransform;

    public string keepColor;
    public string keepShape;
    public GameObject hitKeepObj;
    GameObject KeepObj;
    int HitChildCount;

    private void Awake()
    {
        _transform = this.GetComponent<RectTransform>();
        _startPosition = _transform.localPosition;

        gameObject.tag = "KeepShape";
        rectTransform = GetComponent<RectTransform>();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
    }
    private void Start()
    {
        KeepObj = GameObject.FindGameObjectWithTag("Grid");
        if (KeepObj != null)
        {
            //keep ������̸� keep�̹���
            gameObject.GetComponent<Image>().sprite = KeepObj.GetComponent<GridScript>().KeepImg;
            //��ĭ�̵� ������̸�
            /*
            if (KeepObj.GetComponent<GridScript>().OneNum > 0)
            {
                gameObject.GetComponent<Image>().sprite = KeepObj.GetComponent<GridScript>().moveImg;
            }*/
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
       gameObject.tag = "Shape";
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
            GridScript.KeepItemTurn = 3;
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
