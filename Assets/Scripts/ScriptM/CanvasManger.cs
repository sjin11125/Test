using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManger : MonoBehaviour
{
    //canvas�� �ؽ�Ʈ�� ��ȭ �����ض�
    public Text Money;          //��ȭ
    public Text Tree;          //��ȭ
    public Text Snow;          //��ȭ
    public Text Ice;          //��ȭ
    public Text Grass;          //��ȭ
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Money.text = "��ȭ: "+GameManager.Money.ToString();
        Tree.text = GameManager.Tree.ToString();
        Snow.text = GameManager.Snow.ToString();
        Ice.text = GameManager.Ice.ToString();
        Grass.text = GameManager.Grass.ToString();
    }
}
