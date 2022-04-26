using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBoard : MonoBehaviour
{
    TouchScreenKeyboard id;
    string saveID;//���⿡ ����

    TouchScreenKeyboard password;
    string savePassword;//���⿡ ����

    public void OpenIDKeyBoard()
    {
       id= TouchScreenKeyboard.Open("",TouchScreenKeyboardType.Default);
    }
    public void OpenPasswordKeyBoard()
    {
        password = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }

    public void Update()
    {
        if (TouchScreenKeyboard.visible == false && id != null)
        {
            if (id.done)//���� ������
            {
                saveID = id.text;
                id = null;
            }
        }

        if (TouchScreenKeyboard.visible == false && password != null)
        {
            if (password.done)//���� ������
            {
                savePassword = password.text;
                password = null;
            }
        }
    }
}
