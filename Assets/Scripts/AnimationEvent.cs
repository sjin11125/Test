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
        if (GameManager.Money>=2000)
        {
            rand.GetComponent<RandomSelect>().ResultSelect();
            GameManager.Money -= 2000;       //2000�� ����
        }
      
       
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
