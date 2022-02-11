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

        GameObject GetRainbow = GameObject.FindGameObjectWithTag("ItemController");//컨트롤러 다섯번째 자식인
        if (GetRainbow != null)
        {
            ChangeShapeObj = GetRainbow.transform.GetChild(6).gameObject;//레인보우 아이템 오브젝트를 받아
        }
    }

    public void RainbowItemUse(string shape)
    {
        myChlid[0].transform.GetChild(0).transform.gameObject.SetActive(false);
        myChlid[0].transform.GetChild(1).transform.gameObject.SetActive(true);
        int j = 0;
        for (int i = 0; i < 36; i++)
        {
            if (shapestorageObj.GetComponent<ShapeStorage>().shapeData[i].shape == shape)//모양은 그대로 두고 색깔을 바꿔야 되니까 같은 모양의
            {
                getColors[j] = shapestorageObj.GetComponent<ShapeStorage>().shapeData[i].sprite;//스프라이트 저장
                getColorName[j] = shapestorageObj.GetComponent<ShapeStorage>().shapeData[i].color;//그 스프라이트 색깔들 저장
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
        myChlid[2].transform.GetChild(0).gameObject.GetComponent<Text>().text = GridScript.RainbowItemTurn.ToString();//항상 숫자를 받는데

        if (GridScript.RainbowItemTurn <= 0)
        {
            myChlid[2].SetActive(false);//0이하면 사용가능해지게
        }

        if (Input.GetMouseButtonDown(0))//다음 모양으로
        {
            Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray = new Ray2D(wp, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("GridSquare")&& rainbowActive)//스퀘어가 선택됐긴한데
                {
                    if (hit.collider.gameObject == squareColorObj)//바꾸고있는 애가아니라
                    {
                        squareColorObj.transform.GetChild(2).GetComponent<Image>().sprite = RainbowItemChange(colorK);
                        squareColorObj.GetComponent<GridSquare>().keepCurrentColor = getColorName[colorK];//색깔전달
                        colorK++;
                    }
                    else//다른 스퀘어면
                    {
                        if (squareColorObj != null)//그전에 롱클릭없었을시
                        {
                            rainbowActive = false;
                        }
                    }
                }
                else if (hit.collider.gameObject == myChlid[0])//사용완료
                {
                    GridScript.RainbowItemTurn = ItemTurn;
                    myChlid[2].SetActive(true);
                    myChlid[1].SetActive(true);
                    myChlid[0].transform.GetChild(0).transform.gameObject.SetActive(true);
                    myChlid[0].transform.GetChild(1).transform.gameObject.SetActive(false);
                    myChlid[0].SetActive(false);
                    GameObject.FindGameObjectWithTag("Grid").GetComponent<GridScript>().CheckIfKeepLineIsCompleted();
                    squareColorObj = null;
                    rainbowActive = false;
                    if (ChangeShapeObj != null)
                    {
                        ChangeShapeObj.SetActive(true);
                    }
                }
                else if(hit.collider.gameObject == myChlid[1])//사용
                {
                    myChlid[0].SetActive(true);
                    myChlid[1].SetActive(false);
                    rainbowActive = true;
                    //얘를 사용누르면 쉐이프아이템은 꺼져야됨
                    if (ChangeShapeObj != null)
                    {
                        ChangeShapeObj.SetActive(false);
                    }
                }
            }
        }
    }
}
