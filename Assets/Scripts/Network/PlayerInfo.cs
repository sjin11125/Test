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

    public InputField InfoInput;        //���ټҰ� ����
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < GameManager.AllNuniArray.Length; i++)
        {
            if (GameManager.AllNuniArray[i].Image.name != GameManager.Instance.PlayerUserInfo.Image)
                continue;
            GameManager.ProfileImage = GameManager.AllNuniArray[i].Image;
        }

        if (gameObject.tag .Equals( "Profile"))
        {
            Profile[0].text = GameManager.Instance.PlayerUserInfo.Uid;
            Profile[1].text = GameManager.Instance.PlayerUserInfo.Message;
            for (int i = 0; i < GameManager.AllNuniArray.Length; i++)
            {
                bool isNuni=false;
                foreach (var item in GameManager.Instance.CharacterList)
                {
                    if (item.Value.cardImage.Equals(GameManager.AllNuniArray[i].cardImage))
                    {
                        isNuni = true;
                    }
                }
                if (isNuni)
                {
                    GameObject image = Instantiate(NuniImages, Canvas.transform);
                    Image Nuniimage = image.GetComponent<Image>();
                    Nuniimage.sprite = GameManager.AllNuniArray[i].Image;
                }
            }
           
        }
       
    }

    public void ImageEnroll()       //������ �̹��� ���
    {
        if (gameObject.GetComponent<Image>().sprite != null)
        {
            GameManager.ProfileImage = gameObject.transform.parent.GetComponent<Image>().sprite;
            WWWForm form1 = new WWWForm();
            form1.AddField("order", "setProfileImage");
            form1.AddField("player_nickname", GameManager.NickName);
            form1.AddField("profile_image", GameManager.ProfileImage.name);


            StartCoroutine(ImagePost(form1));                        //���� ��ũ��Ʈ�� �ʱ�ȭ�ߴ��� ��������� ���

        }

        //���� ��ũ��Ʈ�� ������Ʈ

        //���� ���
        GameObject settigPanel = GameObject.FindGameObjectWithTag("SettingPanel");
        settigPanel.GetComponent<AudioController>().Sound[0].Play();
    }

    IEnumerator ImagePost(WWWForm form)
    {

        using (UnityWebRequest www = UnityWebRequest.Post(GameManager.URL, form)) // �ݵ�� using�� ����Ѵ�
        {
            yield return www.SendWebRequest();
     
            
        }

    }
    public void EditInfo()                  //���ټҰ� ����
    {
        GameManager.Instance.PlayerUserInfo.Message = InfoInput.text;

        /*WWWForm form1 = new WWWForm();
        form1.AddField("order", "setProfileInfo");
        form1.AddField("player_nickname", GameManager.NickName);
        form1.AddField("profile_info", InfoInput.text);


        StartCoroutine(ImagePost(form1));*/

        FirebaseLogin.Instance.SetUserInfo(GameManager.Instance.PlayerUserInfo);
    }
    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag.Equals("Profile"))
        {
            gameObject.GetComponent<Image>().sprite = GameManager.ProfileImage;
        
            Profile[0].text = GameManager.Instance.PlayerUserInfo.Uid;
            Profile[1].text = GameManager.Instance.PlayerUserInfo.Message;
        }
        if (gameObject.tag .Equals( "Profile_Image"))
        {
            gameObject.GetComponent<Image>().sprite = GameManager.ProfileImage;
            /*  for (int i = 0; i < GameManager.Instance.CharacterList.Count; i++)
              {
                  if (GameManager.ProfileImage.name.Equals( GameManager.Instance.CharacterList[i].Image.name)
                  {
                      profile_image.sprite = GameManager.Instance.CharacterList[i].Image;
                  }
              }*/
        }
    }
}
