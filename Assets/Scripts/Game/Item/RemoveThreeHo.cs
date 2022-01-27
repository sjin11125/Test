using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveThreeHo : MonoBehaviour
{
    GameObject[] myChlid = new GameObject[3];
    private Image squareImage;
    public Image normalImage;
    public int ItemTurn = 3;
    void Start()
    {
        for (int i = 0; i < myChlid.Length; i++)
        {
            myChlid[i] = gameObject.transform.GetChild(i).gameObject;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);//������ ��
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("GridSquare") && myChlid[0].activeSelf == true)//����� ���õƳ�
                {
                    FindLeftRight(hit.collider.gameObject);
                    GridScript.ThreeHorizontalItem = ItemTurn;
                    myChlid[0].SetActive(false);
                    myChlid[1].SetActive(true);
                    myChlid[2].SetActive(true);
                }
                else if(hit.collider.gameObject == myChlid[1])
                {
                    myChlid[0].SetActive(true);
                    myChlid[1].SetActive(false);
                }
            }
        }

        myChlid[2].transform.GetChild(0).gameObject.GetComponent<Text>().text = GridScript.ThreeHorizontalItem.ToString();//�׻� ���ڸ� �޴µ�
        if (GridScript.ThreeHorizontalItem <= 0)
        {
            myChlid[2].SetActive(false);//0���ϸ� ��밡��������
        }
    }

    void FindLeftRight(GameObject center)
    {
        int centerIndex = -1;
        GameObject[] tempObj = new GameObject[25];
        for (int i = 0; i < 25; i++)
        {
            tempObj[i] = center.GetComponentInParent<GridScript>().transform.GetChild(i).gameObject;//�׸��� ��ũ��Ʈ �ڽĵ� ��� ����

            if (tempObj[i] == center)//�°� ���� ���õ� �ֶ� ������
            {
                centerIndex = i;
                center.GetComponent<GridSquare>().ClearOccupied();
                center.GetComponent<GridSquare>().Deactivate();
                squareImage = center.transform.GetChild(2).gameObject.GetComponent<Image>();
                squareImage.sprite = normalImage.sprite;
            }
        }
        if (centerIndex - 1 > 0)//����ģ�� ����
        {
            if (tempObj[centerIndex - 1] != null)
            {
                tempObj[centerIndex - 1].GetComponent<GridSquare>().ClearOccupied();
                tempObj[centerIndex - 1].GetComponent<GridSquare>().Deactivate();
                squareImage = tempObj[centerIndex - 1].transform.GetChild(2).gameObject.GetComponent<Image>();
                squareImage.sprite = normalImage.sprite;
            }
        }
        if (centerIndex + 1 < 25)//������ģ�� ����
        {
            if (tempObj[centerIndex + 1] != null)
            {
                tempObj[centerIndex + 1].GetComponent<GridSquare>().ClearOccupied();
                tempObj[centerIndex + 1].GetComponent<GridSquare>().Deactivate();
                squareImage = tempObj[centerIndex + 1].transform.GetChild(2).gameObject.GetComponent<Image>();
                squareImage.sprite = normalImage.sprite;
            }
        }
    }
}
