using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NuniParseManager : MonoBehaviour
{   //GameManager���� �Ľ��� ���� �������� �޾� ���� ���� �гο� �ֱ�

    public GameObject NuniPannelPrefab;           //���� �г� ������
    public GameObject Scroll;

    public GameObject NuniInfoPanel;
    public static Image[] NuniInfoImages;
    public static Text[] NuniInfoTexts;
    public static bool isNuniButtonClick;

    public static Card SelectedNuni;                //Ŭ���� ����

    public Text ZemText;
    // Start is called before the first frame update
    void Start()
    {
        NuniInfoImages=NuniInfoPanel.GetComponentsInChildren<Image>();
        NuniInfoTexts=NuniInfoPanel.GetComponentsInChildren<Text>();

        SelectedNuni = this.gameObject.AddComponent<Card>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isNuniButtonClick)              //���Ϲ�ư Ŭ���߳�
        {
            isNuniButtonClick = false;
            NuniInfoPanel.SetActive(true);
        }
    }

    public void NuniDogamOpen()             //���� ���� �������� ��
    {
        ZemText.text = "���� ��: " + GameManager.Zem;
        GameManager.isMoveLock = true;
        //GM�� �ִ� ��� ���� ���� �ҷ��� �гο� �ֱ�
        Transform[] child=Scroll.GetComponentsInChildren<Transform>();
        for (int j = 1; j < child.Length; j++)
        {
            Destroy(child[j].gameObject);
        }
        for (int i = 0; i < GameManager.AllNuniArray.Length; i++)
        {
            GameObject NuniPannel = Instantiate(NuniPannelPrefab) as GameObject;
            NuniPannel.transform.SetParent(Scroll.transform);
            

            Button NuniButton = NuniPannel.GetComponentInChildren<Button>();
            Image[] image = NuniPannel.GetComponentsInChildren<Image>();
            Text NuniName = NuniPannel.GetComponentInChildren<Text>();

            Card nuni = GameManager.AllNuniArray[i];
            NuniButton.enabled = true;
            image[1].sprite = nuni.GetChaImange();   //���� �̹��� �ֱ�
            NuniPannel.name = nuni.cardImage;        //���� �̸� �ֱ�
            NuniName.text = nuni.cardName;
            NuniPannel.GetComponent<RectTransform>().localScale = new Vector3(2.8f, 2.8f, 0);

            NuniPannel.GetComponent<Card>().SetValue(nuni);


            //OpenNuniInfoPanel();                //���� ���� �ֱ�
        }
    }

    public static void OpenNuniInfoPanel()         //���� ���� �гο� ���� ���� �ֱ�
    {
        
           NuniInfoImages[2].sprite = SelectedNuni.GetChaImange();             //���� �̹��� �ֱ�
        NuniInfoTexts[0].text = SelectedNuni.cardName;              //���� �̸� �ֱ�
        NuniInfoTexts[2].text = SelectedNuni.Info;          //���� ���� �ֱ�
        NuniInfoTexts[4].text = SelectedNuni.Effect;          //���� ����ȿ�� �ֱ�

        isNuniButtonClick = true;



    }
}
