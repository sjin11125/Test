using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EraserItem : MonoBehaviour
{
    private Image squareImage;
    public Image normalImage;
    public GameObject normalObj;
    public GameObject hooverObj;
    bool buttonDown;
    public int ItemTurn= 20;
    void Start()
    {
        GridScript.EraserItemTurn = ItemTurn;
        hooverObj.SetActive(true);
        hooverObj.transform.GetChild(0).gameObject.GetComponent<Text>().text = GridScript.EraserItemTurn.ToString();
    }
    void Update()
    {
        hooverObj.transform.GetChild(0).gameObject.GetComponent<Text>().text = GridScript.EraserItemTurn.ToString();
        
        if (Input.GetMouseButtonDown(0) && buttonDown == true)// && hooverObj.activeSelf == false) //��Ŭ�Ҷ�&&��ư ����������
        {
            GridScript.EraserItemTurn = ItemTurn;
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);//������ ��
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("GridSquare"))//����� ���õƳ�
                {
                    GameObject contectChild = hit.collider.gameObject.transform.GetChild(2).gameObject; //����° �ڽĿ� activeImage����
                    squareImage = contectChild.GetComponent<Image>();
                    squareImage.sprite = normalImage.sprite;//������� �ٲ�

                    GameObject contectSquare = hit.collider.gameObject.transform.gameObject; //�θ� �޾�
                    contectSquare.GetComponent<GridSquare>().ClearOccupied(); //��ũ��Ʈ�� ���þȵȿɼ����� �ٲ�
                    contectSquare.GetComponent<GridSquare>().Deactivate();
                    buttonDown = false;
                }
            }
        }

        if(buttonDown == true)
        {
            hooverObj.SetActive(true);
            hooverObj.transform.GetChild(0).gameObject.GetComponent<Text>().text = " ";
        }
        else
        {
            if (GridScript.EraserItemTurn == ItemTurn)
            {
                hooverObj.SetActive(true);
            }
            else if(GridScript.EraserItemTurn == 0)
            {
                hooverObj.SetActive(false);
            }
        }
    }

    public void NormalBtnOnClick()
    {
        buttonDown = true;      
    }
}
