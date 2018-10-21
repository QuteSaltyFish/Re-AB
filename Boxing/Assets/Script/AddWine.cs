using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddWine : MonoBehaviour {
    public GameObject obj;
    public int collisiontime;
    public int WineType;
    public WineUsage usewine;
    public Transform water;
    private  Vector3 selfposition;
    private Quaternion selfrotation;
	// Use this for initialization
	void Start () {
        selfposition = this.transform.position;
        selfrotation = this.transform.rotation;
        collisiontime = 0;
        ;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == obj)
        {
            collisiontime++;
            if (collisiontime == 0)
                usewine.wineuse.Add(WineType);
            //this.transform.position = selfposition;
            //this.transform.rotation = selfrotation;
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject == obj)
    //    {
    //        this.WaterdropStart();
    //        collisiontime++;
    //        if (collisiontime == 0)
    //            usewine.wineuse.Add(WineType);
    //        this.transform.position = selfposition;
    //        this.transform.rotation = selfrotation;
    //    }
    //}
}
