using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExitController : MonoBehaviour
{
    public void Awake()
    {
        //GameLoad();
    }
    public void GameSave()
    {
        PlayerPrefs.SetInt("Money", GameManager.Money);//��
        PlayerPrefs.SetInt("ShinMoney", GameManager.ShinMoney);//��
        PlayerPrefs.Save();
        print("save");
    }
    public void GameLoad()
    {
        GameManager.ShinMoney = 10000;
        print("load");
    }
    public void GameExit()
    {
        //GameSave();
        print("exit");
        Application.Quit();
    }
}
