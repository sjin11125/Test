using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveThreeHo : MonoBehaviour
{
    GameObject[] myChlid = new GameObject[3];
    private Image squareImage;
    public Image normalImage;
    public int ItemTurn;
    bool useRemove;
    bool centerhave;
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
                if (hit.collider.gameObject.CompareTag("GridSquare") && useRemove == true)//����� ���õƳ�
                {
                    if (hit.collider.gameObject.transform.GetChild(2).gameObject.activeSelf == true)//�����Ѿְ� ������ �����־�� ����Ұ���
                    {
                        FindLeftRight(hit.collider.gameObject);
                    }
                    if (centerhave == true)
                    {
                        GridScript.ThreeHorizontalItem = ItemTurn;
                        myChlid[0].SetActive(false);
                        myChlid[1].SetActive(true);
                        myChlid[2].SetActive(true);
                        useRemove = false;
                    }
                }
                else if(hit.collider.gameObject == myChlid[1])
                {
                    myChlid[0].SetActive(true);
                    myChlid[1].SetActive(false);
                    useRemove = true;
                    centerhave = false;
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
        GameObject[] tempObj = new GameObject[25];
        for (int i = 0; i < 25; i++)
        {           
            tempObj[i] = GameObject.FindGameObjectWithTag("Grid").transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < tempObj.Length; i++)
        {
            if (tempObj[i] == center)//�°� ���� ���õ� �ֶ� ������
            {
                clearSquare(tempObj[i]);

                int left = i - 1;
                int right = i + 1;
                if (left > -1)
                {
                    if (left / 5 == i / 5 && tempObj[left].transform.GetChild(2).gameObject.activeSelf == true)
                    {//���� �ٿ��ִ��� ���
                        clearSquare(tempObj[left]);
                    }
                }
                if (right < 26)
                {
                    if (right / 5 == i / 5 && tempObj[right].transform.GetChild(2).gameObject.activeSelf == true)
                    {
                        clearSquare(tempObj[right]);
                    }
                }            
                centerhave = true;
            }
        }
    }

    void clearSquare(GameObject square)//���������� ����
    {
        if (square.transform.GetChild(2).gameObject.activeSelf == true)
        {
            square.GetComponent<GridSquare>().ClearOccupied();
            square.GetComponent<GridSquare>().Deactivate();
            squareImage = square.transform.GetChild(2).gameObject.GetComponent<Image>();
            squareImage.sprite = normalImage.sprite;
        }
    }
}
