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
            gameObject.GetComponent<Image>().sprite = KeepObj.GetComponent<GridScript>().KeepImg;
        }
    }

    void Update()
    {
        GridScript.KeepItemTurn = 30;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GridSquare")
        {
            if (collision.gameObject.GetComponent<GridSquare>().SquareOccupied == false)//비어있으면
            {
                hitKeepObj = collision.gameObject;
                hitKeepObj.GetComponent<GridSquare>().keepImage = gameObject.GetComponent<Image>().sprite;//스프라이트를 상대 스퀘어에 전달
                HitChildCount = hitKeepObj.transform.GetSiblingIndex();//그리드에서 몇번째 인덱스와 부딫혔나               
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

        if (drop == true)//놓았으면
        {
            hitKeepObj.GetComponent<GridSquare>().UseSquareKeep();//상대 오브젝트를 켠다
            Destroy(this.gameObject);//이 프리팹은 삭제된다
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
    }
    public void OnDrop(PointerEventData eventData)
    {

        
    }
}
