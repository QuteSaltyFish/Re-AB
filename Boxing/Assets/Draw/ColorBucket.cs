using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Playtime_Painter;

public class ColorBucket : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject gm = collision.gameObject;
        PainterBall pb = gm.GetComponent<PainterBall>();
        Color c = gameObject.GetComponent<Renderer>().material.color;
        pb.ChangeColor(c);
    }

}
