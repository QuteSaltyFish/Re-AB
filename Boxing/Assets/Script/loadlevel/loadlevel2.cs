using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class loadlevel2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        global.loadName = "next";
        Invoke("loadB", 5f);
        global.addtime();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void loadB()
    {
        SceneManager.LoadScene("middle");
    }
}
