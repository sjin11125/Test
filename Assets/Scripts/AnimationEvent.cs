using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public GameObject rand;
    private GameObject settigPanel;

    private void Awake()
    {
        settigPanel = GameObject.FindGameObjectWithTag("SettingPanel");
    }

    public void NuniActive()
    {
        //ī�� �� ��Ƽ��
        rand.GetComponent<RandomSelect>().ResultSelect();
        GameManager.Money -= 2000;       //500�� ����
    }

    public void NuniAnimationEnd()
    {
        //���� �ִϸ��̼� ����
        settigPanel.GetComponent<AudioController>().Sound[2].Play();
    }

    public void EffectEnd29()
    {
        Destroy(this.gameObject);
    }
}
