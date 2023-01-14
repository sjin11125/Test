using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

[Serializable]
public class Cardsave
{
    
    public string Uid;

    public string cardImage;
    public string isLock;
    public string Id;

    public Cardsave(string uid, string cardImage, string isLock, string id)
    {
        Uid = uid;
        this.cardImage = cardImage;
        this.isLock = isLock;
        Id = id;
    }
}
[Serializable]
public class UIEdit
{
    public BuildUIType buildUIType;
    public Button btn;
    public GameObject prefab;

}
[System.Serializable]
public class UserInfo               //��������
{
    public string Uid;
    public string Money;
    public string Message;
    public string Image;    
    public string ShinMoney;
    public string BestScore;
    public string Tuto;
    public string Version;
}
[System.Serializable]
public class SendMessage
{
    public SendMessage(string _name, string _message)
    {
        name = _name;
        message = _message;
    }
    public string name;
    public string message;
}
[System.Serializable]
public class Buildingsave
{
    public string order, result, msg;

    public string Uid;              //�÷��̾� UID

    public string BuildingPosition_x;                //�ǹ� ��ġ(x��ǥ)
    public string BuildingPosition_y;                //�ǹ� ��ġ(y��ǥ)
    //-------------------------�Ľ�����------------------------------
    public string isLock;               //��� ����
    public string Building_name;            //�ǹ� �̸�
    //public string Reward;               //ȹ���ڿ�
    //public string Info;                 //�ǹ� ����
    public string Building_Image;          //���� �̹��� �̸� *
    public string Cost;        //�ǹ����
    public string ShinCost;
    public string Level;       //�ǹ� ����
    public string isFliped;        //������������
    public string Id;

    public Buildingsave(string buildingPosition_x, string buildingPosition_y, string isLock, string building_name, string building_Image, string level, string isFlied, string id)
    {
        BuildingPosition_x = buildingPosition_x;
        BuildingPosition_y = buildingPosition_y;
        this.isLock = isLock;
        Building_name = building_name;
        Building_Image = building_Image;
        Level = level;
        this.isFliped = isFlied;
        Id = id;
    }
}