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
    Sprite[] ChaImage;
    Character Cha;
    public static Sprite ChaImage_;
    GameObject Window;

   public static List<int> itemList = new List<int>();
    public Sprite[] ItemImages;
    string[] ItemInfo = {"���찳��","ŵ�̴�","���������̴�","�̸������","���ΰ�ħ�̴�" };
    /* ������ ���
 * 0: ���찳
 * 1: ŵ
 * 2: ��������
 * 3: �̸�����
 * 4: ���ΰ�ħ
 */
    public static Button[] LockButton;

    public static GameObject Canvas;
    // Start is called before the first frame update
    void Start()
    {
        if (isParsing == false)
        {
            DicParsingManager DPManager = new DicParsingManager();
            NuNiInformation = DPManager.Parse_character(1);    //���� ���� �Ľ�
            isParsing = true;
        }
        Canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CharacterOpen()
    {
    
        float p = 0;

        //RefreshButtonArray(NuNiInformation);         //�������� ���� �ǹ��� ���ΰ�ħ

        List<Button> LockButtonList = new List<Button>();       //��� ĳ���͵� ��ư ����ִ� ����Ʈ

        Debug.Log(NuNiInformation.Length);
        for (int j = 0; j < NuNiInformation.Length; j++)         //�����ϱ� �� ĳ���� ��Ÿ����
        {
            if (NuNiInformation[j].Item != 10)
            {
                DogamCha = Instantiate(CharacterPrefab);
                DogamCha.transform.SetParent(Scroll.transform);

                Transform[] ChaPrefabChilds = DogamCha.GetComponentsInChildren<Transform>();

                //���� ĳ���� ��ư 
                DogamCha.GetComponent<RectTransform>().name = j.ToString();

                Button DogamChaButton = DogamCha.GetComponent<Button>();
                Image[] image = DogamChaButton.GetComponentsInChildren<Image>();

                Text CharacterName = ChaPrefabChilds[1].GetComponent<Text>();
                if (NuNiInformation[j].isLock == "F")      //ĳ���Ͱ� ������� ����
                {
                    //Debug.Log(NuNiInformation[j].GetCharacter("Name"));
                    string ChaName;

                    //BuildingPrefabChilds[4].tag = "unLock";
                    ChaName = NuNiInformation[j].cardImage;

                    CharacterName.text = NuNiInformation[j].cardName;

                    NuNiInformation[j].SetChaImage(GameManager.GetCharacterImage(ChaName));

                    image[1].sprite = ItemImages[NuNiInformation[j].Item];//NuNiInformation[j].GetChaImange();   //ĳ���� �̸� �� �޾ƿͼ� �̹��� ã��

                }

                else                            //������� ��� �̹��� �ֱ�
                {
                    //BuildingPrefabChilds[4].tag = "Lock";
                    CharacterName.text = "��贩��";
                    NuNiInformation[j].SetChaImage(GameManager.DogamChaImageData["Lock"]);
                    image[1].sprite = GameManager.DogamChaImageData["Lock"];

                    // DogamChaButton.enabled = false;
                    //DogamChaButton.GetComponent<Image>().sprite = GameManager.DogamChaImageData["Lock"];

                    LockButtonList.Add(DogamChaButton);


                }
            }
        

            
            p += 100f;
        }
        //LockButton = LockButtonList.ToArray();      //��� ��ư ����Ʈ �迭�� ���� �ֱ�

    }





    public void RefreshButtonArray(Character[] CharactersArray)
    {
        List<Button> LockButtonList = new List<Button>();
        LockButton = new Button[CharactersArray.Length];        //
        for (int i = 0; i < CharactersArray.Length; i++)
        {
            if (CharactersArray[i].GetCharacter("isLock") == "F")
            {

            }
        }
    }
}
