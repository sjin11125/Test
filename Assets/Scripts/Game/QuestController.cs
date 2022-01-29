using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public bool[] QuestActive;
    GameObject gridObj;
    int pinkCount;
    public static bool complete;
    void Start()
    {
        gridObj = GameObject.FindGameObjectWithTag("Grid");
        pinkCount = 0;
    }

    private void Update()
    {
        for (int i = 0; i < QuestActive.Length; i++)
        {
            if (QuestActive[0] == true)//����Ʈ Ŭ���� �� ����ȭ�鿡�� QuestActive[0]�� false ����
            {
                GetPink();
            }
        }
    }

    void GetPink()
    {
        if(complete == true)
        {
            for (int i = 0; i < 5; i++)
            {
                int line = gridObj.GetComponent<GridScript>().completeIndexArray[i];
                if (gridObj.GetComponent<GridScript>().colors[line] == "Pink")
                {
                    pinkCount++;
                    print("��ũ��" + pinkCount);
                }
            }
        }
    }
}
