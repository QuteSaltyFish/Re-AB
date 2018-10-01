using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
public class HitTheDoor : MonoBehaviour {
    public VRTK_BodyPhysics body;
    public GameObject door1;
    public GameObject door2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    void Update () {
        Debug.Log(body.GetCurrentCollidingObject());

        if (body.GetCurrentCollidingObject() == door1 || body.GetCurrentCollidingObject() == door2)
            this.transform.eulerAngles += new Vector3(0, 180, 0);
	}
}
