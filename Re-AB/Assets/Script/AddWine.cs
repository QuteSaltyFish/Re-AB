using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddWine : MonoBehaviour {
    public GameObject obj;
    public int collisiontime;
    public int WineType;
    public WineUsage usewine;
    private Transform water;
	// Use this for initialization
	void Start () {
        collisiontime = 0;
        water = GetComponentInChildren<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void WaterdropStart()
    {
        float movespeed = water.localScale.y/ 10;
        Vector3 moveposition = water.up;
        if (water.localScale.y>0)
        {
            water.position += moveposition * Time.deltaTime * water.lossyScale.y / 10;
            water.localScale += moveposition * Time.deltaTime * movespeed;
        }
    }

    private void WaterdropEnd()
    {

    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == obj)
        {
            this.WaterdropStart();
            if (collisiontime == 0)
                usewine.wineuse.Add(WineType);
        }
    }
}
