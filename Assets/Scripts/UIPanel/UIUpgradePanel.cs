using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UniRx;
public class UIUpgradePanel : UIBase
{
    public Text UpgradeTextCost;
    public Text UpgradeTextBefore;
    public Text UpgradeTextAfter;

    public Building building;
    public GameObject NoEffectPanel;
    public GameObject NoMoneyPanel;

    int MoneyCost = 0;
    int ShinMoneyCost = 0;

    private void Start()
    {
        base.Start();

        for (int j = 0; j < GameManager.BuildingArray.Length; j++)
        {
            if (building.Building_Image == GameManager.BuildingArray[j].Building_Image)
            {

                UpgradeTextBefore.text = GameManager.BuildingArray[j].Reward[building.Level - 1].ToString();     //���� �� ȹ�� ��ȭ
                Debug.Log("������: " + GameManager.BuildingArray[j].Reward[building.Level - 1]);

                MoneyCost = GameManager.BuildingArray[j].Cost[building.Level];
                ShinMoneyCost= GameManager.BuildingArray[j].ShinCost[building.Level];

                UpgradeTextAfter.text = GameManager.BuildingArray[j].Reward[building.Level].ToString();                       //���� �� ȹ�� ��ȭ
                Debug.Log("������: " + GameManager.BuildingArray[j].Reward[building.Level - 1]);

                UpgradeTextCost.text = "����: " + MoneyCost.ToString() + ",   ������ ����: " + ShinMoneyCost.ToString() + " �� �Ҹ�˴ϴ�.";
                break;

            }
        }

        if (UIYesBtn != null)
        {

            UIYesBtn.onClick.AsObservable().Subscribe(_ =>
            {
                Upgrade(building);

            }).AddTo(this);

        }
        if (UINoBtn != null)
        {

            UINoBtn.onClick.AsObservable().Subscribe(_ =>
            {
                Destroy(this.gameObject);

            }).AddTo(this);
        }
        if (UICloseBtn != null)
        {

            UICloseBtn.onClick.AsObservable().Subscribe(_ =>
            {
                Destroy(this.gameObject);
            }).AddTo(this);
        }
    }

    public void Upgrade(Building building)
    {
        bool isUp=false;
        if (building.Level < 2)
        {
            if (building.Building_Image == "building_level(Clone)" ||
                   building.Building_Image == "village_level(Clone)" ||
                   building.Building_Image == "flower_level(Clone)")
            {
                Debug.Log("�ش� �ǹ�����");
                for (int i = 0; i < GameManager.CharacterList.Count; i++)
                {
                    if (GameManager.CharacterList[i].cardName == "����������")
                    {
                        Debug.Log("�ش� �����̽�");
                        isUp = true;
                        break;

                    }
                }
            }
            //GameObject UPPannel = Instantiate(UpgradePannel);
            if (building.Building_Image == "syrup_level(Clone)" ||
             building.Building_Image == "fashion_level(Clone)" ||
             building.Building_Image == "school_level(Clone)")
            {
                Debug.Log("�ش� �ǹ�����22");
                for (int i = 0; i < GameManager.CharacterList.Count; i++)
                {
                    if (GameManager.CharacterList[i].cardName == "����Ʈ����")
                    {
                        Debug.Log("�ش� �����̽�222");
                        isUp = true;
                        break;
                    }
                }
            }
            if (isUp == true)               //�ش� ���� ���� �� ���׷��̵� O
            {
                if (GameManager.ShinMoney>=ShinMoneyCost &&           //��ȭ üũ
                    GameManager.Money>=MoneyCost)
                {
                    GameManager.ShinMoney -= ShinMoneyCost;
                    GameManager.Money -= MoneyCost;

                    building.Level += 1;
                    building.BuildingImage.sprite = GameManager.GetDogamChaImage(building.Building_Image+building.Level.ToString());//�ǹ��̹��� �ٲ�
                }
                else                                             //��ȭ�� ����
                {
                    NoMoneyPanel.SetActive(true);
                }
            }
            else               //�ش� ���� ���� �� ���׷��̵� X
            {

                NoEffectPanel.SetActive(true);          //�ش� ���ϰ� ������ �гζ߰�
            }

        }
        return;
    }
}
