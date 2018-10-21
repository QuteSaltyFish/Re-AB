using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingDialogs : MonoBehaviour {

    public AudioClip endingA;
    public AudioClip endingB;
    public AudioClip endingC;

    private AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayEndingDialog(string ending){
        switch (ending){
            case "endingA":
                m_audioSource.clip = endingA;
                break;
            case "endingB":
                m_audioSource.clip = endingB;
                break;
            case "endingC":
                m_audioSource.clip = endingC;
                break;
        }
        m_audioSource.Play();
    }
}