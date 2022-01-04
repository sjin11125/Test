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
    int count = 0;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)&& count == 0&& hooverObj.activeSelf) //��Ŭ�Ҷ�&&��ư ����������
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);//������ ��
            if (hit.collider != null)
            {
                if(hit.collider.gameObject.CompareTag("GridSquare"))//����� ���õƳ�
                {
                    GameObject contectChild = hit.collider.gameObject.transform.GetChild(2).gameObject; //����° �ڽĿ� activeImage����
                    squareImage = contectChild.GetComponent<Image>();
                    squareImage.sprite = normalImage.sprite;//������� �ٲ�

                    GameObject contectSquare = hit.collider.gameObject.transform.gameObject; //�θ� �޾�
                    contectSquare.GetComponent<GridSquare>().ClearOccupied(); //��ũ��Ʈ�� ���þȵȿɼ����� �ٲ�
                    contectSquare.GetComponent<GridSquare>().Deactivate();

                    count++;
                }
            }
        }

        if (normalObj.activeSelf)
        {
            count = 0;
        }
    }
}
