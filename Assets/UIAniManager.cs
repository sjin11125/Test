using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UIAniManager : MonoBehaviour
{

    public RectTransform mainMenu, ShopMenu;

    public RectTransform startMenu;

    //public StartManager startmanager;
    // Start is called before the first frame update

    public void Start()
    {
        try
        {
            Debug.Log("���� �߾����� �̵�");
            mainMenu.DOAnchorPos(Vector2.zero, 0.5f); // �����г� ó���� �߰�����
            Debug.Log("���� �߾����� �̵� ��");
        }
        catch (System.Exception e)
        {
            Debug.Log( e.Message);
            throw;
        }
                                      //startMenu.transform.localScale = Vector2.zero;
    }

    // Update is called once per frame

    public void ShopButton()
    {
        mainMenu.DOAnchorPos(new Vector2(-1000, 0), 0.5f);
        ShopMenu.DOAnchorPos(new Vector2(0, 0), 0.5f);
    }

    public void CloseShopButton()
    {
        mainMenu.DOAnchorPos(new Vector2(0, 0), 0.5f);
        ShopMenu.DOAnchorPos(new Vector2(0, 2000), 0.5f);
    }
    
    public void StartOpen()
    {
        mainMenu.DOAnchorPos(new Vector2(-1000, 0), 0.5f);
        //startMenu.transform.DOScale(Vector3.one, 0.3f);
        //startmanager.CharacterOpen();

    }

    public void StartClose()
    {
        startMenu.transform.DOScale(Vector3.zero, 0.3f);

    }

    void Update()
    {

    }
}