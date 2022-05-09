using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuniDialogParsing : MonoBehaviour
{
    public static Character[] Mal;
    TextAsset csvData;
    void Start()
    {
        if (GameManager.nuniDialogParse == false)
        {
            GameManager.nuniDialogParse = true;
        }
    }
    public Card[] Parse_character(int index)                //���� ���� �ҷ��� 
    {
        List<Card> CharacterList = new List<Card>();
        if (index == 1)
        {
            csvData = Resources.Load<TextAsset>("Cha_Dialogue");    //csv���� ������

        }
        string[] data = csvData.text.Split(new char[] { '\n' });    //���� �������� �ɰ�.
        //string[] pro_data;
        Debug.Log(data.Length);


        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] pro_data = data[i].Split(',');

            if (pro_data[0] == "end")
            {
                break;
            }
            Card character = new Card(pro_data[1], pro_data[2], pro_data[3], pro_data[4], pro_data[5], pro_data[6],
                                       pro_data[7], pro_data[8], pro_data[9], pro_data[10], pro_data[11], pro_data[12]);
            //���    /   �̸�  /  ������ /   �̹��� /  ����  /  ����  /  ��   /  ������ /  ����  / ����ȿ��  / �ǹ�  / ��� ȹ�淮
            character.SetChaImage(GameManager.CharacterImageData[pro_data[4]]);
            CharacterList.Add(character);
        }

        for (int i = 0; i < CharacterList.Count; i++)
        {
            string ImageName = CharacterList[i].cardImage;
            if (GameManager.CharacterImageData.ContainsKey(ImageName))
            {
                CharacterList[i].SetChaImage(GameManager.CharacterImageData[ImageName]);     //���� �̹��� �ֱ�     

            }
        }
        return CharacterList.ToArray();

    }
}
