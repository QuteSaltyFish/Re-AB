using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePeople : MonoBehaviour {
    public int time;
    public GameObject[] birthplace = new GameObject[4];
    public GameObject player;
    private Vector3[] birthrotation = new Vector3[4];
    // Use this for initialization
    private void Awake()
    {
        for (int i = 0; i < time; ++i)
            global.addtime();
        birthrotation[0] = new Vector3(0, 0, 0);
        birthrotation[1] = new Vector3(0, 180, 0);
        birthrotation[2] = new Vector3(0, 135, 0);
        birthrotation[3] = new Vector3(0, -45, 0);
    }
    void Start () {
        player.transform.position = birthplace[global.gametime].transform.position;
        player.transform.Rotate(birthrotation[global.gametime]);
        birthplace[global.gametime].SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

	}

}
