using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class FriendInfoUI : MonoBehaviour
{
    // Start is called before the first frame update
   // FriendInfo FriendInfo;
    public Button GoBtn;          //ģ�� ���� ���� ��ư
    public Button RemoveBtn;          //ģ�� ���� ��ư
    

    public Text FriendName;     //ģ�� �г�
    public Text FriendMessage;     //ģ�� �޼���
    public Image FriendImage;       //ģ�� ����

    public void SetFriendInfo(FriendInfo friendInfo)
    {
        FriendName.text = friendInfo.FriendName;
        //FriendImage.sprite=GameManager.Instance.ima
        //�̹��� �ֱ�

    }
    public void Start()
    {
        GoBtn.OnClickAsObservable().Subscribe(_=> { 

        });
    }
    
}
