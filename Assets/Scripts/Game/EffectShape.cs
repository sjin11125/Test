using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectShape : MonoBehaviour
{
    AudioSource audioSource;
    bool OndeAct = false;
    private void Start()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }
    public void ActivateEffect() //animation add event�� ���
    {
        if(OndeAct == true)
        {
            this.audioSource.Stop();
        }
        else
        {
            this.audioSource.Play();
        }
    }
    public void DeactivateEffect() //animation add event�� ���
    {        
        OndeAct = true;
        Destroy(gameObject);
    }
}
