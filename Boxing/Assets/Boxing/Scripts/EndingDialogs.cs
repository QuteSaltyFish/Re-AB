<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingDialogs : MonoBehaviour {

    public AudioClip endingA;
    public AudioClip endingB;
    public AudioClip endingC;

=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingDialogs : MonoBehaviour {

    public AudioClip endingA;
    public AudioClip endingB;
    public AudioClip endingC;

>>>>>>> c4d019e4e47b1057d3b94fdfe9e8dbbda2bfc3d0
    private AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = gameObject.GetComponent<AudioSource>();
<<<<<<< HEAD
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
=======
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
>>>>>>> c4d019e4e47b1057d3b94fdfe9e8dbbda2bfc3d0
}