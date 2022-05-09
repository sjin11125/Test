using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
public class PlayerInfo : MonoBehaviour                 //�÷��̾� ������ ��ũ��Ʈ
{
    public static string Id;            //�÷��̾� ���̵�
    public static string NickName;      //�÷��̾� �г���
    public static string SheetsNum;     //�÷��̾� �ǹ� ���� ����ִ� �������� ��Ʈ id
    public static string Info;          //���¸޼���

    public Text[] Profile;      //�г�, ���¸޼���

    string[] Friends;       //ģ�� ���(�г���)

    public GameObject NuniImages,Canvas;

    public Image ProfileImage;      //�� ������ �̹���
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.tag == "Profile")
        {
            Profile[0].text = GameManager.NickName;
            Profile[1].text = GameManager.StateMessage;

            for (int i = 0; i < GameManager.CharacterList.Count; i++)
            {
                GameObject image = Instantiate(NuniImages, Canvas.transform);
                Image Nuniimage = image.GetComponent<Image>();
                Nuniimage.sprite = GameManager.CharacterList[i].Image;
            }
        }
       
    }

    public void ImageEnroll()       //������ �̹��� ���
    {
        if (gameObject.GetComponent<Image>().sprite == null)
        {
            Debug.Log("�̹��� ��");
        }
        else
        {
            GameManager.ProfileImage = gameObject.transform.parent.GetComponent<Image>().sprite;
            Debug.Log("image: "+ GameManager.ProfileImage.name);
            WWWForm form1 = new WWWForm();
            form1.AddField("order", "setProfileImage");
            form1.AddField("player_nickname", GameManager.NickName);
            form1.AddField("profile_image", GameManager.ProfileImage.name);


            StartCoroutine(ImagePost(form1));                        //���� ��ũ��Ʈ�� �ʱ�ȭ�ߴ��� ��������� ���

        }

        //���� ��ũ��Ʈ�� ������Ʈ
    }

    IEnumerator ImagePost(WWWForm form)
    {
        Debug.Log("ImagePost");
        using (UnityWebRequest www = UnityWebRequest.Post(GameManager.URL, form)) // �ݵ�� using�� ����Ѵ�
        {
            yield return www.SendWebRequest();
            //Debug.Log(www.downloadHandler.text);
            
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag=="Profile")
        {
            gameObject.GetComponent<Image>().sprite = GameManager.ProfileImage;
        }
        if (gameObject.tag == "Profile_Image")
        {
            gameObject.GetComponent<Image>().sprite = GameManager.ProfileImage;
            /*  for (int i = 0; i < GameManager.CharacterList.Count; i++)
              {
                  if (GameManager.ProfileImage.name== GameManager.CharacterList[i].Image.name)
                  {
                      profile_image.sprite = GameManager.CharacterList[i].Image;
                  }
              }*/
        }
    }
}
