using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeShapeItem : MonoBehaviour
{
    public GameObject[] myChlid = new GameObject[2];
    GameObject shapestorageObj;
    public Sprite[] getShapes;// = new Sprite[6];
    public string[] getShpesName;
    public static GameObject squareObj;
    public int colorK = 0;
    public static bool changeActive;
    GameObject rainbowObj;
    public int ItemTurn;

    void Start()
    {
        for (int i = 0; i < myChlid.Length; i++)
        {
            myChlid[i] = gameObject.transform.GetChild(i).gameObject;
        }
        shapestorageObj = GameObject.FindGameObjectWithTag("ShapeStorage");

        myChlid[0].transform.GetChild(0).transform.gameObject.SetActive(true);
        myChlid[0].transform.GetChild(1).transform.gameObject.SetActive(false);
        myChlid[0].SetActive(false);

        GameObject GetRainbow = GameObject.FindGameObjectWithTag("ItemController");//��Ʈ�ѷ� �ټ���° �ڽ���
        if (GetRainbow != null)
        {
            rainbowObj = GetRainbow.transform.GetChild(5).gameObject;//���κ��� ������ ������Ʈ�� �޾�
        }
    }

    public void RainbowItemUse(string color)
    {
        myChlid[0].transform.GetChild(0).transform.gameObject.SetActive(false);
        myChlid[0].transform.GetChild(1).transform.gameObject.SetActive(true);
        int j = 0;
        for (int i = 0; i < 36; i++)
        {
            if (shapestorageObj.GetComponent<ShapeStorage>().shapeData[i].color == color)
            {
                getShapes[j] = shapestorageObj.GetComponent<ShapeStorage>().shapeData[i].sprite;
                getShpesName[j] = shapestorageObj.GetComponent<ShapeStorage>().shapeData[i].shape;
                j++;
            }
        }
    }

    public Sprite RainbowItemChange(int k)
    {
        if (k > 5)
        {
            k = 0;
            colorK = k;
        }
        return getShapes[k];
    }

    void Update()
    {
        myChlid[2].transform.GetChild(0).gameObject.GetComponent<Text>().text = GridScript.ChangeShapeItem.ToString();//�׻� ���ڸ� �޴µ�

        if (GridScript.ChangeShapeItem <= 0)
        {
            myChlid[2].SetActive(false);//0���ϸ� ��밡��������
        }

        if (Input.GetMouseButtonDown(0))//���� �������
        {
            Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray = new Ray2D(wp, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("GridSquare") && changeActive)//����� ���õƱ��ѵ�
                {
                    if (hit.collider.gameObject == squareObj)
                    {
                        squareObj.transform.GetChild(2).GetComponent<Image>().sprite = RainbowItemChange(colorK);
                        squareObj.GetComponent<GridSquare>().currentShape = getShpesName[colorK];//��������
                        colorK++;
                    }
                    else//�ٸ� �������
                    {
                        if (squareObj != null)//������ ��Ŭ����������
                        {
                            changeActive = false;
                        }
                    }
                }
                else if (hit.collider.gameObject == myChlid[0])//���Ϸ�
                {
                    GridScript.ChangeShapeItem = ItemTurn;
                    myChlid[2].SetActive(true);
                    myChlid[1].SetActive(true);
                    myChlid[0].transform.GetChild(0).transform.gameObject.SetActive(true);
                    myChlid[0].transform.GetChild(1).transform.gameObject.SetActive(false);
                    myChlid[0].SetActive(false);
                    GameObject.FindGameObjectWithTag("Grid").GetComponent<GridScript>().CheckIfKeepLineIsCompleted();
                    squareObj = null;
                    changeActive = false;
                    if (rainbowObj != null)
                    {
                        rainbowObj.SetActive(true);
                    }
                }
                else if (hit.collider.gameObject == myChlid[1])//���
                {
                    myChlid[0].SetActive(true);
                    myChlid[1].SetActive(false);
                    changeActive = true;
                    //�긦 ��봩���� �÷��������� �����ߵ�
                    if (rainbowObj != null)
                    {
                        rainbowObj.SetActive(false);
                    }
                }
            }
        }
    }
}
