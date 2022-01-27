using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RainbowItem : MonoBehaviour
{
    public GameObject[] myChlid = new GameObject[2];
    GameObject shapestorageObj;
    public Sprite[] getColors;// = new Sprite[6];
    public string[] getColorName;
    public static GameObject squareColorObj;
    public int colorK = 0;
    public static bool rainbowActive;
    GameObject ChangeShapeObj;
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
            ChangeShapeObj = GetRainbow.transform.GetChild(6).gameObject;//���κ��� ������ ������Ʈ�� �޾�
        }
    }

    public void RainbowItemUse(string shape)
    {
        myChlid[0].transform.GetChild(0).transform.gameObject.SetActive(false);
        myChlid[0].transform.GetChild(1).transform.gameObject.SetActive(true);
        int j = 0;
        for (int i = 0; i < 36; i++)
        {
            if (shapestorageObj.GetComponent<ShapeStorage>().shapeData[i].shape == shape)//����� �״�� �ΰ� ������ �ٲ�� �Ǵϱ� ���� �����
            {
                getColors[j] = shapestorageObj.GetComponent<ShapeStorage>().shapeData[i].sprite;//��������Ʈ ����
                getColorName[j] = shapestorageObj.GetComponent<ShapeStorage>().shapeData[i].color;//�� ��������Ʈ ����� ����
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
        return getColors[k];
    }

    void Update()
    {
        myChlid[2].transform.GetChild(0).gameObject.GetComponent<Text>().text = GridScript.RainbowItemTurn.ToString();//�׻� ���ڸ� �޴µ�

        if (GridScript.RainbowItemTurn <= 0)
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
                if (hit.collider.gameObject.CompareTag("GridSquare")&& rainbowActive)//����� ���õƱ��ѵ�
                {
                    if (hit.collider.gameObject == squareColorObj)//�ٲٰ��ִ� �ְ��ƴ϶�
                    {
                        squareColorObj.transform.GetChild(2).GetComponent<Image>().sprite = RainbowItemChange(colorK);
                        squareColorObj.GetComponent<GridSquare>().keepCurrentColor = getColorName[colorK];//��������
                        colorK++;
                    }
                    else//�ٸ� �������
                    {
                        if (squareColorObj != null)//������ ��Ŭ����������
                        {
                            rainbowActive = false;
                        }
                    }
                }
                else if (hit.collider.gameObject == myChlid[0])//���Ϸ�
                {
                    GridScript.RainbowItemTurn = ItemTurn;
                    myChlid[2].SetActive(true);
                    myChlid[1].SetActive(true);
                    myChlid[0].transform.GetChild(0).transform.gameObject.SetActive(true);
                    myChlid[0].transform.GetChild(1).transform.gameObject.SetActive(false);
                    myChlid[0].SetActive(false);
                    GameObject.FindGameObjectWithTag("Grid").GetComponent<GridScript>().CheckIfKeepLineIsCompleted();
                    GridScript.ChangeShapeItem++;
                    GridScript.EraserItemTurn++;
                    GridScript.ReloadItemTurn++;
                    GridScript.NextExchangeItemTurn++;
                    GridScript.TrashItemTurn++;
                    squareColorObj = null;
                    rainbowActive = false;
                    if (ChangeShapeObj != null)
                    {
                        ChangeShapeObj.SetActive(true);
                    }
                }
                else if(hit.collider.gameObject == myChlid[1])//���
                {
                    myChlid[0].SetActive(true);
                    myChlid[1].SetActive(false);
                    rainbowActive = true;
                    //�긦 ��봩���� �������������� �����ߵ�
                    if (ChangeShapeObj != null)
                    {
                        ChangeShapeObj.SetActive(false);
                    }
                }
            }
        }
    }
}
