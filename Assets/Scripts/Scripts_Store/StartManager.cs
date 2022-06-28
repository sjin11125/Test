using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    public GameObject CharacterPrefab;
    public static Card[] NuNiInformation;
    public static int ItemIndex;
    public static int ChaIndex;
    public GameObject Scroll;       //��ũ�ѿ� content �ֱ�

    public static bool isParsing = false;

    GameObject DogamCha;
    public static Sprite ChaImage_;

    public Text ItemInfoText;
    //int[] itemList = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
   public static Dictionary<int, bool> itemList = new Dictionary<int, bool> {
                                                    {0,false},
                                                    { 1,false},
                                                    { 2,false},
                                                    { 3,false},
                                                    { 4,false},
                                                    { 5,false},
                                                    { 6,false},
                                                    { 7,false},
                                                    { 8,false},
                                                    { 9,false}
                                                        };
    public Sprite[] ItemImages;         //������ �̹�����
    public Sprite LockImage;

    string[] ItemInfos = { "��ġ�� ���� �� ���ϴ� ������ �ϳ� �����Ѵ�. ",
                          "������ ������ �ٸ� ���� ŵ �ϰ� �ٽ� ����� �� �ְ� �Ѵ�.",
                          "��ġ�ϱ� �� ������ ������ ������ �� �ִ�.",
                          "������ ���� ������ �̸� �� �� �ִ�.",
                          "������ ������ �������� ��ü �� �� �ִ�.",
                          "������ ���� ���� ���� ��ġ�ؾ� �ϴ� ������ ��ü�� �� �ִ�.",
                          "��ġ�� ������ ������ �ٲ� �� �ִ�.",//��ġ�� ���� ������ �ٲ� �� �ִ�.
                          "��� ������ ������ ��ü�� �� �ִ� ���� �ϳ� �����Ѵ�.",//��� ������ ���� ��ü�� �� �ִ� �� �ϳ� �����Ѵ�.
                          "��ġ�� ������ ������ ���� ���Ʒ��� ������ �����Ѵ�.",//��ġ�� Ÿ���� ������ ���� ���Ʒ��� Ÿ�������Ѵ�.
                          "��ġ�� ���� �� ������ ���� �翷���� ������ �����Ѵ�."};//��ġ�� Ÿ�� �� ������ ���� �翷���� Ÿ�� �����Ѵ�.
    public Sprite[] ItenImage;
    public Text ItemInfo;
    /* ������ ���
    * 0: ���찳               (Ȳ��)
    * 1: ŵ                   (��)
    * 2: ��������             (û�Һ�)
    * 3: �̸�����             (Ž��)
    * 4: ���ΰ�ħ             (������)
    * 5: <=>                  (������)
    * 6: ����3��              (����)
    * 7: ����3��              (����)
    * 8: ��� ��ü�Ҽ� �ִ� ��(������)
    * 9: ���� ������ �ٲ۴�   (������)
    */
    public static Button[] LockButton;

    public static GameObject Canvas;

    void Awake()
    {
        if (isParsing .Equals( false))
        {
            DicParsingManager DPManager = new DicParsingManager();
            NuNiInformation = DPManager.Parse_character(1);    //���� ���� �Ľ�
            isParsing = true;
        }
        Canvas = GameObject.Find("Canvas");
        StartManager.ChaIndex = 99;
        CharacterOpen();
    }

    void Update()
    {
        if (ChaIndex.Equals(99))
        {
            ItemInfo.text = "";
        }
        else
        ItemInfo.text = ItemInfos[StartManager.ChaIndex];
    }

    public void CharacterOpen()
    {
        Transform[] child = Scroll.GetComponentsInChildren<Transform>();
        for (int i = 1; i < child.Length; i++)
        {
            Destroy(child[i].gameObject);
        }



        for (int j = 0; j < itemList.Count; j++)         //�����ϱ� �� ĳ���� ��Ÿ����
        {
            Card[] NuniArray = GameManager.CharacterList.ToArray();
            for (int i = 0; i < NuniArray.Length; i++)
            {
                if (NuniArray[i].Item .Equals( j))
                {

                    itemList[j] = true;

                 
                }
            }
           



        }
        for (int j = 0; j < itemList.Count; j++)
        {
            DogamCha = Instantiate(CharacterPrefab);
            DogamCha.transform.SetParent(Scroll.transform);

            Transform[] ChaPrefabChilds = DogamCha.GetComponentsInChildren<Transform>();

            //���� ĳ���� ��ư 
            DogamCha.GetComponent<RectTransform>().name = j.ToString();
            DogamCha.GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 1);

            Button DogamChaButton = DogamCha.GetComponent<Button>();
            Image[] image = DogamChaButton.GetComponentsInChildren<Image>();

            if (itemList[j] .Equals( true))
            {
                image[1].sprite = ItenImage[j];//NuNiInformation[j].GetChaImange();   //ĳ���� �̸� �� �޾ƿͼ� �̹��� ã��
                //Instantiate(ItemImages[j], DogamCha.transform);
                //GameManager.Items[j] = true;
            }
            else
            {
                image[1].sprite = LockImage;//NuNiInformation[j].GetChaImange();   //ĳ���� �̸� �� �޾ƿͼ� �̹��� ã��
                //Instantiate(ItemImages[10], DogamCha.transform);
                DogamCha.tag = "Lock";
                //GameManager.Items[j] = false;
            }

           /* string ChaName  = NuNiInformation[j].cardImage;

            CharacterName.text = NuNiInformation[j].cardName;

            NuNiInformation[j].SetChaImage(GameManager.GetCharacterImage(ChaName));
            Debug.Log(NuNiInformation[j].Item);*/

            
        }
        //LockButton = LockButtonList.ToArray();      //��� ��ư ����Ʈ �迭�� ���� �ֱ�
    }





    public void RefreshButtonArray(Character[] CharactersArray)
    {
        List<Button> LockButtonList = new List<Button>();
        LockButton = new Button[CharactersArray.Length];        //
        for (int i = 0; i < CharactersArray.Length; i++)
        {
            if (CharactersArray[i].GetCharacter("isLock") .Equals( "F"))
            {

            }
        }
    }
}
