using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerInfo : MonoBehaviour
{
    public static string Id;            //�÷��̾� ���̵�
    public static string NickName;      //�÷��̾� �г���
    public static string SheetsNum;     //�÷��̾� �ǹ� ���� ����ִ� �������� ��Ʈ id
    public static string Info;          //���¸޼���

    string[] Friends;       //ģ�� ���(�г���)
    // Start is called before the first frame update
    void Start()
    {
        Friends = new string[1];
        Friends[1] = "Vicky";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
