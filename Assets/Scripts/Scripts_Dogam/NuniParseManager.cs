using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NuniParseManager : MonoBehaviour
{   //GameManager���� �Ľ��� ���� �������� �޾� ���� ���� �гο� �ֱ�

    public GameObject NuniPannelPrefab;           //���� �г� ������
    public GameObject Scroll;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NuniDogamOpen()             //���� ���� �������� ��
    {
        //GM�� �ִ� ��� ���� ���� �ҷ��� �гο� �ֱ�
        for (int i = 0; i < GameManager.AllNuniArray.Length; i++)
        {
            GameObject NuniPannel = Instantiate(NuniPannelPrefab) as GameObject;
            NuniPannel.transform.SetParent(Scroll.transform);
            

            Button NuniButton = NuniPannel.GetComponentInChildren<Button>();
            Image[] image = NuniPannel.GetComponentsInChildren<Image>();
            
            if (GameManager.AllNuniArray[i].isLock=="F")       // ���ϸ� ���� ������ ���� ��
            {
                NuniButton.enabled = true;
                image[2].sprite = GameManager.AllNuniArray[i].GetChaImange();   //���� �̹��� �ֱ�
                NuniPannel.name = GameManager.AllNuniArray[i].cardImage;        //���� �̸� �ֱ�
            }
            else
            {
                NuniButton.enabled = false;
                NuniPannel.name = "?";
            }


        }
    }
}
