using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishTutorialsItemControl : TutorialsItemControl
{
    protected override void Run()
    {
        // Ʃ�丮�� �ϼ� ���
        PlayerPrefs.SetInt("TutorialDone", 1);

        // home scene�� ����.
        SceneManager.LoadScene("Main");


        base.Run();
    }
}
