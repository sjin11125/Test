using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManger : MonoBehaviour
{
    //canvas�� �ؽ�Ʈ�� ��ȭ �����ض�
    public Text Money;          //��ȭ
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Money.text = "��ȭ: "+GameManager.Money.ToString();
    }
}
