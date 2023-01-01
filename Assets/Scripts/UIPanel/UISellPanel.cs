using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UISellPanel : UIBase
{
    public Building building;
    public override void Start()
    {
        base.Start();

       // UIPanelName.text = "�ǹ� ����";
       // UIPanelText.text = "�ǹ��� �����Ͻðڽ��ϱ�?";

        if (UIYesBtn!=null)
        {

            UIYesBtn.onClick.AsObservable().Subscribe(_ =>
            {
                Remove(building);
            }).AddTo(this);

        }
        if (UINoBtn!=null)
        {

            UINoBtn.onClick.AsObservable().Subscribe(_ =>
            {
                Destroy(this.gameObject);

            }).AddTo(this);
        }
        if (UICloseBtn!=null)
        {

            UICloseBtn.onClick.AsObservable().Subscribe(_ =>
            {
                Destroy(this.gameObject);
            }).AddTo(this);
        }
    }
    public void Remove(Building building)
    {
        Debug.Log("�ǹ� ����");

        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(building.gameObject.transform.position);
        BoundsInt areaTemp = building.area;
        areaTemp.position = positionInt;
        GridBuildingSystem.current.RemoveArea(areaTemp);





        if (building.Type.Equals(BuildType.Make))     //�������� ��� ��ġX �ٷ� ����
        {
            GameManager.Money += building.Cost[building.Level - 1];          //�ڿ� �ǵ�����
            CanvasManger.AchieveMoney += building.Cost[building.Level - 1];
            GameManager.ShinMoney += building.ShinCost[building.Level - 1];
            CanvasManger.AchieveShinMoney += building.ShinCost[building.Level - 1];
            Destroy(building.transform.gameObject);
        }
        else                                //��ġ�ϰ� ����
        {
            GameManager.Money += building.Cost[building.Level - 1] / 10;          //�ڿ� �ǵ�����
            CanvasManger.AchieveMoney += building.Cost[building.Level - 1] / 10;
            GameManager.ShinMoney += building.ShinCost[building.Level - 1] / 3;
            CanvasManger.AchieveShinMoney += building.ShinCost[building.Level - 1] / 3;

            LoadManager.RemoveBuildingSubject.OnNext(building);           //���� ������ �ִ� �ǹ� ��Ͽ��� ����
            LoadManager.Instance.buildingsave.BuildingReq(BuildingDef.removeValue, building);
            Destroy(building.transform.gameObject);
        }

    }
}
