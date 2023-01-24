using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchieveScroll : MonoBehaviour
{
    public Text AchieveName;
    public Text AchieveContext;
    public Text AchieveCount;

    public List<Text> CountText;

    public Button RewardBtn;

    // Start is called before the first frame update
    private void Start()
    {
            
    }
    public void SetData(AchieveInfo Info,int index)
    {
        AchieveName.text = Info.AchieveName;
        AchieveContext.text = Info.Context;
        AchieveCount.text = index.ToString();          //�� ���� ���� �Է�

        for (int i = 0; i < CountText.Count; i++)
        {
            switch (Info.RewardType[i])
            {
                case "Money":
                    CountText[i].text = "����" + System.Environment.NewLine + "+" + Info.Reward[i];
                    break;
                case "ShinMoney":
                    CountText[i].text = "������ ����" + System.Environment.NewLine + "+" + Info.Reward[i];
                    break;
                case "Zem":
                    CountText[i].text = "��" + System.Environment.NewLine + "+" + Info.Reward[i];
                    break;

                default:
                    break;
            }
        }
  
    }

}
