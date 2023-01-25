using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchieveScroll : MonoBehaviour
{
    public Text AchieveName;
    //public Text AchieveContext;
    public Text AchieveCount;

    public List<Text> CountText;
    public Slider CountSlider;

    public Button RewardBtn;
    public Image RewardImage;

    public Sprite IStoneSprite,StoneSprite,ZemSprite;
    // Start is called before the first frame update
    private void Start()
    {
            
    }
    public void SetData(AchieveInfo Info,int index)
    {
        AchieveName.text = Info.AchieveName;
        //AchieveContext.text = Info.Context;
        AchieveCount.text = index.ToString() + "/"+ Info.Count[index].ToString();          //�� ���� ī��Ʈ/�� ī��Ʈ

        CountSlider.maxValue = int.Parse(Info.Count[index]);//�� ī��Ʈ
        CountSlider.minValue= 0;//�� ���� ī��Ʈ

        switch (Info.RewardType[index])
        {
            case "Money":
                RewardImage.sprite = StoneSprite;
                break;
            case "ShinMoney":
                RewardImage.sprite = IStoneSprite;
                break;
            case "Zem":
                RewardImage.sprite = ZemSprite;
                break;

            default:
                break;
        }

       /* for (int i = 0; i < CountText.Count; i++)
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
        }*/
  
    }

}
